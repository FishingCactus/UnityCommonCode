using UnityEngine;
using UnityEditor;

public class ObjectToTerrain : EditorWindow
{
    // -- PRIVATE

    private static string[] choice_name_table = new string[] { "Bottom Up", "Top Down" };

    private int resolution = 512;
    private int radio_selected_index = 0;
    private float height_shift = 0.0f;
    private Vector3 terrain_offset;

    delegate void CleanUp();

    private void CreateTerrain()
    {
        ShowProgressBar( 1, 100 );

        TerrainData terrain = new TerrainData();
        terrain.heightmapResolution = resolution;

        GameObject terrainObject = Terrain.CreateTerrainGameObject( terrain );

        Undo.RecordObject( terrainObject, "Object to Terrain" );

        MeshCollider collider = Selection.activeGameObject.GetComponent<MeshCollider>();
        CleanUp cleanUp = null;

        if( !collider )
        {
            collider = Selection.activeGameObject.AddComponent<MeshCollider>();
            cleanUp = () => DestroyImmediate( collider );
        }

        Bounds bounds = collider.bounds;
        float sizeFactor = collider.bounds.size.y / ( collider.bounds.size.y + terrain_offset.y );

        terrain.size = collider.bounds.size + terrain_offset;
        bounds.size = new Vector3( terrain.size.x, collider.bounds.size.y, terrain.size.z );

        float[,] heights_array = new float[terrain.heightmapResolution, terrain.heightmapResolution];
        Ray ray = new Ray( new Vector3( bounds.min.x, bounds.max.y + bounds.size.y, bounds.min.z ), -Vector3.up );
        RaycastHit hit = new RaycastHit();
        float mesh_height_inverse = 1 / bounds.size.y;
        Vector3 ray_origin = ray.origin;
        int max_height = heights_array.GetLength( 0 );
        int max_length = heights_array.GetLength( 1 );
        Vector2 x_z_step = new Vector2( bounds.size.x / max_length, bounds.size.z / max_height );

        for( int z_index = 0; z_index < max_height; z_index++ )
        {
            ShowProgressBar( z_index, max_height );

            for( int x_index = 0; x_index < max_length; x_index++ )
            {
                float height = 0.0f;

                if( collider.Raycast( ray, out hit, bounds.size.y * 3.0f ) )
                {
                    height = ( hit.point.y - bounds.min.y ) * mesh_height_inverse;
                    height += height_shift;

                    if( radio_selected_index == 0 )
                    {
                        height *= sizeFactor;
                    }

                    height = Mathf.Max( 0.0f, height );
                }

                heights_array[z_index, x_index] = height;
                ray_origin.x += x_z_step[0];
                ray.origin = ray_origin;
            }

            ray_origin.z += x_z_step[1];
            ray_origin.x = bounds.min.x;
            ray.origin = ray_origin;
        }

        terrain.SetHeights( 0, 0, heights_array );

        EditorUtility.ClearProgressBar();

        cleanUp?.Invoke();
    }

    void ShowProgressBar(
        float progress,
        float maximum_progress
        )
    {
        float current_progress = progress / maximum_progress;

        EditorUtility.DisplayProgressBar(
            "Creating Terrain...",
            string.Format( "{0}%", Mathf.RoundToInt( current_progress * 100.0f ) ),
            current_progress
            );
    }

    // -- UNITY

    [MenuItem( "FishingCactus/Tools/Object to Terrain", false, 2000 )]
    static void OpenWindow()
    {
        GetWindow<ObjectToTerrain>( true );
    }

    void OnGUI()
    {
        resolution = EditorGUILayout.IntField( "Resolution", resolution );
        terrain_offset = EditorGUILayout.Vector3Field( "Add terrain", terrain_offset );
        height_shift = EditorGUILayout.Slider( "Shift height", height_shift, -1.0f, 1.0f );
        radio_selected_index = GUILayout.SelectionGrid( radio_selected_index, choice_name_table, choice_name_table.Length, EditorStyles.radioButton );

        if( GUILayout.Button( "Create Terrain" ) )
        {
            if( Selection.activeGameObject == null )
            {
                EditorUtility.DisplayDialog( "No object selected", "Please select an object.", "Ok" );

                return;
            }
            else
            {
                CreateTerrain();
            }
        }
    }
}
