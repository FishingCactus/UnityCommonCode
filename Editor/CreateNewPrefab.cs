using UnityEngine;
using UnityEditor;

public class CreateNewPrefab : EditorWindow
{
    // -- PRIVATE

    static string PrefabPathLocalKey = "FC/CommonCode/NewPrefabPath";

    static string PrefabCreationPath = "Assets/_Content/Prefabs/Assets/";
    static string PrefabPrefix = "P_";
    static string PrefabExtension = ".prefab";

    static void CreatePrefabs(
        bool can_be_variant,
        bool put_in_empty_parent,
        bool it_must_propose_path_selection
        )
    {
        string previous_path = EditorPrefs.GetString( PrefabPathLocalKey, PrefabExtension );
        string destination_path = string.Empty;

        if( it_must_propose_path_selection )
        {
            destination_path = EditorUtility.OpenFolderPanel( "Destination Path :", PrefabCreationPath, "" );

            if( string.IsNullOrEmpty( destination_path )
                || !AssetDatabase.IsValidFolder( destination_path )
                )
            {
                Debug.LogWarning( $"[CreatePrefabs] : Invalid path : \"{destination_path}\"." );

                return;
            }

            if( destination_path.StartsWith( Application.dataPath ) )
            {
                destination_path = "Assets" + destination_path.Substring( Application.dataPath.Length );
            }

            destination_path += "/";

            EditorPrefs.SetString( PrefabPathLocalKey, destination_path );
        }
        else
        {
            destination_path = PrefabCreationPath;
        }

        GameObject[] object_table = Selection.gameObjects;

        foreach( GameObject game_object in object_table )
        {
            GameObject new_prefab;

            if( put_in_empty_parent )
            {
                new_prefab = new GameObject( game_object.name );
                new_prefab.name = game_object.name.Replace( "(Clone)", "" );

                game_object.name = game_object.name.Replace( PrefabPrefix, "" );

                GameObject game_object_as_child = Instantiate( game_object, new_prefab.transform );
                game_object_as_child.name = game_object_as_child.name.Replace( "(Clone)", "" );
                game_object_as_child.transform.position = Vector3.zero;
            }
            else
            {
                new_prefab = game_object;
            }

            if( !new_prefab.name.StartsWith(PrefabPrefix) )
            {
                new_prefab.name = PrefabPrefix + game_object.name;
            }

            string local_path = destination_path + new_prefab.name + PrefabExtension;

            if( AssetDatabase.LoadAssetAtPath( local_path, typeof(GameObject)) )
            {
                if( !EditorUtility.DisplayDialog("Are you sure?", "The prefab already exists. Do you want to overwrite it?", "Yes", "No") )
                {
                    return;
                }
            }

            if( can_be_variant )
            {
                Debug.Log( new_prefab.name + " prefab variant created" );
                CreateVariant( new_prefab, local_path );
            }
            else
            {
                Debug.Log( new_prefab.name + " prefab created" );
                CreateNew( new_prefab, local_path );
            }

            if( put_in_empty_parent )
            {
                new_prefab.transform.position = game_object.transform.position;
                 
                DestroyImmediate( game_object );
            }
        }
    }

    static void CreateNew( 
        GameObject new_object, 
        string local_path 
        )
    {
        PrefabUtility.SaveAsPrefabAsset(new_object, local_path);
    }

    static void CreateVariant(
        GameObject new_object,
        string local_path
        )
    {
        PrefabUtility.SaveAsPrefabAssetAndConnect( new_object, local_path, InteractionMode.UserAction );
    }

    // -- UNITY

    [MenuItem("FishingCactus/Tools/Create New Prefab/New or Variant", true)]
    static bool ValidateCreatePrefabOrVariant()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/New or Variant")]
    static void CreatePrefabOrVariant()
    {
        CreatePrefabs( true, false, true );
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/Force new (no variant)", true)]
    static bool ValidateCreatePrefab()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/Force new (no variant)")]
    static void CreatePrefab()
    {
        CreatePrefabs( false, false, true );
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/New within an Empty Parent", true)]
    static bool ValidateCreateEmptyParentPrefab()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/New within an Empty Parent")]
    static void CreateEmptyParentPrefab()
    {
        CreatePrefabs( false, true, true );
    }
}
