using TMPro;
using UnityEngine;

namespace SDKFramework.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPWrapper : UIText
    {
        private TextMeshProUGUI textMeshPro;

        void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        public override string text
        {
            get => textMeshPro.text;
            set => textMeshPro.text = value;
        }

        public override Color color
        {
            get => textMeshPro.color;
            set => textMeshPro.color = value;
        }

        public override float lineSpacing
        {
            get => textMeshPro.lineSpacing;
            set => textMeshPro.lineSpacing = value;
        }
        public override float resizeTextMinSize 
        {
            get => textMeshPro.fontSizeMin ;
            set => textMeshPro.fontSizeMin  = value;
        }
        
        public override RectTransform rectTransform => textMeshPro.rectTransform;
        public override Material material {
            get => textMeshPro.material;
            set => textMeshPro.material = value;
        }
    }
}