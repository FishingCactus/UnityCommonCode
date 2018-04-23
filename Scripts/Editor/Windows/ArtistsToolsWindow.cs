using UnityEditor;
using UnityEngine;

internal class ArtistsToolsWindow : EditorWindow
{
    // -- PRIVATE

    private Object ReplacerObject;
    private bool ItMustKeepLocalTransforms = true;
    private int MinimalGridSize = 6;
    private float SpacingMultiplier = 1.5f;

    // -- UNITY

    [MenuItem( "FishingCactus/ArtistsTools" )]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof( ArtistsToolsWindow ) );
    }

    void OnGUI()
    {
        GUILayout.Label( "Replace Tool --------------", EditorStyles.boldLabel );

        ItMustKeepLocalTransforms = EditorGUILayout.Toggle( "Keep Rotation & Scale", ItMustKeepLocalTransforms );
        ReplacerObject = EditorGUILayout.ObjectField( "Replace selected by : ", ReplacerObject, typeof(GameObject), true );

        if( GUILayout.Button( "Replace selection - WARNING : No undo !" ) && ( ReplacerObject != null ) )
        {
            GameObject[] selected_object_array = Selection.gameObjects;
            GameObject[] new_object_array = new GameObject[ selected_object_array.Length ];

            for( int object_index = 0; object_index < selected_object_array.Length; object_index += 1 )
            {
                GameObject instanciated_object = PrefabUtility.InstantiatePrefab( ReplacerObject ) as GameObject;

                if( selected_object_array[object_index].transform.parent )
                {
                    instanciated_object.transform.parent = selected_object_array[object_index].transform.parent;
                }

                instanciated_object.transform.localPosition = selected_object_array[object_index].transform.localPosition;

                if( ItMustKeepLocalTransforms )
                {
                    instanciated_object.transform.localRotation = selected_object_array[object_index].transform.localRotation;
                    instanciated_object.transform.localScale = selected_object_array[object_index].transform.localScale;
                }

                new_object_array[object_index] = instanciated_object;

                DestroyImmediate( (selected_object_array[object_index] as Object), true );
            }

            Selection.objects = new_object_array;
        }

        GUILayout.Label("Align Tool -------------- ", EditorStyles.boldLabel);

        MinimalGridSize = Mathf.Max( 1, EditorGUILayout.IntField( "Minimal Grid Size", MinimalGridSize ) );
        SpacingMultiplier = Mathf.Max( 1.1f, EditorGUILayout.FloatField( "Spacing Multiplier", SpacingMultiplier ) );

        if( GUILayout.Button( "Align selected objects" ) )
        {
            GameObject[] selected_object_array = Selection.gameObjects;

            if( selected_object_array.Length > 0 )
            {
                Vector3 next_object_position = Vector3.zero;
                float x_spacing = 0.0f;
                float z_spacing = 0.0f;

                foreach( GameObject object_to_move in selected_object_array )
                {
                    Bounds full_bounds = new Bounds( object_to_move.transform.position, Vector3.one );
                    Renderer[] renderer_array = object_to_move.GetComponentsInChildren<Renderer>();

                    foreach( Renderer renderer in renderer_array )
                    {
                        full_bounds.Encapsulate( renderer.bounds );
                    }

                    x_spacing = Mathf.Max( x_spacing, full_bounds.size.x );
                    z_spacing = Mathf.Max( z_spacing, full_bounds.size.z );
                }

                int object_count = 0;
                int grid_size = Mathf.Max( MinimalGridSize, (int)Mathf.Sqrt( selected_object_array.Length ) );

                foreach( GameObject object_to_move in selected_object_array )
                {
                    object_to_move.transform.position = next_object_position;
                    next_object_position.x += SpacingMultiplier * x_spacing;

                    if( object_count++ >= grid_size-1 )
                    {
                        object_count = 0;

                        next_object_position.x = 0.0f;
                        next_object_position.z += SpacingMultiplier * z_spacing;
                    }
                }
            }
        }
    }
}
