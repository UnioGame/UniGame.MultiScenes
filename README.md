# MultiScenes

Multi scene support for Unity3D

- [Getting Started](#getting-started)
- [Usages](#usages)
  - [CreateMultiSceneAsset](#create-multiscene-asset) 
  - [API References](#api-references)
- [License](#license)

# Getting Started

## How to Install

Add to your project manifiest by path [%UnityProject%]/Packages/manifiest.json new Scope:

```json
{
  "scopedRegistries": [
    {
      "name": "UniGame",
      "url": "http://package.unigame.pro:4873/",
      "scopes": [
        "com.unigame",
        "com.littlebigfun",
        "com.alelievr"
      ]
    },
    
    "__comment":"another scoped registers",
    
  ],
}

```

Now install via Package Manager

![](https://github.com/UnioGame/UniGame.MultiScenes/blob/main/GitAssets/install.png)

# Usages

## Create Multiscene Asset

![](https://github.com/UnioGame/UniGame.MultiScenes/blob/main/GitAssets/create3.png)

When your asset is ready you can load a set of game scenes and bake them into asset data

![](https://github.com/UnioGame/UniGame.MultiScenes/blob/main/GitAssets/inspector1.png)

* Open - Load baked scene into workspace
* Update - Bake current loaded scene from worlspace into asset data
* Validate - Validate backed data

## API References

```cs

/// <summary>
/// Open scenes async by addressable MultiSceneAsset reference with target LoadSceneMode
/// </summary>
/// <param name="multiSceneAssetReference">reference to multiscene asset</param>
/// <param name="loadSceneMode">scene load mode</param>
public static async UniTask OpenScenesAsync(
            this AssetReferenceT<MultiSceneAsset> multiSceneAssetReference, 
            LoadSceneMode loadSceneMode)
            
            
/// <summary>
/// Open scenes async by MultiSceneAsset with target LoadSceneMode
/// </summary>
/// <param name="multiSceneAsset">multiscene asset</param>
/// <param name="loadSceneMode">scene load mode</param>
public static async UniTask OpenScenesAsync(
    this MultiSceneAsset multiSceneAsset, 
    LoadSceneMode loadSceneMode)
    
```


# License

MIT
