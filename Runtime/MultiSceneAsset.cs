namespace UniGame.MultiScene.Runtime
{
    using UnityEngine;

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
}