using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SDKFramework.Asset
{
    public partial class AssetModule : BaseModule
    {
        public void LoadScene(string name, LoadSceneMode mode, Action callback)
        {
            StartCoroutine(LoadSceneInternal(name, mode, callback));
        }

        private IEnumerator LoadSceneInternal(string name, LoadSceneMode mode, Action callback)
        {
            yield return SceneManager.LoadSceneAsync(name, mode);
            callback?.Invoke();
        }
    }
}
