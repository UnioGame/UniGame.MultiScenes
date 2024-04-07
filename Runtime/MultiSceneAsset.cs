namespace UniGame.MultiScene.Runtime
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.Serialization;
    
#if UNITY_EDITOR
    using UnityEditor;
#endif

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

#if TRI_INSPECTOR
    using TriInspector;
#endif
    
    public sealed class MultiSceneAsset : ScriptableObject
    {
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [InlineProperty]
#endif
        [FormerlySerializedAs("_sceneHandlers")] 
        [SerializeField]
        public SceneHandler[] sceneHandlers = Array.Empty<SceneHandler>();

        public SceneHandler[] SceneHandlers
        {
            get => sceneHandlers;
            set => sceneHandlers = value;
        }
        
        public bool HasScene(string sceneName)
        {
            for (var i = 0; i < sceneHandlers.Length; i++)
            {
                var sceneHandler = sceneHandlers[i];
                if (sceneHandler.name == sceneName)
                    return true;
            }

            return false;
        }

#if UNITY_EDITOR

#if ODIN_INSPECTOR || TRI_INSPECTOR
        [Button]
#endif
        public void Open()
        {
            MultiSceneTool.Open(this);
        }

#if ODIN_INSPECTOR || TRI_INSPECTOR
        [Button]
#endif
        public void BakeScenes()
        {
            var confirm = EditorUtility
                .DisplayDialog("Update Existing MultiSceneAsset?", "Are you sure you want to overwrite the existing multi scene?", "Update", "Cancel");
            if (!confirm) return;
            
            Undo.RecordObject(this, "Update MultiSceneAsset");
            EditorUtility.SetDirty(this);
            MultiSceneTool.Update(this);
        }
        
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [Button]
#endif
        public void Validate()
        {
            MultiSceneTool.Validate(this);
        }
        
#endif
    }

    [Serializable]
    public class MultiScene
    {
        public List<Scene> scenes = new List<Scene>();
    }
}