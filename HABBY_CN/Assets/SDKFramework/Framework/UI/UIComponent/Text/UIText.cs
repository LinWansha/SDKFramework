using UnityEngine;

namespace SDKFramework.UI
{
    /// <summary>
    /// Each time a new text component is replaced,
    /// a new text component can be implemented,
    /// you can follow the practice of TextWrapper,
    /// when the implementation is finished,
    /// directly AddComponent your implementation,
    /// There is no need to manually add components that you would otherwise add
    /// </summary>
    public abstract class UIText : MonoBehaviour
    {
        public abstract string text { get; set; }
        
        public abstract Color color { get; set; }
        
        public abstract float lineSpacing { get; set; }
        
        public abstract float resizeTextMinSize { get; set; }
        
        public abstract RectTransform rectTransform { get;}
        
        public abstract Material material { get; set; }
        
        
    }//if need use new property, extension it in this

}
