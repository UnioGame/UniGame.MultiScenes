namespace UniGame.MultiScene.Runtime
{
    using System;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class AssetReferenceMultiScene : AssetReferenceT<MultiSceneAsset>
    {
        public AssetReferenceMultiScene(string guid) : base(guid) {}
    }
}