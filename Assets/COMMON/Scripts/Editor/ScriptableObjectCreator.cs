using UnityEngine;
using UnityEditor;
using System.IO;

namespace FishingCactus
{
    public static class ScriptableObjectCreator
    {
        public static T CreateAsset<T>( string folder, string name = null )
            where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            string asset_name = name ?? typeof( T ).ToString();

            if ( !Directory.Exists( folder ) )
            {
                Directory.CreateDirectory( folder );
            }

            string asset_path = AssetDatabase.GenerateUniqueAssetPath( Path.Combine( folder, asset_name ) + ".asset" );

            AssetDatabase.CreateAsset( asset, asset_path );

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();

            return asset;
        }
    }

}