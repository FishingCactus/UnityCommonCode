using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if CINEMACHINE_AVAILABLE
    using Cinemachine;
#endif

namespace FishingCactus
{
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
                Transform[] transform_table
                )
            {
                if( transform_table == null
                    || transform_table.Length == 0
                    )
                {
                    return;
                }

                Debug.AssertFormat( RemainingTime == 0.0f, "Cannot start physics simulation if already running" );

                ParentWindow = parent;
                AutoSimulationWasEnabled = Physics.autoSimulation;
                Timer = 0.0f;
                Physics.autoSimulation = false;
                RemainingTime = duration;
                EditorApplication.update += EditorUpdate;

                var rigid_body_table = new List<Rigidbody>( transform_table.Length );

                foreach( var transform in transform_table )
                {
                    var rigid_body = transform.GetComponent<Rigidbody>();

                    if( rigid_body )
                    {
                        rigid_body.velocity = Vector3.zero;
                        rigid_body.angularVelocity = Vector3.zero;

                        rigid_body_table.Add( rigid_body );
                    }
                }

                FreezeDynamicRigidBodies( rigid_body_table );
            }

            // -- PRIVATE

            private bool AutoSimulationWasEnabled;
            private EditorWindow ParentWindow;
            private float RemainingTime = 0.0f;
            private float Timer = 0.0f;
            private List<Rigidbody> RigidBodyToResetTable;

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

                    UnfreezeDynamicRigidBodies();
                }
            }

            private void FreezeDynamicRigidBodies( List<Rigidbody> rigid_body_table )
            {
                RigidBodyToResetTable = FindObjectsOfType<Rigidbody>()
                    .Except( rigid_body_table )
                    .Where( rb => !rb.isKinematic )
                    .ToList();

                foreach( var rigid_body in RigidBodyToResetTable )
                {
                    rigid_body.isKinematic = true;
                }
            }

            private void UnfreezeDynamicRigidBodies()
            {
                foreach( var rigid_body in RigidBodyToResetTable )
                {
                    rigid_body.isKinematic = false;
                }
            }
        }

        // -- PRIVATE

        private Object ReplacerObject;
        private bool ItMustKeepObjectProperties = true;
        private bool ItMustKeepLocalTransforms = true;
        private bool ItMustKeepLocalRotation = false;
        private bool ItMustMergeComponents = false;
        private bool IncludeChildren = true;
        private bool AllMeshColliders = false;
        private int MinimalGridSize = 6;
        private int ObjectToSelectCount = 6;
        private float SpacingMultiplier = 1.5f;
        private float PhysicsSimulationTime = 5.0f;
        private float UniformScaleFactor;
        private Vector2 ScrollPosition;
        private Vector3 Offset;
        private Vector3 ScaleFactor;
        private PhysicsSimulator Simulator = new PhysicsSimulator();

        private void SetupViewCamera(
            Transform camera_transform,
            float field_of_view
            )
        {
            var scene_view = UnityEditor.SceneView.lastActiveSceneView;

            if( scene_view != null )
            {
                var target = scene_view.camera;

                target.transform.position = camera_transform.position;
                target.transform.rotation = camera_transform.rotation;

                target.fieldOfView = field_of_view;
                scene_view.orthographic = false;
                scene_view.AlignViewToObject( target.transform );
            }
        }

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

            ItMustKeepObjectProperties = EditorGUILayout.Toggle( "Keep object properties", ItMustKeepObjectProperties );
            ItMustKeepLocalTransforms = EditorGUILayout.Toggle( "Keep rotation & scale", ItMustKeepLocalTransforms );
            ItMustKeepLocalRotation = EditorGUILayout.Toggle("Keep rotation", ItMustKeepLocalRotation);
            ItMustMergeComponents = EditorGUILayout.Toggle( "Merge components", ItMustMergeComponents );
            ReplacerObject = EditorGUILayout.ObjectField( "Replace selected by : ", ReplacerObject, typeof( GameObject ), false );

            if( GUILayout.Button( "Replace selection" ) && (ReplacerObject != null) )
            {
                GameObject[] selected_object_array = Selection.gameObjects;
                GameObject[] new_object_array = new GameObject[selected_object_array.Length];

                for( int object_index = 0; object_index < selected_object_array.Length; object_index += 1 )
                {
                    GameObject instanciated_object = PrefabUtility.InstantiatePrefab( ReplacerObject ) as GameObject;

                    var old_object = selected_object_array[object_index];
                    if( old_object.transform.parent )
                    {
                        instanciated_object.transform.parent = old_object.transform.parent;
                    }

                    instanciated_object.transform.localPosition = old_object.transform.localPosition;

                    if( ItMustKeepObjectProperties )
                    {
                        instanciated_object.SetActive( old_object.activeSelf );
                        instanciated_object.isStatic = old_object.isStatic;
                        instanciated_object.tag = old_object.tag;
                        instanciated_object.layer = old_object.layer;
                    }

                    if( ItMustKeepLocalTransforms )
                    {
                        instanciated_object.transform.localRotation = old_object.transform.localRotation;
                        instanciated_object.transform.localScale = old_object.transform.localScale;
                    }

                    if ( ItMustKeepLocalRotation )
                    {
                        instanciated_object.transform.localRotation = old_object.transform.localRotation;
                    }

                    if ( ItMustMergeComponents )
                    {
                        foreach( var component in old_object.GetComponents<Component>() )
                        {
                            if( !instanciated_object.GetComponent( component.GetType() ) )
                            {
                                var new_component = instanciated_object.AddComponent( component.GetType() );
                                EditorUtility.CopySerialized( component, new_component );
                            }
                        }
                    }

                    new_object_array[object_index] = instanciated_object;

                    Undo.DestroyObjectImmediate( old_object );
                    Undo.RegisterCreatedObjectUndo( instanciated_object, "Replaced object" );
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

                    object_to_rotate.transform.Rotate( new Vector3( 0.0f, random_rotation, 0.0f ) );
                }
            }

            if (GUILayout.Button( "Random z rotation on selected !") )
            {
                GameObject[] selected_object_array = Selection.gameObjects;

                foreach ( GameObject object_to_rotate in selected_object_array )
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

            #if CINEMACHINE_AVAILABLE
                GUILayout.Label( "Camera --------------", EditorStyles.boldLabel );

                CinemachineVirtualCamera cinemachine_camera = null;
                Camera classic_camera = null;

                if( Selection.gameObjects.Length == 1 )
                {
                    cinemachine_camera = Selection.gameObjects[0].GetComponent<CinemachineVirtualCamera>();
                    classic_camera = Selection.gameObjects[0].GetComponent<Camera>();
                }

                GUI.enabled = cinemachine_camera != null || classic_camera != null;

                if( GUILayout.Button( "Setup view camera" ) )
                {
                    if( cinemachine_camera != null )
                    {
                        SetupViewCamera( cinemachine_camera.transform, cinemachine_camera.m_Lens.FieldOfView );
                    }
                    else if( classic_camera != null )
                    {
                        SetupViewCamera( classic_camera.transform, classic_camera.fieldOfView );
                    }
                }
            #endif

            GUI.enabled = true;

            GUILayout.Label( "Remove Meshcollider Tool -------------- " , EditorStyles.boldLabel );

            IncludeChildren = EditorGUILayout.Toggle( "Include children", IncludeChildren );
            AllMeshColliders = EditorGUILayout.Toggle( "Remove all mesh colliders" , AllMeshColliders );

            if( GUILayout.Button( "Remove meshcolliders !" ) )
            {
                GameObject[] selected_object_array = Selection.gameObjects;

                if( IncludeChildren )
                {
                    foreach( GameObject mesh_collider_object in selected_object_array )
                    {
                        MeshCollider[] mesh_collider_array = mesh_collider_object.GetComponentsInChildren<MeshCollider>();

                        foreach( MeshCollider mesh_collider in mesh_collider_array )
                        {
                            if( mesh_collider.sharedMesh == null || AllMeshColliders )
                            {
                                Undo.DestroyObjectImmediate( mesh_collider );
                            }
                        }
                    }

                    return;
                }

                foreach( GameObject mesh_collider_object in selected_object_array )
                {
                    MeshCollider mesh_collider = mesh_collider_object.GetComponent<MeshCollider>();

                    if ( mesh_collider == null)
                    {
                        continue;
                    }

                    if( mesh_collider.sharedMesh == null || AllMeshColliders )
                    {
                        Undo.DestroyObjectImmediate( mesh_collider );
                    }
                }
            }

            GUILayout.Label( "Random Deselection tool --------------", EditorStyles.boldLabel );
            ObjectToSelectCount = Mathf.Max(1, EditorGUILayout.IntField( "Count to deselect", ObjectToSelectCount ) );

            if( GUILayout.Button( "Random deselect on selected !" ) )
            {
                List<GameObject> selected_object_list = new List<GameObject>( Selection.gameObjects );
                int count_to_delete = Random.Range( 1, ObjectToSelectCount );

                while( count_to_delete > 0 )
                {
                    selected_object_list.RemoveAt( 0 );
                    count_to_delete--;
                }

                Selection.objects = selected_object_list.ToArray();
            }

            EditorGUILayout.EndScrollView();
        }
    }
}