using UnityEngine;
using UnityEditor;

public class CreateNewPrefab : EditorWindow
{
    // -- TYPES

    private enum VariantMode
    {
        Enabled = 0,
        Disabled
    }

    private enum ParentMode
    {
        Free = 0,
        Attached
    }

    private enum PathSelection
    {
        Default = 0,
        DisplayDialog
    }

    // -- FIELDS

    static string PrefabPathLocalKey = "FC/CommonCode/NewPrefabPath";

    static string PrefabCreationPath = "Assets/_Content/Prefabs/Assets/";
    static string PrefabPrefix = "P_";
    static string PrefabExtension = ".prefab";

    // -- METHODS

    static private void CreatePrefabs(
        VariantMode variant_mode,
        ParentMode parent_mode,
        PathSelection path_mode
        )
    {
        string previous_path = EditorPrefs.GetString( PrefabPathLocalKey, PrefabExtension );
        string destination_path = string.Empty;

        if( path_mode == PathSelection.DisplayDialog )
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

            if( parent_mode == ParentMode.Attached )
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

            if( variant_mode == VariantMode.Enabled )
            {
                Debug.Log( new_prefab.name + " prefab variant created" );
                CreateVariant( new_prefab, local_path );
            }
            else
            {
                Debug.Log( new_prefab.name + " prefab created" );
                CreateNew( new_prefab, local_path );
            }

            if( parent_mode == ParentMode.Attached )
            {
                new_prefab.transform.position = game_object.transform.position;
                 
                DestroyImmediate( game_object );
            }
        }
    }

    static private void CreateNew( 
        GameObject new_object, 
        string local_path 
        )
    {
        PrefabUtility.SaveAsPrefabAsset(new_object, local_path);
    }

    static private void CreateVariant(
        GameObject new_object,
        string local_path
        )
    {
        PrefabUtility.SaveAsPrefabAssetAndConnect( new_object, local_path, InteractionMode.UserAction );
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/New or Variant", true)]
    static private bool ValidateCreatePrefabOrVariant()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/New or Variant")]
    static private void CreatePrefabOrVariant()
    {
        CreatePrefabs( VariantMode.Enabled, ParentMode.Free, PathSelection.DisplayDialog );
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/Force new (no variant)", true)]
    static private bool ValidateCreatePrefab()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/Force new (no variant)")]
    static private void CreatePrefab()
    {
        CreatePrefabs( VariantMode.Disabled, ParentMode.Free, PathSelection.DisplayDialog );
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/New within an Empty Parent", true)]
    static private bool ValidateCreateEmptyParentPrefab()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/New within an Empty Parent")]
    static private void CreateEmptyParentPrefab()
    {
        CreatePrefabs( VariantMode.Disabled, ParentMode.Attached, PathSelection.DisplayDialog );
    }
}
