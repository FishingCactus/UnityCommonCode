using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class PrefabReplacementEditor : EditorWindow
{
    // -- PRIVATE

    private GameObject ActiveGameObject;
    private GameObject[] ActiveGameObjects = new GameObject[ 0 ];

    private string SearchPrefabName;
    private string SearchPrefabLabel;

    private string[] FilteredPrefabs = new string[ 0 ];

    Vector2 ScrollPosition;

    private PrefabReplacementEditor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += EvaluateIcons;
    }

    private void EvaluateIcons( int instance_id, Rect selection_rect )
    {
        GameObject go = EditorUtility.InstanceIDToObject( instance_id ) as GameObject;
        if ( go == null ) return;

        if ( ActiveGameObjects != null && ActiveGameObjects.Length > 1 )
        {
            for ( int i = 0; i < ActiveGameObjects.Length; i++ )
            {
                if ( ActiveGameObjects[ i ] == go ) DrawIcon( "arrow", selection_rect );
            }
        }
        else if ( ActiveGameObject != null )
        {

            if ( ActiveGameObject == go ) DrawIcon( "arrow", selection_rect );
        }
    }

    [MenuItem( "FishingCactus/Prefab Replacement Tool", false, 1 )]
    public static void OpenPrefabReplacement()
    {
        PrefabReplacementEditor window =
            (PrefabReplacementEditor) GetWindow( typeof( PrefabReplacementEditor ), false, "Prefab Replacement Tool", true );
        window.minSize = new Vector2( 150, 100 );
        window.ShowUtility();
    }

    private static void DrawIcon( string texName, Rect rect )
    {
        Rect r = new Rect( rect.x + rect.width - 16f, rect.y, 16f, 16f );
        GUI.DrawTexture( r, GetTex( texName ) );
    }

    private static Texture2D GetTex( string name )
    {
        return (Texture2D) Resources.Load( "EditorIcons/" + name );
    }

    private bool AddPrefab( string prefab_path, bool replace = false )
    {
        GameObject new_game_object = AssetDatabase.LoadAssetAtPath<GameObject>( prefab_path );

        if ( ActiveGameObjects != null && ActiveGameObjects.Length > 0 )
        {
            foreach ( var go in ActiveGameObjects )
            {
                GameObject instanciated_game_object = PrefabUtility.InstantiatePrefab( new_game_object ) as GameObject;
                instanciated_game_object.transform.SetParent( go.transform.parent );
                instanciated_game_object.transform.position = go.transform.position;
                instanciated_game_object.transform.localScale = go.transform.localScale;
                instanciated_game_object.transform.rotation = go.transform.rotation;
                if ( replace )
                {
                    DestroyImmediate( go );
                }
            }
        }
        else if ( ActiveGameObject != null )
        {
            GameObject instanciated_game_object = PrefabUtility.InstantiatePrefab( new_game_object ) as GameObject;
            instanciated_game_object.transform.SetParent( ActiveGameObject.transform.parent );
            instanciated_game_object.transform.position = ActiveGameObject.transform.position;
            instanciated_game_object.transform.localScale = ActiveGameObject.transform.localScale;
            instanciated_game_object.transform.rotation = ActiveGameObject.transform.rotation;
            if ( replace )
            {
                DestroyImmediate( ActiveGameObject );
            }
        }

        return false;
    }

    private void SearchPrefab( string search_input, string search_label )
    {
        string[] filtered_prefabs_guids;

        if ( string.IsNullOrEmpty( search_label ) )
        {
            filtered_prefabs_guids = AssetDatabase.FindAssets( search_input + " t:Prefab" );
        }
        else
        {
            filtered_prefabs_guids = AssetDatabase.FindAssets( search_input + " t:Prefab l:" + search_label );
        }

        FilteredPrefabs = new string[ filtered_prefabs_guids.Length ];

        for ( int i = 0; i < FilteredPrefabs.Length; i++ )
        {
            FilteredPrefabs[ i ] = AssetDatabase.GUIDToAssetPath( filtered_prefabs_guids[ i ] );
        }
    }

    private string CleanupName( string name )
    {
        return Regex.Replace( name, @"(.*) (\(.?.?.?.?.?.?\))*", "$1" );
    }

    private interface IHierarchyIcon
    {
        string EditorIconPath { get; }
    }

    // -- UNITY

    private void OnGUI()
    {
        EditorGUILayout.LabelField( "Replace selected prefabs", EditorStyles.boldLabel );

        if ( Selection.activeObject != null )
        {
            if ( Selection.gameObjects.Length > 1 )
            {
                ActiveGameObjects = Selection.gameObjects;
            }
            else
            {
                ActiveGameObjects = default( GameObject[] );
            }

            ActiveGameObject = Selection.activeGameObject;
        }


        SearchPrefabName = EditorGUILayout.TextField( "Search prefabs containing", SearchPrefabName );
        SearchPrefabLabel = EditorGUILayout.TextField( "Search only from label", SearchPrefabLabel );

        if ( GUILayout.Button( "Search Prefabs" ) )
        {
            SearchPrefab( SearchPrefabName, SearchPrefabLabel );
        }

        {
            string result_message;

            if ( FilteredPrefabs.Length > 0 )
            {
                result_message = "Found " + FilteredPrefabs.Length + " prefabs matching";
            }
            else
            {
                result_message = "No prefab found";
            }

            GUILayout.Box( result_message, GUILayout.ExpandWidth( true ), GUILayout.Height( 25 ) );
        }

        if ( FilteredPrefabs.Length > 0 )
        {
            ScrollPosition = EditorGUILayout.BeginScrollView( ScrollPosition );

            for ( int i = 0; i < FilteredPrefabs.Length; i++ )
            {
                GUILayout.Space( 5 );

                EditorGUILayout.LabelField( "..." + FilteredPrefabs[ i ].Substring( Mathf.Max( FilteredPrefabs[ i ].Length - 40, 0 ) ) );

                EditorGUILayout.BeginHorizontal();

                if ( GUILayout.Button( "Add" ) )
                {
                    AddPrefab( FilteredPrefabs[ i ], false );
                }

                if ( GUILayout.Button( "Replace" ) )
                {
                    AddPrefab( FilteredPrefabs[ i ], true );
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

    }
}