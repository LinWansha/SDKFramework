#if UNITY_EDITOR

using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

namespace SDKFramework.Utils.LogPro
{
    public class LogIntercepter
    {
        private static string GetStackTrace()
        {
            var editorWindowAssembly = typeof(EditorWindow).Assembly;
            var consoleWindowType = editorWindowAssembly.GetType("UnityEditor.ConsoleWindow");

            var consoleWindowFieldInfo = consoleWindowType.GetField(
                "ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);

            var consoleWindow = consoleWindowFieldInfo.GetValue(null) as EditorWindow;

            if (consoleWindow != EditorWindow.focusedWindow)
            {
                return null;
            }

            var activeTextFieldInfo = consoleWindowType.GetField(
                "m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);

            return (string)activeTextFieldInfo.GetValue(consoleWindow);
        }


        //用正则来匹配对应的文件名和行号
        private static readonly Regex LogRegex = new Regex(@" \(at (.+)\:(\d+)\)\r?\n");

        //Log类所在的脚本名称，用来比较调用信息，判断log信息是否由自定义Log类打印
        private static readonly string KeyCs = "x.cs";


        [OnOpenAsset(0)]
        static bool ReposLog(int instanceID, int line)
        {
            var trackInfo = GetStackTrace();

            if (string.IsNullOrEmpty(trackInfo) || !trackInfo.Contains(KeyCs)) return false;

            var match = LogRegex.Match(trackInfo);
            if (!match.Success) return false;

            match = match.NextMatch();
            if (!match.Success) return false;

            var file = match.Groups[1].Value;
            var lineId = int.Parse(match.Groups[2].Value);

            InternalEditorUtility.OpenFileAtLineExternal(
                Path.Combine(GetProjectPath(), file), lineId);
            return true;
        }


        private static string GetProjectPath()
        {
            var di = new DirectoryInfo(Application.dataPath);
            return di.Parent.FullName;
        }
    }
}
#endif