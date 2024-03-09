using SDKFramework.Config;
using UnityEngine;

namespace SDKFramework.UI
{
    public abstract class UIMediator<T> : UIMediator where T : UIView
    {
        protected T View;

        protected override void OnShow(object arg)
        {
            base.OnShow(arg);
            View = ViewObject.GetComponent<T>();
        }
        protected override void OnHide()
        {
            base.OnHide();
            View = default;
        }

        protected void Close()
        {
            ModuleDriver.Instance.GetModule<UIModule>().CloseUI(this);
        }

        public override void InitMediator(UIView v)
        {
            base.InitMediator(v);
            View = v as T;
            OnInit();
        }

        protected virtual void OnInit() { }
    }

    public abstract class UIMediator
    {

        public event System.Action OnMediatorHide;
        public GameObject ViewObject { get; set; }
        public int SortingOrder { get; set; }
        public UIMode UIMode { get; set; }

        public virtual void InitMediator(UIView view) { }

        public void Show(GameObject viewObject, object arg)
        {
            ViewObject = viewObject;
            OnShow(arg);
        }
        protected virtual void OnShow(object arg) { }

        public void Hide()
        {
            OnHide();
            OnMediatorHide?.Invoke();
            OnMediatorHide = null;
            ViewObject = default;
        }

        protected virtual void OnHide() { }

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        protected virtual void OnUpdate(float deltaTime) { }
    }
}
