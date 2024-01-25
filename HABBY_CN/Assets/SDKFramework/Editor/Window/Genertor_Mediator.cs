namespace SDKFramework.Editor
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using Newtonsoft.Json;
    using Config;

    /// <summary>
    /// 作者：mrq
    /// 时间：2024/1/3
    /// 功能：自动生成具体中介者和具体同事类脚本
    /// </summary>
    public class Genertor_Mediator : EditorWindow
    {
        private string viewName = "";
        private string prefabPath = "";
        private GameObject UIPrefab;
        private UIMode UILayer = UIMode.Normal;
        private GUIStyle headerStyle;
        private const string Tittle = "ViewMediator Script Generator";

        [MenuItem("SDKFramework/Generator ViewMediator")]
        public static void ShowWindow()
        {
            var window = GetWindow<Genertor_Mediator>(Tittle);
            window.maxSize = new Vector2(400, 250);
            window.minSize = window.maxSize;
        }

        private void OnEnable()
        {
            headerStyle = new GUIStyle
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState() { textColor = Color.green }
            };
        }

        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(Tittle, headerStyle);
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical(GUILayout.MaxWidth(250));

            GUILayout.Label("Drag in the prefab to instantiate a UI object\n(拖一个UI预制体进来)");
            // 能够接受拖放进入的对象
            UIPrefab = (GameObject)EditorGUILayout.ObjectField(UIPrefab, typeof(GameObject), false,
                GUILayout.MinWidth(200), GUILayout.MinHeight(40));
            GUILayout.Space(10);

            GUILayout.Label("Configure UI hierarchy(配置UI层级):");
            // 使用EnumPopup创建下拉菜单
            UILayer = (UIMode)EditorGUILayout.EnumPopup(UILayer);
            GUILayout.Space(10);

            if (UIPrefab != null)
            {
                viewName = UIPrefab.name.Substring(0, UIPrefab.name.Length - 2);
                prefabPath = AssetDatabase.GetAssetPath(UIPrefab);
                GUILayout.Label($"View Name: {viewName}");
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Generate Scripts", GUILayout.MinWidth(180), GUILayout.MinHeight(30)))
            {
                if (!string.IsNullOrEmpty(viewName))
                {
                    GenerateMediator();
                }
                else
                {
                    Debug.LogError("请先拖入UI预制 再生成脚本！！");
                }
            }

            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
        }

        private void GenerateMediator()
        {
            string
                path = Application.dataPath + "/SDKFramework/UI/" +
                       viewName; //EditorUtility.SaveFolderPanel("Select Folder to Save Scripts", "", "");
            Directory.CreateDirectory(path);
            if (!string.IsNullOrEmpty(path))
            {
                //先添加UIViewID
                AddEnumItemToUIViewID($"{viewName}UI", (viewId) =>
                {
                    UIConfig viewInfo = new UIConfig();
                    viewInfo.ID = viewId;
                    viewInfo.Description = viewName;
                    viewInfo.Asset = prefabPath;
                    viewInfo.Mode = UILayer;
                    string jsonStr = File.ReadAllText("Assets/StreamingAssets/SDKConfig/UIConfig.json");
                    List<UIConfig> infolist = JsonConvert.DeserializeObject<List<UIConfig>>(jsonStr);
                    infolist.Add(viewInfo);

                    File.WriteAllText("Assets/StreamingAssets/SDKConfig/UIConfig.json",
                        JsonConvert.SerializeObject(infolist, Formatting.Indented));
                });
                //生成脚本写入文件时 继承抽象中介者或抽象同事
                string viewScript =
                    $"using SDKFramework.UI;\n\n[UIView(typeof({viewName}Mediator), UIViewID.{viewName}UI)]\npublic class " +
                    viewName + "View : UIView\n{\n\n}";
                string mediatorScript = $"using SDKFramework.UI;\n\npublic class " + viewName +
                                        $"Mediator : UIMediator<{viewName}View>\n{{\n\n}}";
                File.WriteAllText(path + "/" + viewName + "View.cs", viewScript);
                File.WriteAllText(path + "/" + viewName + "Mediator.cs", mediatorScript);
                Debug.Log(path);
                AssetDatabase.Refresh();
                Debug.Log($"Generating {viewName}View and {viewName}Mediator scripts.");
            }
            else
            {
                Debug.LogError("Error: Invalid folder path.");
            }
        }

        private void AddEnumItemToUIViewID(string newItem, Action<int> callback)
        {
            // Replace this with the path to your UIViewID.cs file
            string path = Application.dataPath + "/SDKFramework/Framework/UI/UIViewID.cs";

            string[] lines = File.ReadAllLines(path);
            List<string> updatedLines = new List<string>();

            int lastEnumIntValue = 0;
            bool foundEnumEnd = false;

            foreach (string line in lines)
            {
                if (line.Trim() == "}")
                {
                    foundEnumEnd = true;

                    string newLine = "    " + newItem + " = " + (lastEnumIntValue + 1) + ",\n";
                    updatedLines.Add(newLine);
                }

                if (!foundEnumEnd)
                {
                    string[] enumItems = line.Split(new[] { ',', '=' }, StringSplitOptions.RemoveEmptyEntries);

                    if (enumItems.Length >= 2)
                    {
                        string lastEnumValue = enumItems[enumItems.Length - 1].Trim();
                        lastEnumIntValue = int.Parse(lastEnumValue);
                    }
                }

                updatedLines.Add(line);
            }

            if (!foundEnumEnd)
            {
                Debug.LogError("Error: Could not find the end of the UIViewID enum.");
                return;
            }

            string newContent = string.Join("\n", updatedLines);

            File.WriteAllText(path, newContent);
            callback?.Invoke(lastEnumIntValue + 1);
        }
    }
}