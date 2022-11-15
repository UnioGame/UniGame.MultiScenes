namespace UniGame.MultiScene.Runtime
{
    using Cysharp.Threading.Tasks;
    using UnityEngine.AddressableAssets;
    using UnityEngine.SceneManagement;

    public static class MultiSceneAssetExtensions
    {
        /// <summary>
        /// Open scenes async by addressable MultiSceneAsset reference with target LoadSceneMode
        /// </summary>
        /// <param name="multiSceneAssetReference">reference to multiscene asset</param>
        /// <param name="loadSceneMode">scene load mode</param>
        public static async UniTask OpenScenesAsync(
            this AssetReferenceT<MultiSceneAsset> multiSceneAssetReference, 
            LoadSceneMode loadSceneMode)
        {
            var multiSceneAsset = await multiSceneAssetReference.LoadAssetAsync<MultiSceneAsset>().ToUniTask();
            await multiSceneAsset.OpenScenesAsync(loadSceneMode);
        }

        /// <summary>
        /// Open scenes async by MultiSceneAsset with target LoadSceneMode
        /// </summary>
        /// <param name="multiSceneAsset">multiscene asset</param>
        /// <param name="loadSceneMode">scene load mode</param>
        public static async UniTask OpenScenesAsync(
            this MultiSceneAsset multiSceneAsset, 
            LoadSceneMode loadSceneMode)
        {
            var sceneHandlers = multiSceneAsset.SceneHandlers;
            var isFirst = true;

            foreach (var handler in sceneHandlers)
            {
                if (!handler.IsLoaded)
                    continue;
                var sceneMode = isFirst && loadSceneMode == LoadSceneMode.Single 
                    ? LoadSceneMode.Single : LoadSceneMode.Additive;
                isFirst = false;

                if (handler.IsAddressables)
                {
                    await LoadAddressablesSceneAsync(handler.Guid, sceneMode);
                }
                else
                {
                    await LoadSceneAsync(handler.Name, sceneMode);
                }
            }

            foreach (var handler in sceneHandlers)
            {
                if (!handler.IsLoaded || !handler.IsActive)
                    continue;
                
                var scene = SceneManager.GetSceneByName(handler.Name);
                SceneManager.SetActiveScene(scene);
            }

        }
        
        private static async UniTask<Scene> LoadAddressablesSceneAsync(string guid, LoadSceneMode loadSceneMode)
        {
            var result = await Addressables.LoadSceneAsync(guid,loadSceneMode).ToUniTask();
            return result.Scene;
        }

        private static async UniTask<Scene> LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            await SceneManager.LoadSceneAsync(sceneName, loadSceneMode).ToUniTask();
            var scene = SceneManager.GetSceneByName(sceneName);
            return scene;
        }
    }
}