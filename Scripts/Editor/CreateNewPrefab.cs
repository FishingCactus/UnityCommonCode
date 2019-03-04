using UnityEngine;
using UnityEditor;

public class CreateNewPrefab : EditorWindow
{
    // -- PRIVATE

    //static string PrefabCreationPath = "Assets/__Content/Prefabs/";
    static string PrefabCreationPath = "Assets/__Epistory2/Prefabs/Assets/";
    static string PrefabPrefix = "P_";
    static string PrefabExtension = ".prefab";


    static void CreatePrefabs( bool can_be_variant, bool put_in_empty_parent )
    {
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

            string local_path = PrefabCreationPath + new_prefab.name + PrefabExtension;

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

    static void CreateNew( GameObject new_object, string local_path )
    {
        Object prefab = PrefabUtility.CreateEmptyPrefab( local_path );
        PrefabUtility.ReplacePrefab( new_object, prefab, ReplacePrefabOptions.ConnectToPrefab );
    }

    static void CreateVariant( GameObject new_object, string local_path)
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
        CreatePrefabs(true, false);
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/Force new (no variant)", true)]
    static bool ValidateCreatePrefab()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/Force new (no variant)")]
    static void CreatePrefab()
    {
        CreatePrefabs( false, false );
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/New within an Empty Parent", true)]
    static bool ValidateCreateEmptyParentPrefab()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("FishingCactus/Tools/Create New Prefab/New within an Empty Parent")]
    static void CreateEmptyParentPrefab()
    {
        CreatePrefabs( false, true );
    }

}
