namespace UniGame.MultiScene.Editor
{
    using System.IO;
    using Runtime;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(MultiSceneItemAttribute))]
    public class MultiSceneDrawer  : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var guid = property.stringValue;
            var asset = default(Object);
            if (!string.IsNullOrEmpty(guid))
            {
                var scenePath = AssetDatabase.GUIDToAssetPath(guid);
                asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
            }
            
            var targetAsset = EditorGUI.ObjectField(position,asset, typeof(SceneAsset), false);

            if (targetAsset == asset) return;
            
            var path = AssetDatabase.GetAssetPath(targetAsset);
            var guidValue = AssetDatabase.AssetPathToGUID(path);
            property.stringValue = guidValue;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}