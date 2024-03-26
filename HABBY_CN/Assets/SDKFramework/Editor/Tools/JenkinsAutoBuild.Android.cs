using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using Debug=UnityEngine.Debug;

namespace SDKFramework.Editor.Tools
{
    public partial class BuildUnityProject
    {
        private static string[] Scenes = { "Assets/SDKFramework/Example/SDKExample.unity" }; // 替换为您实际的场景
        private static string EXPORT_FOLDER = "D:\\UnityWork\\SDKFramework\\HABBY_CN\\AndroidProject";
        private static string macros;
        private static BuildTargetGroup buildTargetGroup = BuildTargetGroup.Android; // 请根据需要替换为其他目标平台

        static BuildUnityProject()
        {
            macros = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            macros = "USE_ANTIADDICTION;MRQ";
        }

        [MenuItem("SDKFramework/Jenkins Trigger")]
        public static void PerformAndroidBuild()
        {
            // 获取命令行参数
            string[] args = Environment.GetCommandLineArgs();
            bool enableDebug = false;
            bool isDevBuild = false;
            string versionName = "0.0.0";
            int versionCode = 1;

            foreach (var arg in args)
            {
                Debug.Log("Build arg is " + arg);
                if (arg.StartsWith("-enableDebug"))
                {
                    var tmpParams = arg.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmpParams.Length == 2)
                    {
                        bool.TryParse(tmpParams[1], out enableDebug);
                    }
                }

                if (arg.StartsWith("-dev"))
                {
                    var tmpParams = arg.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmpParams.Length == 2)
                    {
                        bool.TryParse(tmpParams[1], out isDevBuild);
                    }
                }

                if (arg.StartsWith("-VersionName"))
                {
                    var tmpParams = arg.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmpParams.Length == 2)
                    {
                        versionName = tmpParams[1].Trim();
                    }
                }

                if (arg.StartsWith("-VersionCode"))
                {
                    var tmpParams = arg.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmpParams.Length == 2)
                    {
                        int.TryParse(tmpParams[1], out versionCode);
                    }
                }
            }

            Debug.Log($"Enable Debug: {enableDebug}");

            string targetMacros = enableDebug ? macros + ";ENABLE_DEBUG" : macros;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            var targerOptions = BuildOptions.None;
            ModifyMarco(targetMacros);
            ModifyDevlopmentBuild(isDevBuild, out targerOptions);
            SetPackageVersion(versionName, versionCode);
            
            buildPlayerOptions.scenes = Scenes;
            buildPlayerOptions.locationPathName = EXPORT_FOLDER;
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = targerOptions;
            

            // 根据提供的 options 构建项目，并获取构建报告
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

            // 获取构建结果
            BuildResult result = report.summary.result;

            // 根据构建结果进行操作

            if (result != BuildResult.Succeeded)
            {
                throw new Exception("Unity Build Failure");
            }
        }

        private static void SetPackageVersion(string versionName, int versionCode)
        {
            PlayerSettings.bundleVersion = versionName;
            PlayerSettings.Android.bundleVersionCode = versionCode;
            Debug.Log($"SetPackageVersion : Package Version is : {versionName}.{versionCode}");
        }

        private static void ModifyDevlopmentBuild(bool isDevBuild,out BuildOptions options)
        {
            Debug.Log($"ModifyDevlopmentBuild : Dev is :{isDevBuild}");
            if (isDevBuild)
            {
                options = BuildOptions.AcceptExternalModificationsToPlayer;
                options |= BuildOptions.Development;
                options |= BuildOptions.ConnectWithProfiler;
                return;
            }

            options = BuildOptions.None;
        }

        private static void ModifyMarco(string targetMacros)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, targetMacros);
            macros = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            Debug.Log("ModifyMarco : Build macros is : " + macros);
        }

    }
}