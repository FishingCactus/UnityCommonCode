using UnityEngine;
using UnityEditor;

public class CreateNewPrefab : EditorWindow
{
    [MenuItem("FishingCactus/Tools/Create New Prefab")]
    static void CreatePrefab()
    {
        GameObject[] object_table = Selection.gameObjects;

        foreach( GameObject go in object_table )
        {
            string local_path = "Assets/Content/Prefabs/" + "P_" + go.name + ".prefab";

            if( AssetDatabase.LoadAssetAtPath( local_path, typeof( GameObject ) ) )
            {
                if( EditorUtility.DisplayDialog("Are you sure?", "The prefab already exists. Do you want to overwrite it?", "Yes", "No" ) )
                {
                    CreateNew( go, local_path );
                }
            }
            else
            {
                Debug.Log( "P_" + go.name + " Prefab Created" );
                CreateNew(go, local_path  );
            }
        }
    }

    [MenuItem( "FishingCactus/Tools/Create New Prefab", true)]
    static bool ValidateCreatePrefab()
    {
        return Selection.activeGameObject != null;
    }

    static void CreateNew(
        GameObject new_object,
        string local_path
        )
    {
        Object prefab = PrefabUtility.CreateEmptyPrefab( local_path );
        PrefabUtility.ReplacePrefab( new_object, prefab, ReplacePrefabOptions.ConnectToPrefab );
    }
}
