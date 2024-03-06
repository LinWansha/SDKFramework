using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using SDKFramework.Asset;
using SDKFramework.Config;

namespace SDKFramework.Editor
{
    public class Generator_Mediator : EditorWindow
    {
        private readonly string FilePath = $"{AssetModule.ConfigPath}UIConfig.json";
        private string viewName = "";
        private GameObject UIPrefab;
        private UIMode UILayer = UIMode.Normal;
        private GUIStyle headerStyle;
        private const string Title = "ViewMediator Script Generator";

        [MenuItem("SDKFramework/Generator ViewMediator")]
        public static void ShowWindow()
        {
            var window = GetWindow<Generator_Mediator>(Title);
            window.maxSize = new Vector2(350, 250);
            window.minSize = window.maxSize;
        }

        private void OnEnable()
        {
            headerStyle = new GUIStyle
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState { textColor = Color.green }
            };
        }

        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(Title, headerStyle);
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical(GUILayout.MaxWidth(250));
            DrawPrefabFieldWithInstructions();
            DrawEnumSelection();
            DisplaySelectedViewName();

            EditorGUILayout.Space();

            DrawGenerateScriptsButton();

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void DrawPrefabFieldWithInstructions()
        {
            GUILayout.Label("Drag in the prefab to instantiate a UI object \n (拖一个UI预制体进来)");
            UIPrefab = (GameObject)EditorGUILayout.ObjectField(UIPrefab, typeof(GameObject), false,
                GUILayout.MinWidth(200), GUILayout.MinHeight(40));
            GUILayout.Space(10);
        }

        private void DrawEnumSelection()
        {
            GUILayout.Label("Configure UI hierarchy (配置UI层级)：");
            UILayer = (UIMode)EditorGUILayout.EnumPopup(UILayer);
            GUILayout.Space(10);
        }

        private void DisplaySelectedViewName()
        {
            if (UIPrefab == null) return;

            viewName = UIPrefab.name.Substring(0, UIPrefab.name.Length - 2);
            GUILayout.Label($"View Name: {viewName}");
        }

        private void DrawGenerateScriptsButton()
        {
            if (!GUILayout.Button("Generate Scripts", GUILayout.MinWidth(180), GUILayout.MinHeight(30))) return;

            if (string.IsNullOrEmpty(viewName))
            {
                Debug.LogError("请先拖入UI预制 再生成脚本！！");
                return;
            }

            GenerateMediator();
        }

        private void GenerateMediator()
        {
            string path = Application.dataPath + "/SDKFramework/UI/" + viewName;
            Directory.CreateDirectory(path);

            AddEnumItemToUIViewID($"{viewName}UI", (viewId) =>
            {
                UIConfig viewInfo = new UIConfig
                {
                    ID = viewId,
                    Description = viewName,
                    Asset = AssetDatabase.GetAssetPath(UIPrefab),
                    Mode = UILayer
                };

                string jsonStr = File.ReadAllText(FilePath);
                List<UIConfig> infoList = JsonConvert.DeserializeObject<List<UIConfig>>(jsonStr);
                infoList.Add(viewInfo);

                File.WriteAllText(FilePath,
                    JsonConvert.SerializeObject(infoList, Formatting.Indented));
            });

            string viewScript =
                $"using SDKFramework.UI;\n\n[UIView(typeof({viewName}Mediator), UIViewID.{viewName}UI)]\npublic class " +
                viewName + "View : UIView\n{\n\n}";

            string mediatorScript = $"using SDKFramework.UI;\n\npublic class " + viewName +
                                    $"Mediator : UIMediator<{viewName}View>\n{{\n\n}}";

            File.WriteAllText(path + "/" + viewName + "View.cs", viewScript);
            File.WriteAllText(path + "/" + viewName + "Mediator.cs", mediatorScript);

            AssetDatabase.Refresh();

            Debug.Log($"Generating {viewName}View and {viewName}Mediator scripts.");
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