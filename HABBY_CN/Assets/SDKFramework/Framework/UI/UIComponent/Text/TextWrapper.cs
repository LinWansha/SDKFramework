using UnityEngine;
using UnityEngine.UI;

namespace SDKFramework.UI
{
    [RequireComponent(typeof(Text))]
    public class TextWrapper:UIText
    {
        private Text UGUIText;

        void Awake()
        {
            UGUIText = GetComponent<Text>();
        }

        public override string text
        {
            get => UGUIText.text;
            set => UGUIText.text = value;
        }

        public override Color color
        {
            get => UGUIText.color;
            set => UGUIText.color = value;
        }

        public override float lineSpacing
        {
            get => UGUIText.lineSpacing;
            set => UGUIText.lineSpacing = value;
        }
        public override float resizeTextMinSize 
        {
            get => UGUIText.resizeTextMinSize;
            set => UGUIText.resizeTextMinSize = (int)value;
        }
        
        public override RectTransform rectTransform => UGUIText.rectTransform;
        public override Material material {
            get => UGUIText.material;
            set => UGUIText.material = value;
        }
    }
}