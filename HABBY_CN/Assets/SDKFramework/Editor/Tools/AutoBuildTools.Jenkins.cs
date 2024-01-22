using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;


namespace BuildTools
{
    public class BuildTools
    {
    	[InitializeOnLoad]
        public class GlobalConfig
        {
            //自动填入keystore密码工具
            static GlobalConfig()
            {
                PlayerSettings.Android.keystorePass = "XXXXXX";
                PlayerSettings.Android.keyaliasName = "XXX";
                PlayerSettings.Android.keyaliasPass = "XXXXXX";
            }
        }
        public class BuildJson
        {
            public int DownloadPlatform { set; get; }
            public int GameType { set; get; }
        }
        /// <summary>
        /// 用户安装包下载渠道
        /// </summary>
        public enum DownloadPlatform
        {
            TempTest,//内部测试
            TapTap,
            Game4399,
            Steam,
            /// <summary>
            /// 豌豆荚
            /// </summary>
            WanDouJia,
            /// <summary>
            /// 官方群
            /// </summary>
            Official
        }

        /// <summary>
        /// 游戏类型 功能控制开关
        /// </summary>
        public enum GameType
        {
            /// <summary>
            /// 地图版
            /// </summary>
            GameLife,
            /// <summary>
            /// 对战总版
            /// </summary>
            XinMo,
        }


        //得到工程中所有场景名称
        static string[] SCENES = FindEnabledEditorScenes();

        //static string[] Target_SCENES;


        //一系列批量build的操作

        //快易典apk打包
        [MenuItem("BuildTools/正式服/游戏人生")]
        static void GameLife()
        {
            BulidTarget("游戏人生(4399)", GameType.GameLife, DownloadPlatform.Game4399);
            BulidTarget("游戏人生(TapTap)", GameType.GameLife, DownloadPlatform.TapTap);
            BulidTarget("游戏人生(官方)", GameType.GameLife, DownloadPlatform.Official);
            BulidTarget("游戏人生(豌豆荚)", GameType.GameLife, DownloadPlatform.WanDouJia);
        }

        //批量打包apk包
        [MenuItem("BuildTools/正式服/心魔")]
        static void XinMo()
        {
            BulidTarget("心魔(4399)", GameType.XinMo, DownloadPlatform.Game4399);
            BulidTarget("心魔(TapTap)", GameType.XinMo, DownloadPlatform.TapTap);
            BulidTarget("心魔(官方)", GameType.XinMo, DownloadPlatform.Official);
            BulidTarget("心魔(豌豆荚)", GameType.XinMo, DownloadPlatform.WanDouJia);
        }


        static BuildJson buildJson = new BuildJson();

        //这里封装了一个简单的通用方法。
        static void BulidTarget(string apkName, GameType gameType, DownloadPlatform DownloadPlatform)
        {
            buildJson.DownloadPlatform = (int)DownloadPlatform;
            buildJson.GameType = (int)gameType;
            //buildJson.version = "000157";
            WriteLine();//把数据写入项目本地文件，用于项目运行时拿取

            string app_name = apkName;
            //string target_dir = Application.dataPath + "/TargetAndroid";
            string target_dir = "E:/APK_正式服";
            string target_name = app_name + ".apk";
            //环境变量(宏定义)
            string ScriptingDefineSymbols = "VUFORIA_SAMPLE_ORIENTATION_SETTINGS;VUFORIA_ANDROID_SETTINGS;CROSS_PLATFORM_INPUT;MOBILE_INPUT;";
            BuildTargetGroup targetGroup = BuildTargetGroup.Android;
            BuildTarget buildTarget = BuildTarget.Android;

            //string applicationPath = Application.dataPath.Replace("/Assets", "");
            //每次build删除之前的残留
            if (Directory.Exists(target_dir))
            {
                if (File.Exists(target_name))
                {
                    File.Delete(target_name);
                }
            }
            else
            {
                Directory.CreateDirectory(target_dir);
            }
            //==================这里是比较重要的东西，设置打包apk的bundleID以及区分宏定义调用相应代码=======================
            switch (gameType)
            {
                case GameType.GameLife:
                    PlayerSettings.applicationIdentifier = "com.dxzb.xinmo";
                    PlayerSettings.bundleVersion = "1.0";
                    //PlayerSettings.Android.bundleVersionCode = 14;
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, ScriptingDefineSymbols + "Cheng");
                    PlayerSettings.productName = "游戏人生";
                    break;
                case GameType.XinMo:
                    PlayerSettings.applicationIdentifier = "com.dxzb.xinmose";
                    PlayerSettings.bundleVersion = "1.0";
                    //PlayerSettings.Android.bundleVersionCode = 14;
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, ScriptingDefineSymbols + "Cheng;BattleSplit");
                    PlayerSettings.productName = "心魔";
                    break;
                // case GameType.BreakThrough:
                //     PlayerSettings.applicationIdentifier = "com.dxzb.xinmoBT.m4399";
                //     PlayerSettings.bundleVersion = "1.0";
                //     //PlayerSettings.Android.bundleVersionCode = 14;
                //     PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, ScriptingDefineSymbols + "Cheng;BattleSplit");
                //     PlayerSettings.productName = "灵兽闯关";
                //     break;
            }
            PlayerSettings.companyName = "xxx";
            //==================这里是比较重要的东西=======================
            //开始Build场景，等待吧～
            GenericBuild(SCENES, target_dir + "/" + target_name, buildTarget, BuildOptions.None);
        }
        
        static string[] FindEnabledEditorScenes()
        {
            List<string> EditorScenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                EditorScenes.Add(scene.path);
            }
            return EditorScenes.ToArray();
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="scenes">打包场景</param>
        /// <param name="target_dir">APK输出文件夹地址</param>
        /// <param name="build_target">打包平台</param>
        /// <param name="build_options"></param>
        static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
        {
            //EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, build_target);
            BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);

            //string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
            //if (res.Length > 0)
            //{
            //    throw new Exception("BuildPlayer failure: " + res);
            //}
        }

        /// <summary>
        /// 流 编辑器
        /// </summary>
        static void WriteLine()
        {
            StreamWriter writer;
            string filePath = "";
            filePath = Application.streamingAssetsPath + "/BuildJson.json";

            FileInfo fileInfo = new FileInfo(filePath);
            writer = new StreamWriter(filePath, false);
            writer.Write(JsonConvert.SerializeObject(buildJson),false);
            // 释放 流
            writer.Close();
            //writer.Flush();
        }
		/// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="srcPath">需要被拷贝的文件夹路径</param>
        /// <param name="tarPath">拷贝目标路径</param>
        private void CopyFolder(string srcPath, string tarPath)
        {
            if (!Directory.Exists(srcPath))
            {
                Debug.Log("CopyFolder is finish.");
                return;
            }

            if (!Directory.Exists(tarPath))
            {
                Directory.CreateDirectory(tarPath);
            }

            //获得源文件下所有文件
            List<string> files = new List<string>(Directory.GetFiles(srcPath));
            files.ForEach(f =>
            {
                string destFile = Path.Combine(tarPath, Path.GetFileName(f));
                File.Copy(f, destFile, true); //覆盖模式
            });

            //获得源文件下所有目录文件
            List<string> folders = new List<string>(Directory.GetDirectories(srcPath));
            folders.ForEach(f =>
            {
                string destDir = Path.Combine(tarPath, Path.GetFileName(f));
                CopyFolder(f, destDir); //递归实现子文件夹拷贝
            });
        }
    }
}
