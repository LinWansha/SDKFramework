using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDKFramework.Utils
{
    /// <summary>
    /// 文本控件,支持超链接
    /// </summary>
    public class HyperlinkText : Text, IPointerClickHandler
    {
        /// <summary>
        /// 超链接信息类
        /// </summary>
        private class HyperlinkInfo
        {
            public int startIndex;

            public int endIndex;

            public string name;

            public readonly List<Rect> boxes = new List<Rect>();
        }

        /// <summary>
        /// 解析完最终的文本
        /// </summary>
        private string m_OutputText;

        /// <summary>
        /// 超链接信息列表
        /// </summary>
        private readonly List<HyperlinkInfo> m_HrefInfos = new List<HyperlinkInfo>();

        /// <summary>
        /// 文本构造器
        /// </summary>
        protected static readonly StringBuilder s_TextBuilder = new StringBuilder();

        [Serializable]
        public class HrefClickEvent : UnityEvent<string>
        {
        }

        [SerializeField] private HrefClickEvent m_OnHrefClick = new HrefClickEvent();

        /// <summary>
        /// 超链接点击事件
        /// </summary>
        public HrefClickEvent onHrefClick
        {
            get { return m_OnHrefClick; }
            set { m_OnHrefClick = value; }
        }


        /// <summary>
        /// 超链接正则
        /// </summary>
        private static readonly Regex s_HrefRegex =
            new Regex(@"<a href=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

        private HyperlinkText mHyperlinkText;

        protected override void Awake()
        {
            base.Awake();
            mHyperlinkText = GetComponent<HyperlinkText>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            mHyperlinkText.onHrefClick.AddListener(OnHyperlinkTextInfo);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            mHyperlinkText.onHrefClick.RemoveListener(OnHyperlinkTextInfo);
        }


        public override void SetVerticesDirty()
        {
            base.SetVerticesDirty();
#if UNITY_EDITOR
#pragma warning disable CS0618 // Type or member is obsolete
            if (UnityEditor.PrefabUtility.GetPrefabType(this) == UnityEditor.PrefabType.Prefab)
#pragma warning restore CS0618 // Type or member is obsolete
            {
                return;
            }
#endif
            m_OutputText = GetOutputText(text);
        }


        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            var orignText = m_Text;
            m_Text = m_OutputText;
            base.OnPopulateMesh(toFill);
            m_Text = orignText;
            UIVertex vert = new UIVertex();

            // 处理超链接包围框
            foreach (var hrefInfo in m_HrefInfos)
            {
                hrefInfo.boxes.Clear();
                if (hrefInfo.startIndex >= toFill.currentVertCount)
                {
                    continue;
                }

                // 将超链接里面的文本顶点索引坐标加入到包围框
                toFill.PopulateUIVertex(ref vert, hrefInfo.startIndex);
                var pos = vert.position;

                var bounds = new Bounds(pos, Vector3.zero);
                for (int i = hrefInfo.startIndex, m = hrefInfo.endIndex; i < m; i++)
                {
                    if (i >= toFill.currentVertCount)
                    {
                        break;
                    }

                    toFill.PopulateUIVertex(ref vert, i);
                    pos = vert.position;
                    if (pos.x < bounds.min.x) // 换行重新添加包围框
                    {
                        hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                        bounds = new Bounds(pos, Vector3.zero);
                    }
                    else
                    {
                        bounds.Encapsulate(pos); // 扩展包围框
                    }
                }

                hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
            }
        }

        /// <summary>
        /// 获取超链接解析后的最后输出文本
        /// </summary>
        /// <returns></returns>
        protected virtual string GetOutputText(string outputText)
        {
            s_TextBuilder.Length = 0;
            m_HrefInfos.Clear();
            var indexText = 0;
            foreach (Match match in s_HrefRegex.Matches(outputText))
            {
                s_TextBuilder.Append(outputText.Substring(indexText, match.Index - indexText));

                string str = s_TextBuilder.ToString();
                char[] array = str.ToCharArray(); 
                IEnumerator enumerator = array.GetEnumerator(); 
                StringBuilder stringBuilder = new StringBuilder();
                while (enumerator.MoveNext()) //开始枚举
                {
                    if ((char)enumerator.Current != ' ') //向StringBuilder类对象添加非空格字符
                        stringBuilder.Append(enumerator.Current.ToString());
                }

                var group = match.Groups[1];
                var hrefInfo = new HyperlinkInfo
                {
                    startIndex = stringBuilder.Length * 4, // 超链接里的文本起始顶点索引
                    endIndex = (stringBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
                    name = group.Value
                };

                m_HrefInfos.Add(hrefInfo);
                s_TextBuilder.Append("<color=#75A4F8>"); 
                s_TextBuilder.Append(match.Groups[2].Value);
                s_TextBuilder.Append("</color>");
                indexText = match.Index + match.Length;
            }

            s_TextBuilder.Append(outputText.Substring(indexText, outputText.Length - indexText));
            return s_TextBuilder.ToString();
        }

        /// <summary>
        /// 点击事件检测是否点击到超链接文本
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 lp = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position,
                eventData.pressEventCamera, out lp);

            foreach (var hrefInfo in m_HrefInfos)
            {
                var boxes = hrefInfo.boxes;
                for (var i = 0; i < boxes.Count; ++i)
                {
                    if (boxes[i].Contains(lp))
                    {
                        m_OnHrefClick.Invoke(hrefInfo.name);
                        return;
                    }
                }
            }
        }


        private void OnHyperlinkTextInfo(string info)
        {
            Debug.Log("超链接信息：" + info);
            Application.OpenURL(info);
        }
    }
}