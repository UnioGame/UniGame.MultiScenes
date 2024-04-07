using UniGame.MultiScene.Runtime;

namespace UniGame.MultiScene.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEngine;

#if !ODIN_INSPECTOR && !TRI_INSPECTOR
    [CustomEditor(typeof(MultiSceneAsset))]
#endif
    public sealed class MultiSceneAssetEditor : Editor
    {
        [OnOpenAsset(1)]
        private static bool OpenAssetHandler(int instanceId, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceId);
            if (obj is not MultiSceneAsset multiSceneAsset)
                return false;

            MultiSceneTool.Open(multiSceneAsset);
            return true;
        }
        
        [MenuItem("Assets/Create/Multi Scene", priority = 1)]
        private static void Create()
        {
            var multiSceneAsset = CreateMultiSceneAsset();
            ProjectWindowUtil.CreateAsset(multiSceneAsset, "New Multi Scene.asset");
        }

        public static MultiSceneAsset CreateMultiSceneAsset()
        {
            var multiSceneAsset = CreateInstance<MultiSceneAsset>();
            MultiSceneTool.Update(multiSceneAsset);
            return multiSceneAsset;
        }

        public override void OnInspectorGUI()
        {
            var multiSceneAsset = (MultiSceneAsset) target;

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Open", GUILayout.ExpandWidth(false)))
                    multiSceneAsset.Open();
                
                if (GUILayout.Button("Bake Scenes", GUILayout.ExpandWidth(false)))
                    multiSceneAsset.BakeScenes();

                if (GUILayout.Button("Validate", GUILayout.ExpandWidth(false)))
                    multiSceneAsset.Validate();
            }
            
            GUILayout.Label($"{multiSceneAsset.SceneHandlers.Length} Scenes", EditorStyles.boldLabel);
            
            var isDirty = false;

            for (var i = 0; i < multiSceneAsset.SceneHandlers.Length; i++)
            {
                ref var sceneHandler = ref multiSceneAsset.SceneHandlers[i];
                using (new EditorGUILayout.VerticalScope())
                {
                    var scenePath = AssetDatabase.GUIDToAssetPath(sceneHandler.Guid);
                    var fileName = Path.GetFileNameWithoutExtension(scenePath);

                    var activeLabel = sceneHandler.IsActive ? "Yes" : "No";
                    var loadedLabel = sceneHandler.IsLoaded ? "Yes" : "No";
                    var addressableLabel = sceneHandler.IsAddressables ? "Yes" : "No";

                    var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                    var targetAsset = EditorGUILayout.ObjectField(asset, typeof(SceneAsset), false);
                    
                    if (targetAsset != asset)
                    {
                        isDirty = true;
                        sceneHandler.guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(targetAsset));
                        MultiSceneTool.Validate(multiSceneAsset);
                    }

                    GUILayout.Label(fileName, EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    
                    GUILayout.Label($"Path: {scenePath}");
                    GUILayout.Label($"Name: {sceneHandler.Name}");
                    GUILayout.Label($"Active: {activeLabel}");
                    GUILayout.Label($"Loaded: {loadedLabel}");
                    GUILayout.Label($"Is Addressables: {addressableLabel}");
                    
                    EditorGUI.indentLevel--;

                    GUILayout.Space(10.0f);
                }
            }

            if (isDirty) EditorUtility.SetDirty(multiSceneAsset);
        }
    }
}