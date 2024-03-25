using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace SDKFramework.Account.Utils
{
    public class TextDisaplayHelper
    {
        private static List<TextDisaplayHelper> textDisaplayHelperList;

        public static TextDisaplayHelper GetTextDisaplayHelper()
        {
            if (textDisaplayHelperList == null)
            {
                textDisaplayHelperList = new List<TextDisaplayHelper>();
                textDisaplayHelperList.Add(new TextDisaplayHelper());
            }

            foreach (var item in textDisaplayHelperList)
            {
                if (!item.IsWorking)
                {
                    return item;
                }
            }

            TextDisaplayHelper newHelper = new TextDisaplayHelper();
            textDisaplayHelperList.Add(newHelper);

            return newHelper;
        }

        private bool isWorking = false;

        public bool IsWorking
        {
            get => isWorking;
            // set => isWorking = value;
        }

        /// <summary>
        /// 用于匹配标点符号（正则表达式）
        /// </summary>
        private readonly string strRegex_l = @"(\！|\？|\，|\。|\》|\）|\：|\“|\‘|\、|\；|\+|\-)";

        /// <summary>
        /// 用于匹配标点符号（正则表达式）
        /// </summary>
        private readonly string strRegex_r = @"(\“|\《|\（)";

        /// <summary>
        /// 用于匹配数字（正则表达式）
        /// </summary>
        private readonly string strRegex_i = @"(1|2|3|4|5|6|7|8|9|0|\.)";

        /// <summary>
        /// 用于存储text组件中的内容
        /// </summary>
        private StringBuilder MExplainText = null;

        /// <summary>
        /// 用于存储text生成器中的内容
        /// </summary>
        private IList<UILineInfo> MExpalinTextLine;

        /// <summary>
        /// 整理文字。确保首字母不出现标点
        /// </summary>
        /// <param name="_component">text组件</param>
        /// <param name="_text">需要填入text中的内容</param>
        /// <returns></returns>
        public IEnumerator RearrangingText(Text _component)
        {
            isWorking = true;

            // _component.text = _text;

            //如果直接执行下边方法的话，那么_component.cachedTextGenerator.lines将会获取的是之前text中的内容，而不是_text的内容，所以需要等待一下
            yield return null;

            MExpalinTextLine = _component.cachedTextGenerator.lines;

            //需要改变的字符序号
            //int mChangeIndex = -1;

            MExplainText = new StringBuilder(_component.text);

            for (int i = 1; i < MExpalinTextLine.Count; i++)
            {
                int rCursor;
                if (i < MExpalinTextLine.Count - 1) //最后一行不进行判定
                {
                    rCursor = MExpalinTextLine[i].startCharIdx - 1;

                    //行尾是否有标点
                    if (Regex.IsMatch(_component.text[rCursor].ToString(), strRegex_r))
                    {
                        for (int j = rCursor - 1; j > MExpalinTextLine[i - 1].startCharIdx; j--)
                        {
                            if (!Regex.IsMatch(_component.text[j].ToString(), strRegex_r))
                            {
                                MExplainText.Insert(j + 1, "\n");
                                break;
                            }
                        }
                    }

                    //行尾是否有数字
                    if (Regex.IsMatch(_component.text[rCursor].ToString(), strRegex_i) &&
                        Regex.IsMatch(_component.text[rCursor + 1].ToString(), strRegex_i))
                    {
                        for (int j = rCursor - 1; j > MExpalinTextLine[i - 1].startCharIdx; j--)
                        {
                            if (!Regex.IsMatch(_component.text[j].ToString(), strRegex_i) &&
                                !Regex.IsMatch(_component.text[j].ToString(), strRegex_r))
                            {
                                MExplainText.Insert(j + 1, "\n");
                                break;
                            }
                        }
                    }
                }


                //首位是否有标点
                int lCursor = MExpalinTextLine[i].startCharIdx;
                if (lCursor < _component.text.Length && Regex.IsMatch(_component.text[lCursor].ToString(), strRegex_l))
                {
                    for (int j = lCursor - 1; j > MExpalinTextLine[i - 1].startCharIdx; j--)
                    {
                        if (!Regex.IsMatch(_component.text[j].ToString(), strRegex_l) &&
                            !Regex.IsMatch(_component.text[j - 1].ToString(), strRegex_r))
                        {
                            MExplainText.Insert(j, "\n");
                            break;
                        }
                    }
                }
            }

            _component.text = MExplainText.ToString();

            //_component.text = _text;
            isWorking = false;
        }
    }
}