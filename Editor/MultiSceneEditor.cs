using UniGame.MultiScene.Runtime;
using UnityEditor.AddressableAssets;

namespace UniGame.MultiScene.Editor
{
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    [CustomEditor(typeof(MultiSceneAsset))]
    public sealed class MultiSceneAssetEditor : Editor
    {
        [OnOpenAsset(1)]
        private static bool OpenAssetHandler(int instanceId, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceId);
            if (!(obj is MultiSceneAsset MultiSceneAsset))
                return false;

            Open(MultiSceneAsset);
            return true;
        }
        
        [MenuItem("Assets/Create/Multi Scene", priority = 1)]
        private static void Create()
        {
            var MultiSceneAsset = CreateMultiSceneAsset();
            ProjectWindowUtil.CreateAsset(MultiSceneAsset, "New Multi Scene.asset");
        }

        public static MultiSceneAsset CreateMultiSceneAsset()
        {
            var MultiSceneAsset = CreateInstance<MultiSceneAsset>();
            Update(MultiSceneAsset);
            return MultiSceneAsset;
        }

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
                handles[i] = CreateHandle(handler.Guid, handler.IsActive, handler.IsLoaded);
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
        
        private static SceneSetup[] ToSceneSetup(MultiSceneAsset MultiSceneAsset)
        {
            return MultiSceneAsset.SceneHandlers.Select(ToSceneSetup).Where(IsSceneValid).ToArray();
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
                isLoaded = sceneHandler.IsLoaded
            };

            return sceneSetup;
        }

        private static void Open(MultiSceneAsset MultiSceneAsset)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            
            var scenes = ToSceneSetup(MultiSceneAsset);
            EditorSceneManager.RestoreSceneManagerSetup(scenes);
        }

        public override void OnInspectorGUI()
        {
            var MultiSceneAsset = (MultiSceneAsset) target;

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Open", GUILayout.ExpandWidth(false)))
                    Open(MultiSceneAsset);
                
                if (GUILayout.Button("Update", GUILayout.ExpandWidth(false)))
                {
                    var confirm = EditorUtility.DisplayDialog("Update Existing MultiSceneAsset?", "Are you sure you want to overwrite the existing multi scene?", "Update", "Cancel");
                    if (confirm)
                    {
                        Undo.RecordObject(target, "Update MultiSceneAsset");
                        EditorUtility.SetDirty(target);
                        
                        Update(MultiSceneAsset);
                    }
                }

                if (GUILayout.Button("Validate", GUILayout.ExpandWidth(false)))
                    Validate(MultiSceneAsset);

            }
            
            GUILayout.Label($"{MultiSceneAsset.SceneHandlers.Length} Scenes", EditorStyles.boldLabel);
            foreach (var sceneHandler in MultiSceneAsset.SceneHandlers)
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    var scenePath = AssetDatabase.GUIDToAssetPath(sceneHandler.Guid);
                    var fileName = Path.GetFileNameWithoutExtension(scenePath);

                    var activeLabel = sceneHandler.IsActive ? "Yes" : "No";
                    var loadedLabel = sceneHandler.IsLoaded ? "Yes" : "No";
                    var addressablesLabel = sceneHandler.IsAddressables ? "Yes" : "No";
                    
                    GUILayout.Label(fileName, EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    GUILayout.Label($"Path: {scenePath}");
                    GUILayout.Label($"Name: {sceneHandler.Name}");
                    GUILayout.Label($"Active: {activeLabel}");
                    GUILayout.Label($"Loaded: {loadedLabel}");
                    GUILayout.Label($"Is Addressables: {addressablesLabel}");
                    EditorGUI.indentLevel--;
                    
                    GUILayout.Space(10.0f);
                }
            }
        }
    }
}