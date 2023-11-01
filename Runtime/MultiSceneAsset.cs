namespace UniGame.MultiScene.Runtime
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.Serialization;

    public sealed class MultiSceneAsset : ScriptableObject
    {
        [FormerlySerializedAs("_sceneHandlers")] 
        [SerializeField]
        public SceneHandler[] sceneHandlers = Array.Empty<SceneHandler>();

        public SceneHandler[] SceneHandlers
        {
            get => sceneHandlers;
            set => sceneHandlers = value;
        }
    }

    [Serializable]
    public class MultiScene
    {
        public List<Scene> scenes = new List<Scene>();
    }
}