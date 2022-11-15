namespace UniGame.MultiScene.Runtime
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct SceneHandler
    {
        [SerializeField]
        private string _name;
        [SerializeField]
        private string _guid;
        [SerializeField]
        private bool _isActive;
        [SerializeField]
        private bool _isLoaded;
        [SerializeField]
        private bool _isAddressables;
        
        public string Name => _name;
        public string Guid => _guid;

        public bool IsActive => _isActive;
        public bool IsLoaded => _isLoaded;
        public bool IsAddressables => _isAddressables;

        public SceneHandler(string name, string guid, bool isActive, bool isLoaded,bool isAddressables)
        {
            _name = name;
            _guid = guid;

            _isActive = isActive;
            _isLoaded = isLoaded;
            _isAddressables = isAddressables;
        }
    }
}