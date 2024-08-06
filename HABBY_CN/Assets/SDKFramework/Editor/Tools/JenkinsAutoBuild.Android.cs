using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using Debug=UnityEngine.Debug;

namespace SDKFramework.Editor.Tools
{
    public partial class BuildUnityProject
    {
        private static string[] Scenes = { "Assets/SDKFramework/Example/SDKExample.unity" };
        private static string EXPORT_FOLDER = "D:\\UnityWork\\SDKFramework\\HABBY_CN\\AndroidProject";
        private static string macros;
        private static BuildTargetGroup buildTargetGroup = BuildTargetGroup.Android;

        static BuildUnityProject()
        {
            macros = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            macros = "USE_ANTIADDICTION;MRQ";
        }

        [MenuItem("SDKFramework/Jenkins Trigger")]
        public static void PerformAndroidBuild()
        {
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
            
            
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            
            BuildResult result = report.summary.result;

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