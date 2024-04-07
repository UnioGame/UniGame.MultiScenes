namespace UniGame.MultiScene.Runtime
{
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [Serializable]
    public struct SceneHandler
    {
        [FormerlySerializedAs("_name")] 
        [SerializeField]
        public string name;
        
        [FormerlySerializedAs("_guid")] 
        [SerializeField]
        [MultiSceneItem]
#if ODIN_INSPECTOR
        [DrawWithUnity]
#endif
        public string guid;
        
        [FormerlySerializedAs("_isActive")] 
        [SerializeField]
        public bool isActive;
        
        [FormerlySerializedAs("_isLoaded")] 
        [SerializeField]
        public bool isLoaded;
        
        [FormerlySerializedAs("_isAddressables")] 
        [SerializeField]
        public bool isAddressables;
        
        public string Name => name;
        public string Guid => guid;

        public bool IsActive => isActive;
        public bool IsLoaded => isLoaded;
        public bool IsAddressables => isAddressables;

        public SceneHandler(string name, string guid, bool isActive, bool isLoaded,bool isAddressables)
        {
            this.name = name;
            this.guid = guid;

            this.isActive = isActive;
            this.isLoaded = isLoaded;
            this.isAddressables = isAddressables;
        }
    }
}