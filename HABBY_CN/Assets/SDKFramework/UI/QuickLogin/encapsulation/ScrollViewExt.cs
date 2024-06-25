using UnityEngine;
using UnityEngine.UI;

public class ScrollViewExt : MonoBehaviour
{
    [SerializeField] RectTransform scrollRect;
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] GameObject accountItem;
    [SerializeField] GameObject persistentItem;
        
    private float staticHeight;
        
    private void Awake()
    {
            
        staticHeight = gridLayoutGroup.padding.top + 
                       gridLayoutGroup.padding.bottom + 
                       (gridLayoutGroup.spacing.y * -1);

        AdjustContentHeight(); // 确保在创建永久项后调整内容高度
    }
        

    public GameObject AddNewItem()
    {
        GameObject newItem =GameObject.Instantiate(accountItem, gridLayoutGroup.transform);
        persistentItem.transform.SetAsLastSibling(); // 确保常驻的Item始终在最后

        AdjustContentHeight(); // 更新ScrollView高度
        return newItem;
    }
    
    public void RemoveItem(GameObject item)
    {
        // 删除指定的子项
        if (gridLayoutGroup.transform.childCount<=2)
            return;
        if (item != null && item != persistentItem)
        {
            DestroyImmediate(item);
            AdjustContentHeight(); // 更新ScrollView高度
        }
    }

    private void AdjustContentHeight()
    {
        int itemCount = gridLayoutGroup.transform.childCount; // 实时计算子项数量
        // Log.Error($"itemCount:{itemCount}");
        float dynamicHeight = itemCount > 0 ?
            itemCount * gridLayoutGroup.cellSize.y +
            (itemCount - 1) * gridLayoutGroup.spacing.y :
            0; // 没有子项时高度为0
        // Log.Error($"dynamicHeight:{dynamicHeight}");

        scrollRect.sizeDelta = new Vector2(scrollRect.sizeDelta.x,
            staticHeight + dynamicHeight);
    }
        
}