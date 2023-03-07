namespace UniGame.MultiScene.Runtime
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public sealed class MultiSceneAsset : ScriptableObject
    {
        [SerializeField]
        private SceneHandler[] _sceneHandlers;

        public SceneHandler[] SceneHandlers
        {
            get => _sceneHandlers;
            set => _sceneHandlers = value;
        }
    }

    [Serializable]
    public class MultiScene
    {
        public List<Scene> scenes = new List<Scene>();
    }
}