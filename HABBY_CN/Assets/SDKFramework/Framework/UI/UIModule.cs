using SDKFramework.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using SDKFramework.Asset;
using UnityEngine;
using UnityEngine.UI;

namespace SDKFramework.UI
{
    public partial class UIModule : BaseModule
    {
        public Transform normalUIRoot;
        public Transform modalUIRoot;
        public Transform closeUIRoot;
        //public Camera UICamera;
        public Image imgMask;
        public Text textTip;

        private static Dictionary<UIViewID, Type> MEDIATOR_MAPPING;
        private static Dictionary<UIViewID, Type> ASSET_MAPPING;

        private readonly List<UIMediator> usingMediators = new List<UIMediator>();
        private readonly Dictionary<Type, Queue<UIMediator>> freeMediators = new Dictionary<Type, Queue<UIMediator>>();
        private readonly GameObjectPool<GameObjectAsset> uiObjectPool = new GameObjectPool<GameObjectAsset>();


        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();
            UIConfig.DeserializeByFile();
        }

        protected internal override void OnModuleStop()
        {
            base.OnModuleStop();
        }

        private static void CacheUIMapping()
        {
            if (MEDIATOR_MAPPING != null)
                return;

            MEDIATOR_MAPPING = new Dictionary<UIViewID, Type>();
            ASSET_MAPPING = new Dictionary<UIViewID, Type>();

            Type baseViewType = typeof(UIView);
            foreach (var type in baseViewType.Assembly.GetTypes())
            {
                if (type.IsAbstract)
                    continue;

                if (baseViewType.IsAssignableFrom(type))
                {
                    object[] attrs = type.GetCustomAttributes(typeof(UIViewAttribute), false);
                    if (attrs.Length == 0)
                    {
                        Debug.LogError($"{type.FullName} 没有绑定 Mediator，请使用UIMediatorAttribute绑定一个Mediator以正确使用");
                        continue;
                    }

                    foreach (UIViewAttribute attr in attrs)
                    {
                        MEDIATOR_MAPPING.Add(attr.ID, attr.MediatorType);
                        ASSET_MAPPING.Add(attr.ID, type);
                        break;
                    }
                }
            }
        }

        protected internal override void OnModuleUpdate(float deltaTime)
        {
            base.OnModuleUpdate(deltaTime);
            uiObjectPool.UpdateLoadRequests();
            for (int i = 0; i < usingMediators.Count; i++)
            {
                usingMediators[i].Update(deltaTime);
            }

            UpdateMask(deltaTime);
        }

        private void OnConsoleActive()
        {
            //HabbyFramework.Input.SetEnable(false);
        }

        private void OnConsoleDeactive()
        {
            //HabbyFramework.Input.SetEnable(true);
        }

        private int GetTopMediatorSortingOrder(UIMode mode)
        {
            int lastIndexMediatorOfMode = -1;
            for (int i = usingMediators.Count - 1; i >= 0; i--)
            {
                UIMediator mediator = usingMediators[i];
                if (mediator.UIMode != mode)
                    continue;

                lastIndexMediatorOfMode = i;
                break;
            }

            if (lastIndexMediatorOfMode == -1)
                return mode == UIMode.Normal ? 0 : 1000;

            return usingMediators[lastIndexMediatorOfMode].SortingOrder;
        }

        private UIMediator GetMediator(UIViewID id)
        {
            CacheUIMapping();

            if (!MEDIATOR_MAPPING.TryGetValue(id, out Type mediatorType))
            {
                Debug.LogError($"找不到 {id} 对应的Mediator");
                return null;
            }

            if (!freeMediators.TryGetValue(mediatorType, out Queue<UIMediator> mediatorQ))
            {
                mediatorQ = new Queue<UIMediator>();
                freeMediators.Add(mediatorType, mediatorQ);
            }

            UIMediator mediator;
            if (mediatorQ.Count == 0)
            {
                mediator = Activator.CreateInstance(mediatorType) as UIMediator;
            }
            else
            {
                mediator = mediatorQ.Dequeue();
            }

            return mediator;
        }

        private void RecycleMediator(UIMediator mediator)
        {
            if (mediator == null)
                return;

            Type mediatorType = mediator.GetType();
            if (!freeMediators.TryGetValue(mediatorType, out Queue<UIMediator> mediatorQ))
            {
                mediatorQ = new Queue<UIMediator>();
                freeMediators.Add(mediatorType, mediatorQ);
            }

            mediatorQ.Enqueue(mediator);
        }

        public UIMediator GetOpeningUIMediator(UIViewID id)
        {
            UIConfig uiConfig = UIConfig.ByID((int)id);
            if (uiConfig.IsNull)
                return null;

            UIMediator mediator = GetMediator(id);
            if (mediator == null)
                return null;

            Type requiredMediatorType = mediator.GetType();
            foreach (var item in usingMediators)
            {
                if (item.GetType() == requiredMediatorType)
                    return item;
            }

            return null;
        }

        public void BringToTop(UIViewID id)
        {
            UIMediator mediator = GetOpeningUIMediator(id);
            if (mediator == null)
                return;

            int topSortingOrder = GetTopMediatorSortingOrder(mediator.UIMode);
            if (mediator.SortingOrder == topSortingOrder)
                return;

            int sortingOrder = topSortingOrder + 10;
            mediator.SortingOrder = sortingOrder;

            usingMediators.Remove(mediator);
            usingMediators.Add(mediator);

            Canvas canvas = mediator.ViewObject.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.sortingOrder = sortingOrder;
            }
        }

        public bool IsUIOpened(UIViewID id)
        {
            return GetOpeningUIMediator(id) != null;
        }

        public UIMediator OpenUISingle(UIViewID id, object arg = null)
        {
            UIMediator mediator = GetOpeningUIMediator(id);
            if (mediator != null)
                return mediator;

            return OpenUI(id, arg);
        }

        public UIMediator OpenUI(UIViewID id, object arg = null)
        {
            UIConfig uiConfig = UIConfig.ByID((int)id);
            if (uiConfig.IsNull)
                return null;

            UIMediator mediator = GetMediator(id);
            if (mediator == null)
                return null;

            // GameObject uiObject = (uiObjectPool.LoadGameObject(uiConfig.Asset, (obj) =>
            // {
            //     UIView newView = obj.GetComponent<UIView>();
            //     mediator.InitMediator(newView);
            // })).gameObject;
            
            // 改为每次重新加载创建新实例
            GameObject uiPrefab = HabbyFramework.Asset.LoadAssets<GameObject>(uiConfig.Asset);
            if (uiPrefab == null)
            {
                Debug.LogError($"Failed to load UI Prefab for ID: {id}");
                return null;
            }

            GameObject uiObject = GameObject.Instantiate(uiPrefab);
            UIView newView = uiObject.GetComponent<UIView>();
    
            if (newView == null)
            {
                Debug.LogError($"No UIView component found on the loaded prefab: {uiConfig.Asset}");
                GameObject.Destroy(uiObject);
                return null;
            }

            mediator.InitMediator(newView);
            return OnUIObjectLoaded(mediator, uiConfig, uiObject, arg);
        }

        public IEnumerator OpenUISingleAsync(UIViewID id, object arg = null)
        {
            if (!IsUIOpened(id))
            {
                yield return OpenUIAsync(id, arg);
            }
        }

        public IEnumerator OpenUIAsync(UIViewID id, object arg = null)
        {
            UIConfig uiConfig = UIConfig.ByID((int)id);

            if (uiConfig.IsNull)
                yield break;

            UIMediator mediator = GetMediator(id);
            if (mediator == null)
                yield break;

            bool loadFinish = false;
            uiObjectPool.LoadGameObjectAsync(uiConfig.Asset, (asset) =>
            {
                GameObject uiObject = asset.gameObject;
                OnUIObjectLoaded(mediator, uiConfig, uiObject, arg);
                loadFinish = true;
            }, (obj) =>
            {
                UIView newView = obj.GetComponent<UIView>();
                mediator.InitMediator(newView);
            });
            while (!loadFinish)
            {
                yield return null;
            }

            yield return null;
            yield return null;
        }

        private UIMediator OnUIObjectLoaded(UIMediator mediator, UIConfig uiConfig, GameObject uiObject, object obj)
        {
            if (uiObject == null)
            {
                Debug.LogError($"加载UI失败:{uiConfig.Asset}");
                RecycleMediator(mediator);
                return null;
            }

            UIView view = uiObject.GetComponent<UIView>();
            if (view == null)
            {
                Debug.LogError($"UI Prefab不包含UIView脚本:{uiConfig.Asset}");
                RecycleMediator(mediator);
                uiObjectPool.UnloadGameObject(view.gameObject);
                return null;
            }

            mediator.UIMode = uiConfig.Mode;
            int sortingOrder = GetTopMediatorSortingOrder(uiConfig.Mode) + 10;

            usingMediators.Add(mediator);

            Canvas canvas = uiObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            //canvas.worldCamera = UICamera;
            if (uiConfig.Mode == UIMode.Normal)
            {
                uiObject.transform.SetParentAndResetAll(normalUIRoot);
                canvas.sortingLayerName = "NormalUI";
            }
            else
            {
                uiObject.transform.SetParentAndResetAll(modalUIRoot);
                canvas.sortingLayerName = "ModalUI";
            }

            mediator.SortingOrder = sortingOrder;
            canvas.sortingOrder = sortingOrder;

            mediator.Show(uiObject, obj);
            return mediator;
        }

        public void CloseUI(UIMediator mediator)
        {
            // if (mediator != null)
            // {
            //     // 回收View
            //     uiObjectPool.UnloadGameObject(mediator.ViewObject);
            //     mediator.ViewObject.transform.SetParentAndResetAll(closeUIRoot);
            //
            //     // 回收Mediator
            //     mediator.Hide();
            //     RecycleMediator(mediator);
            //
            //     usingMediators.Remove(mediator);
            // }
            
            if (mediator != null)
            {
                // 销毁ViewObject
                GameObject viewObject = mediator.ViewObject;
                if (viewObject != null)
                {
                    GameObject.Destroy(viewObject);
                }

                // 隐藏Mediator及其他操作
                mediator.Hide();
                usingMediators.Remove(mediator);
            }
        }

        public void CloseAllUI()
        {
            for (int i = usingMediators.Count - 1; i >= 0; i--)
            {
                CloseUI(usingMediators[i]);
            }
        }

        public void CloseUI(UIViewID id)
        {
            UIMediator mediator = GetOpeningUIMediator(id);
            if (mediator == null)
                return;

            CloseUI(mediator);
        }

        public void SetAllNormalUIVisibility(bool visible)
        {
            normalUIRoot.gameObject.SetActive(visible);
        }

        public void SetAllModalUIVisibility(bool visible)
        {
            modalUIRoot.gameObject.SetActive(visible);
        }

        public void ShowMask(float duration = 0.5f)
        {
            destMaskAlpha = 1;
            maskDuration = duration;
        }

        public void HideMask(float? duration = null)
        {
            destMaskAlpha = 0;
            if (duration.HasValue)
            {
                maskDuration = duration.Value;
            }
        }

        private float destMaskAlpha = 0;
        private float maskDuration = 1;

        private void UpdateMask(float deltaTime)
        {
            Color c = imgMask.color;
            c.a = maskDuration > 0
                ? Mathf.MoveTowards(c.a, destMaskAlpha, 1f / maskDuration * deltaTime)
                : destMaskAlpha;
            c.a = Mathf.Clamp01(c.a);
            imgMask.color = c;
            imgMask.enabled = imgMask.color.a > 0;
        }

        public void ShowConsole()
        {
        }
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
sealed class UIViewAttribute : Attribute
{
    public UIViewID ID { get; }
    public Type MediatorType { get; }

    public UIViewAttribute(Type mediatorType, UIViewID id)
    {
        ID = id;
        MediatorType = mediatorType;
    }
}