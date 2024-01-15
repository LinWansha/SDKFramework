using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 作者：mrq
/// 时间：2024/1/3
/// 功能：自动生成具体中介者和具体同事类脚本
/// 目前存在问题：如果用的是Rider，IDE无法识别新生成的脚本，需要重启Rider
/// TODO：扩展，UI属性写入配置表
/// </summary>
public class GenertorMediatorTool : EditorWindow
{
    private string viewName = "";
    private GUIStyle headerStyle;

    [MenuItem("SDKFramework/GeneratorMediator")]
    public static void ShowWindow()
    {
        var window = GetWindow<GenertorMediatorTool>("Script Generator");
        window.maxSize = new Vector2(300, 150);
        window.minSize = window.maxSize;
    }

    private void OnEnable()
    {
        headerStyle = new GUIStyle
        {
            fontSize = 14, 
            fontStyle = FontStyle.Bold, 
            alignment = TextAnchor.MiddleCenter,
            normal = new GUIStyleState(){textColor = Color.green}
        };
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Script Generator", headerStyle);
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical(GUILayout.MaxWidth(250));

        GUILayout.Label("Enter mediator script name");
        viewName = EditorGUILayout.TextField(viewName, GUILayout.MinWidth(200),GUILayout.MinHeight(40));

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Scripts", GUILayout.MinWidth(200)))
        {
            GenerateMediator();
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
            AddEnumItemToUIViewID($"{viewName}UI");
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

    private void AddEnumItemToUIViewID(string newItem)
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
    }
}