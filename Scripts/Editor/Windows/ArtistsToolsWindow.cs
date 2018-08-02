using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

internal class ArtistsToolsWindow : EditorWindow
{
    private class PhysicsSimulator
    {
        // -- PUBLIC

        public bool IsRunning
        {
            get
            {
                return RemainingTime > 0.0f;
            }
        }

        public void Start(
            EditorWindow parent,
            float duration,
            Transform[] transforms
            )
        {
            Debug.AssertFormat( RemainingTime == 0.0f, "Cannot start physics simulation if already running" );

            ParentWindow = parent;
            AutoSimulationWasEnabled = Physics.autoSimulation;
            Timer = 0.0f;
            Physics.autoSimulation = false;
            RemainingTime = duration;
            EditorApplication.update += EditorUpdate;

            foreach( var transform in transforms )
            {
                var rigid_body = transform.GetComponent<Rigidbody>();

                if( rigid_body )
                {
                    rigid_body.velocity = Vector3.zero;
                    rigid_body.angularVelocity = Vector3.zero;
                }
            }

            SaveTransforms( transforms );
        }

        // -- PRIVATE

        private bool AutoSimulationWasEnabled;
        private EditorWindow ParentWindow;
        private float RemainingTime = 0.0f;
        private float Timer = 0.0f;
        private List<Transform> TransformToResetTable;
        private List<Vector3> SavedLocalPositionTable;
        private List<Quaternion> SavedLocalRotationTable;

        private void EditorUpdate()
        {
            Timer += Time.deltaTime;

            while(
                Timer > Time.fixedDeltaTime
                && RemainingTime > 0.0f
                )
            {
                var step = Mathf.Min( RemainingTime, Time.fixedDeltaTime );

                Physics.Simulate( step );
                Timer -= step;
                RemainingTime -= step;
            }

            if( RemainingTime <= 0.0f )
            {
                // :TRICKY: Physics simulation seems to have issues with really small time steps
                //  Physics.Simulate( Timer );

                RemainingTime = 0.0f;

                Physics.autoSimulation = AutoSimulationWasEnabled;
                EditorApplication.update -= EditorUpdate;
                ParentWindow.Repaint();

                ResetTransforms();
            }
        }

        private void SaveTransforms(
            Transform[] transforms
            )
        {
            TransformToResetTable = FindObjectsOfType<Transform>().Except( transforms ).ToList();
            SavedLocalPositionTable = new List<Vector3>( TransformToResetTable.Count );
            SavedLocalRotationTable = new List<Quaternion>( TransformToResetTable.Count );

            foreach( var transform in TransformToResetTable )
            {
                SavedLocalPositionTable.Add( transform.localPosition );
                SavedLocalRotationTable.Add( transform.localRotation );
            }
        }

        private void ResetTransforms()
        {
            for( var index = 0; index < TransformToResetTable.Count; ++index )
            {
                var transform = TransformToResetTable[index];

                transform.localPosition = SavedLocalPositionTable[index];
                transform.localRotation = SavedLocalRotationTable[index];
            }
        }
    }

    // -- PRIVATE

    private Object ReplacerObject;
    private bool ItMustKeepLocalTransforms = true;
    private int MinimalGridSize = 6;
    private float SpacingMultiplier = 1.5f;
    private Vector3 Offset;
    private float PhysicsSimulationTime = 5.0f;
    private Vector3 ScaleFactor;
    private float UniformScaleFactor;
    private Vector2 ScrollPosition;
    private PhysicsSimulator Simulator = new PhysicsSimulator();

    // -- UNITY

    [MenuItem( "FishingCactus/ArtistsTools" )]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow( typeof( ArtistsToolsWindow ) );
    }

    void OnGUI()
    {
        ScrollPosition = EditorGUILayout.BeginScrollView( ScrollPosition );

        GUILayout.Label( "Replace Tool --------------", EditorStyles.boldLabel );

        ItMustKeepLocalTransforms = EditorGUILayout.Toggle( "Keep Rotation & Scale", ItMustKeepLocalTransforms );
        ReplacerObject = EditorGUILayout.ObjectField( "Replace selected by : ", ReplacerObject, typeof( GameObject ), true );

        if( GUILayout.Button( "Replace selection - WARNING : No undo !" ) && ( ReplacerObject != null ) )
        {
            GameObject[] selected_object_array = Selection.gameObjects;
            GameObject[] new_object_array = new GameObject[selected_object_array.Length];

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

                DestroyImmediate( ( selected_object_array[object_index] as Object ), true );
            }

            Selection.objects = new_object_array;
        }

        GUILayout.Label( "Align Tool -------------- ", EditorStyles.boldLabel );

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

                    if( object_count++ >= grid_size - 1 )
                    {
                        object_count = 0;

                        next_object_position.x = 0.0f;
                        next_object_position.z += SpacingMultiplier * z_spacing;
                    }
                }
            }
        }

        GUILayout.Label( "Random rotation tool --------------", EditorStyles.boldLabel );

        if( GUILayout.Button( "Random y rotation on selected !" ) )
        {
            GameObject[] selected_object_array = Selection.gameObjects;

            foreach( GameObject object_to_rotate in selected_object_array )
            {
                float random_rotation = Random.Range( 30.0f, 120.0f );

                object_to_rotate.transform.Rotate( new Vector3( 0.0f, 0.0f, random_rotation ) );
            }
        }

        GUILayout.Label( "Random Scale tool --------------", EditorStyles.boldLabel );

        if( GUILayout.Button( "Random uniform scale on selected !" ) )
        {
            GameObject[] selected_object_array = Selection.gameObjects;

            foreach( GameObject object_to_scale in selected_object_array )
            {
                float random_scale = Random.Range( 0.5f, 1.0f );

                object_to_scale.transform.localScale = new Vector3( random_scale, random_scale, random_scale );
            }
        }

        GUILayout.Label( "Physics simulate --------------", EditorStyles.boldLabel );

        PhysicsSimulationTime = EditorGUILayout.FloatField( "Simulation time:", PhysicsSimulationTime );

        GUI.enabled = !Simulator.IsRunning;

        if( GUILayout.Button( "Simulate !" ) )
        {
            Simulator.Start( this, PhysicsSimulationTime, Selection.transforms );
        }

        if( GUILayout.Button( "Bake (-> static colliders) !" ) )
        {
            foreach( var transform in Selection.transforms )
            {
                DestroyImmediate( transform.GetComponent<Rigidbody>() );
                transform.gameObject.isStatic = true;
            }
        }

        GUI.enabled = true;

        GUILayout.Label( "Position Offset tool --------------", EditorStyles.boldLabel );

        Offset = EditorGUILayout.Vector3Field( "Offset:", Offset );

        if( GUILayout.Button( "Apply an offset on position !" ) )
        {
            GameObject[] selected_object_array = Selection.gameObjects;

            foreach( GameObject object_to_offset in selected_object_array )
            {
                object_to_offset.transform.position = object_to_offset.transform.position + Offset;
            }
        }

        GUILayout.Label( "Scale multiplier tool --------------", EditorStyles.boldLabel );

        ScaleFactor = EditorGUILayout.Vector3Field( "Multiplier:", ScaleFactor );

        if( GUILayout.Button( "Apply a scale multiplier !" ) )
        {
            GameObject[] selected_object_array = Selection.gameObjects;

            foreach( GameObject object_to_scale in selected_object_array )
            {
                Vector3 new_local_scale = object_to_scale.transform.localScale;
                new_local_scale.x *= ScaleFactor.x;
                new_local_scale.y *= ScaleFactor.y;
                new_local_scale.z *= ScaleFactor.z;

                object_to_scale.transform.localScale = new_local_scale;
            }
        }

        GUILayout.Label( "Uniform Scale multiplier tool --------------", EditorStyles.boldLabel );

        UniformScaleFactor = EditorGUILayout.FloatField( "Multiplier:", UniformScaleFactor );

        if( GUILayout.Button( "Apply an Uniform scale multiplier !" ) )
        {
            GameObject[] selected_object_array = Selection.gameObjects;

            foreach( GameObject object_to_scale in selected_object_array )
            {
                object_to_scale.transform.localScale = object_to_scale.transform.localScale * UniformScaleFactor;

                Light light_component = object_to_scale.GetComponent<Light>();

                if( light_component != null )
                {
                    light_component.range *= UniformScaleFactor;
                    light_component.intensity *= UniformScaleFactor;
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }
}
