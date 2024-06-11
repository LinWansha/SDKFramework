using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBuilder
{
    private Queue<AssetBuildTask> buildTasks = new Queue<AssetBuildTask>();

    public string OutputDir { get; set; } = "AssetBundles";
    public string CopyToDir { get; set; } = "Assets/StreamingAssets";
    public BuildAssetBundleOptions BuildOptions { get; set; } = BuildAssetBundleOptions.None;
    public BuildTarget BuildTarget { get; set; } = BuildTarget.StandaloneWindows64;

    public void AddTask(AssetBuildTask task)
    {
        buildTasks.Enqueue(task);
    }

    public void StartBuild()
    {
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();

        foreach (var task in buildTasks)
        {
            task.Prebuild();
        }

        foreach (var task in buildTasks)
        {
            foreach (var bundle in task.CollectAssetBundles())
            {
                int index = assetBundleBuilds.FindIndex((build) => { return build.assetBundleName == bundle.assetBundleName; });
                if (index >= 0)
                    continue;

                assetBundleBuilds.Add(bundle);
            }
        }

        if (Directory.Exists(OutputDir))
        {
            Directory.Delete(OutputDir, true);
        }
        Directory.CreateDirectory(OutputDir);

        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(OutputDir, assetBundleBuilds.ToArray(), BuildOptions, BuildTarget);
        string[] bundles = manifest.GetAllAssetBundles();
        foreach (var bundle in bundles)
        {
            string bundlePath = $"{OutputDir}/{bundle}";
            byte[] content;
            using (FileStream fs = new FileStream(bundlePath, FileMode.Open, FileAccess.Read))
            {
                content = new byte[(int)fs.Length];
                fs.Read(content, 0, content.Length);
            }
            const int crc = 0;
            byte[] encrypt = new byte[content.Length + crc];
            for (int i = 0; i < crc; i++)
            {
                encrypt[i] = (byte)Random.Range(0, byte.MaxValue + 1);
            }
            System.Array.Copy(content, 0, encrypt, crc, content.Length);
            using (FileStream fs = new FileStream(bundlePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(encrypt, 0, encrypt.Length);
            }
        }

        if (Directory.Exists($"{CopyToDir}/{OutputDir}"))
        {
            Directory.Delete($"{CopyToDir}/{OutputDir}", true);
        }
        foreach (var bundle in manifest.GetAllAssetBundles())
        {
            CopyBundle(bundle);
        }
        CopyBundle(OutputDir);

        foreach (var task in buildTasks)
        {
            task.Postbuild();
        }
    }

    private void CopyBundle(string path)
    {
        // copy bundle
        string fromPath = $"{OutputDir}/{path}";
        string toPath = $"{CopyToDir}/{OutputDir}/{path}";
        string dir = Path.GetDirectoryName(toPath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        File.Copy(fromPath, toPath, true);

        // coppy manifest
        fromPath = $"{fromPath}.manifest";
        toPath = $"{toPath}.manifest";
        File.Copy(fromPath, toPath, true);
    }
}
