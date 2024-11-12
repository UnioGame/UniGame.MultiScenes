namespace UniGame.MultiScene.Runtime
{
    using AddressableTools.Runtime;
    using Core.Runtime;
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
        /// <param name="lifeTime"></param>
        public static async UniTask OpenScenesAsync(
            this AssetReferenceT<MultiSceneAsset> multiSceneAssetReference, 
            LoadSceneMode loadSceneMode)
        {
            var multiSceneAsset = await multiSceneAssetReference
                .LoadAssetAsync<MultiSceneAsset>()
                .ToUniTask();
            
            await multiSceneAsset.OpenScenesAsync(loadSceneMode);
        }

        public static bool IsLoaded(this MultiSceneAsset multiSceneAsset)
        {
            var scenes = multiSceneAsset.sceneHandlers;
            
            foreach (var scene in scenes)
            {
                var loadedScene = SceneManager.GetSceneByName(scene.Name);
                if (!loadedScene.isLoaded) return false;
            }

            return true;
        }

        /// <summary>
        /// Open scenes async by MultiSceneAsset with target LoadSceneMode
        /// </summary>
        /// <param name="multiSceneAsset">multiscene asset</param>
        /// <param name="loadSceneMode">scene load mode</param>
        /// <param name="reload"></param>
        public static async UniTask<MultiScene> OpenScenesAsync(
            this MultiSceneAsset multiSceneAsset, 
            LoadSceneMode loadSceneMode, 
            bool reload = true)
        {
            var multiScenes = new MultiScene();
            var sceneHandlers = multiSceneAsset.SceneHandlers;
            var isFirst = true;

            foreach (var handler in sceneHandlers)
            {
                if (handler.IsLoaded) continue;
                
                var sceneMode = isFirst && loadSceneMode == LoadSceneMode.Single 
                    ? LoadSceneMode.Single : LoadSceneMode.Additive;
                isFirst = false;
                
                if(!reload && SceneManager.GetSceneByName(handler.Name).isLoaded)
                    continue;

                if (handler.IsAddressables)
                {
                    await LoadAddressableSceneAsync(handler.Guid, sceneMode);
                }
                else
                {
                    await LoadSceneAsync(handler.Name, sceneMode);
                }
            }

            foreach (var handler in sceneHandlers)
            {
                if (handler.IsLoaded) continue;
                
                var scene = SceneManager.GetSceneByName(handler.Name);
                multiScenes.scenes.Add(scene);
                
                if(!handler.IsActive) continue;
                
                SceneManager.SetActiveScene(scene);
            }

            return multiScenes;
        }
        
        public static async UniTask CloseScenes(this MultiScene multiSceneAsset)
        {
            var scenes = multiSceneAsset.scenes;
            foreach (var scene in scenes)
                await SceneManager.UnloadSceneAsync(scene).ToUniTask();
        }
        
        private static async UniTask<Scene> LoadAddressableSceneAsync(string guid, LoadSceneMode loadSceneMode)
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