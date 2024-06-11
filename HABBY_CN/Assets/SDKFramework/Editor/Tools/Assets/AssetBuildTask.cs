using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class AssetBuildTask
{
    public string[] BuildFolders { get; set; }
    public string[] BlackListFolders { get; set; }
    public string Extension { get; set; } = "*";
    public Action<AssetBuildTask> PrebuildCallback { get; set; }
    public Action<AssetBuildTask> PostbuildCallback { get; set; }

    public List<AssetBundleBuild> CollectAssetBundles()
    {
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        string[] guids = AssetDatabase.FindAssets("*", BuildFolders);
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (asset is DefaultAsset)
                continue;

            if (IsInBlackList(path))
                continue;

            if (Extension != "*")
            {
                string extension = Path.GetExtension(path);
                if (Extension != extension)
                    continue;
            }

            string assetBundleName = path;
            if (path.ToLower().StartsWith("assets/"))
            {
                assetBundleName = path.Remove(0, "assets/".Length);
            }
            AssetBundleBuild build = new AssetBundleBuild()
            {
                assetBundleName = assetBundleName,
                assetNames = new string[] { path },
            };
            builds.Add(build);
        }

        return builds;
    }

    private bool IsInBlackList(string path)
    {
        if (BlackListFolders == null)
            return false;

        foreach (var blackList in BlackListFolders)
        {
            if (path.StartsWith(blackList))
                return true;
        }

        return false;
    }

    public void Prebuild()
    {
        PrebuildCallback?.Invoke(this);
    }

    public void Postbuild()
    {
        PostbuildCallback?.Invoke(this);
    }
}
