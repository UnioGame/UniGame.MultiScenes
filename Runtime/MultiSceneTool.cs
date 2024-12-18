namespace UniGame.MultiScene.Runtime
{
    using System.Linq;
    using UnityEngine;
    using System.IO;
    
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEditor.AddressableAssets;
#endif
    
    public static class MultiSceneTool
    {
#if UNITY_EDITOR
        public static void Update(MultiSceneAsset MultiSceneAsset)
        {
            var setup = EditorSceneManager.GetSceneManagerSetup();
            MultiSceneAsset.SceneHandlers = UpdateSceneHandlers(setup);
            EditorUtility.SetDirty(MultiSceneAsset);
        }

        

        public static void Validate(MultiSceneAsset MultiSceneAsset)
        {
            var handles = MultiSceneAsset.SceneHandlers;
            for (var i = 0; i < MultiSceneAsset.SceneHandlers.Length; i++)
            {
                var handler = MultiSceneAsset.SceneHandlers[i];
                handles[i] = CreateHandle(handler.Guid, handler.IsActive, handler.loadScene);
            }
            EditorUtility.SetDirty(MultiSceneAsset);
        }

        private static SceneHandler[] UpdateSceneHandlers(SceneSetup[] sceneSetups)
        {
            var handlers = new SceneHandler[sceneSetups.Length];
            for (var i = 0; i < sceneSetups.Length; i++)
            {
                var scene = sceneSetups[i];
                var guid = AssetDatabase.AssetPathToGUID(scene.path);
                var handle = CreateHandle(guid, scene.isActive, scene.isLoaded);
                handlers[i] = handle;
            }

            return handlers;
        }

        private static SceneHandler CreateHandle(string guid,bool isActive, bool isLoaded)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            var isAddressablesAsset = IsAddressablesAsset(guid);
            var handle = new SceneHandler(asset.name,guid,isActive, isLoaded,isAddressablesAsset);
            return handle;
        }

        private static bool IsAddressablesAsset(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return false;

            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var entry = addressableSettings.FindAssetEntry(guid);
            return entry != null;
        }
        
        public static void Open(MultiSceneAsset multiSceneAsset)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            
            var scenes = MultiSceneTool.ToSceneSetup(multiSceneAsset);
            EditorSceneManager.RestoreSceneManagerSetup(scenes);
        }
        
        public static SceneSetup[] ToSceneSetup(MultiSceneAsset MultiSceneAsset)
        {
            return MultiSceneAsset.SceneHandlers
                .Select(ToSceneSetup)
                .Where(IsSceneValid)
                .ToArray();
        }

        private static bool IsSceneValid(SceneSetup sceneSetup)
        {
            if (string.IsNullOrEmpty(sceneSetup.path))
                return false;

            if (!File.Exists(sceneSetup.path))
                return false;

            return true;
        }

        private static SceneSetup ToSceneSetup(SceneHandler sceneHandler)
        {
            var sceneSetup = new SceneSetup
            {
                path = AssetDatabase.GUIDToAssetPath(sceneHandler.Guid),
                isActive = sceneHandler.IsActive,
                isLoaded = sceneHandler.loadScene
            };

            return sceneSetup;
        }
#endif
    }
}