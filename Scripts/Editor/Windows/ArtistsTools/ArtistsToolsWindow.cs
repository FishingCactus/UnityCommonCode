#if USING_HOUDINI
using HoudiniEngineUnity;
using UnityEngine.SceneManagement;
using System.IO;
using System;
#endif
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Cinemachine;
using FishingCactus;

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
    private bool ItMustMergeComponents = false;
    private int MinimalGridSize = 6;
    private float SpacingMultiplier = 1.5f;
    private Vector3 Offset;
    private float PhysicsSimulationTime = 5.0f;
    private Vector3 ScaleFactor;
    private float UniformScaleFactor;
    private Vector2 ScrollPosition;
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

        HoudiniHdaSaver_Draw();

        GUILayout.Label( "Replace Tool --------------", EditorStyles.boldLabel );

        ItMustKeepObjectProperties = EditorGUILayout.Toggle( "Keep object properties", ItMustKeepObjectProperties );
        ItMustKeepLocalTransforms = EditorGUILayout.Toggle( "Keep rotation & scale", ItMustKeepLocalTransforms );
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

                if( ItMustMergeComponents )
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

        GUI.enabled = true;

        EditorGUILayout.EndScrollView();
    }

    #region HoudiniHDASaver
    private void HoudiniHdaSaver_Draw()
    {
#if USING_HOUDINI
        Action RefreshLists = () =>
        {
            HoudiniHDASaver_Data.Clear();
            HEU_HoudiniAssetRoot[] roots = FindObjectsOfType<HEU_HoudiniAssetRoot>();
            foreach( HEU_HoudiniAssetRoot root in roots )
            {
                GameObject rootGameObject = root.gameObject;
                Scene currentAssetScene = rootGameObject.scene;
                bool assetIsInSelection = Selection.Contains( rootGameObject );

                int index = 0;
                while( index < HoudiniHDASaver_Data.Count && HoudiniHDASaver_Data[index].scene != currentAssetScene ) index++;
                if( index == HoudiniHDASaver_Data.Count )
                {
                    HoudiniHDASaver_Data.Add( new HoudiniHdaSaver_DataContainer( currentAssetScene, root, assetIsInSelection ) );
                }
                else
                {
                    HoudiniHDASaver_Data[index].HoudiniRootAssets.Add( root );
                    HoudiniHDASaver_Data[index].HoudiniRootAssetsToogle.Add( assetIsInSelection );
                }
            }
            foreach( HoudiniHdaSaver_DataContainer dataContainer in HoudiniHDASaver_Data)
            {
                dataContainer.ToogleScene = dataContainer.HoudiniRootAssetsToogle.Any( x => x );
            }
        };
        Action Rename = () =>
        {
            string consoleLog = string.Empty;
            int affectedAssetsCount = 0;
            foreach( HoudiniHdaSaver_DataContainer data in HoudiniHDASaver_Data )
            {
                if( data.ToogleScene )
                {
                    consoleLog += data.RenameGameObjects();
                    affectedAssetsCount+=data.HoudiniRootAssets.Count;
                }
            }
            Debug.Log( $"Houdini HDA Save Helper : Rename All ({affectedAssetsCount} assets renamed) {Environment.NewLine}{consoleLog}" );
        };
        Action SaveAll = () =>
        {
            string consoleLog = string.Empty;
            int savedAssetsCount = 0;
            foreach( HoudiniHdaSaver_DataContainer data in HoudiniHDASaver_Data )
            {
                if( data.ToogleScene )
                {
                    consoleLog += data.SaveHoudiniAssets();
                    savedAssetsCount+=data.HoudiniRootAssets.Count;
                }
            }
            Debug.Log( $"Houdini HDA Save Helper : Save All ({savedAssetsCount} assets saved) {Environment.NewLine}{consoleLog}" );
        };
#endif
        GUILayout.Space( 10f );
        EditorGUILayout.LabelField( "", GUI.skin.horizontalSlider );

        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                HoudiniHdaSaver_WindowToogle = GUILayout.Toggle( HoudiniHdaSaver_WindowToogle, "= Houdini Save HDA Helper =", EditorStyles.boldLabel, GUILayout.ExpandWidth( true ) );
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
#if USING_HOUDINI
            bool use_houdini_define = true;
#else
            bool use_houdini_define = false;
#endif
            GUILayout.Label( $"USING_HOUDINI is { (use_houdini_define ? "" : "not ") }defined in player settings." );
            GUI.enabled = !EditorUtils.IsDefined( "USING_HOUDINI", BuildTargetGroup.Standalone );
            if( GUILayout.Button( "Add" ) )
            {
                EditorUtils.AddDefineIfNecessary( "USING_HOUDINI", BuildTargetGroup.Standalone );
            }
            GUI.enabled = !GUI.enabled;
            if( GUILayout.Button( "Remove" ) )
            {
                EditorUtils.RemoveDefineIfNecessary( "USING_HOUDINI", BuildTargetGroup.Standalone );
            }
            GUI.enabled = true;
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

#if USING_HOUDINI
            if( HoudiniHdaSaver_WindowToogle )
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if( GUILayout.Button( "Refresh from opened scenes" ) || !HoudiniHdaSaver_IsInit )
                    {
                        RefreshLists.Invoke();
                        HoudiniHdaSaver_IsInit = true;
                    }
                    if( GUILayout.Button( "Rename All" ) )
                    {
                        Rename.Invoke();
                    }
                    if( GUILayout.Button( "Save All" ) )
                    {
                        SaveAll.Invoke();
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label( "Save Folder Path :", new GUILayoutOption[] { GUILayout.ExpandWidth( false ), GUILayout.Width( 120 ) } );
                    HoudiniHdaSaver_FolderPath = GUILayout.TextField( HoudiniHdaSaver_FolderPath );
                    if( GUILayout.Button( "...", new GUILayoutOption[] { GUILayout.ExpandWidth( false ), GUILayout.Width( 30 ) } ) )
                    {
                        HoudiniHdaSaver_FolderPath = EditorUtility.OpenFolderPanel( "Select Houdini HDA save folder...", HoudiniHdaSaver_FolderPath, "" );
                    }
                }
                EditorGUILayout.EndHorizontal();

                foreach( HoudiniHdaSaver_DataContainer data in HoudiniHDASaver_Data )
                {
                    data.DrawGui();
                }
            }
#endif
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField( "", GUI.skin.horizontalSlider );
        GUILayout.Space( 10f );
    }

#if USING_HOUDINI
    private class HoudiniHdaSaver_DataContainer
    {
        public bool ToogleScene = false;
        public bool ToogleAllAsset = false;
        public Scene scene;
        public List<HEU_HoudiniAssetRoot> HoudiniRootAssets = new List<HEU_HoudiniAssetRoot>();
        public List<bool> HoudiniRootAssetsToogle = new List<bool>();
        public string FileSaveNameBase = "";

        public HoudiniHdaSaver_DataContainer( Scene asset_scene, HEU_HoudiniAssetRoot asset, bool asset_toggle )
        {
            scene = asset_scene;
            HoudiniRootAssets.Add( asset );
            HoudiniRootAssetsToogle.Add( asset_toggle );
            FileSaveNameBase = asset.name;
        }

        public string RenameGameObjects()
        {
            string consoleLog = "";
            for( int i = 0; i < HoudiniRootAssets.Count; i++ )
            {
                if( HoudiniRootAssetsToogle[i] )
                {
                    consoleLog += HoudiniRootAssets[i].gameObject.name + "=>";
                    HoudiniRootAssets[i].gameObject.name = FileSaveNameBase + (i + 1);
                    consoleLog += HoudiniRootAssets[i].gameObject.name + Environment.NewLine;
                }
            }
            return consoleLog;
        }

        public string SaveHoudiniAssets()
        {
            string consoleLog = "";
            if( !string.IsNullOrEmpty( HoudiniHdaSaver_FolderPath ) )
            {
                string folderPath = Path.Combine( HoudiniHdaSaver_FolderPath, scene.name );
                if( !Directory.Exists( folderPath ) )
                {
                    Directory.CreateDirectory( folderPath );
                }
                for( int i = 0; i < HoudiniRootAssets.Count; i++ )
                {
                    if( HoudiniRootAssetsToogle[i] )
                    {
                        string path = Path.Combine( folderPath, HoudiniRootAssets[i].gameObject.name + ".preset" );
                        consoleLog += path + Environment.NewLine;
                        HEU_AssetPresetUtility.SaveAssetPresetToFile( HoudiniRootAssets[i]._houdiniAsset, path );
                    }
                }
            }
            return consoleLog;
        }

        public void DrawGui()
        {
            EditorGUILayout.BeginVertical();
            {
                ToogleScene = EditorGUILayout.BeginToggleGroup( scene.name, ToogleScene );
                if( ToogleScene )
                {
                    using( new GUILayout.HorizontalScope() )
                    {
                        GUILayout.Space( 30f );
                        bool toggleJustChange = false;
                        using( new GUILayout.VerticalScope() )
                        {
                            EditorGUILayout.BeginHorizontal( GUILayout.Height( 20f ) );
                            {
                                EditorGUI.BeginChangeCheck();
                                ToogleAllAsset = EditorGUILayout.ToggleLeft( "All", ToogleAllAsset );
                                toggleJustChange = EditorGUI.EndChangeCheck();

                                GUILayout.Space( 50f );
                                if( GUILayout.Button( "Save", new GUILayoutOption[] { GUILayout.ExpandWidth( false ), GUILayout.Width( 50 ) } ) )
                                {
                                    string consoleLog = SaveHoudiniAssets();
                                    Debug.Log( $"Houdini HDA Save Helper : Scene [{scene.name}]: ({HoudiniRootAssets.Count} assets saved) {Environment.NewLine}{consoleLog}" );
                                }
                                if( GUILayout.Button( "Rename", new GUILayoutOption[] { GUILayout.ExpandWidth( false ), GUILayout.Width( 70 ) } ) )
                                {
                                    string consoleLog = RenameGameObjects();
                                    Debug.Log( $"Houdini HDA Save Helper : Scene [{scene.name}]: ({HoudiniRootAssets.Count} assets renamed) {Environment.NewLine}{consoleLog}" );
                                }
                                FileSaveNameBase = GUILayout.TextField( FileSaveNameBase, new GUILayoutOption[] { GUILayout.ExpandWidth( true ), GUILayout.MinWidth( 140 ) } );
                            }
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginVertical();
                            int AssetIndexToBake = -1;
                            for( int i = 0; i < HoudiniRootAssets.Count; i++ )
                            {
                                EditorGUILayout.BeginHorizontal( );
                                HoudiniRootAssetsToogle[i] = EditorGUILayout.ToggleLeft( HoudiniRootAssets[i].name, toggleJustChange ? ToogleAllAsset : HoudiniRootAssetsToogle[i] );
                                GUILayout.FlexibleSpace( );
                                if( GUILayout.Button( "Save & replace with Baked", new GUILayoutOption[] { GUILayout.ExpandWidth( false ), GUILayout.Width( 170 ) } ) )
                                {
                                    string folderPath = Path.Combine( HoudiniHdaSaver_FolderPath, scene.name );
                                    if( !Directory.Exists( folderPath ) )
                                    {
                                        Directory.CreateDirectory( folderPath );
                                    }
                                    string assetGameObjectName = HoudiniRootAssets[i].gameObject.name;
                                    string path = Path.Combine( folderPath, assetGameObjectName + ".preset" );

                                    Debug.Log( $"Houdini HDA Save Helper : Scene [{scene.name}] baking {assetGameObjectName}" );

                                    HEU_HoudiniAsset currentAsset = HoudiniRootAssets[i]._houdiniAsset;
                                    HEU_AssetPresetUtility.SaveAssetPresetToFile( currentAsset, path );

                                    currentAsset._bakedEvent.AddListener( (asset, success, outputList) => 
                                    {
                                        for( int outputIndex = 0; outputIndex < outputList.Count; outputIndex++ )
                                        {
                                            outputList[0].name = $"{assetGameObjectName}_baked";
                                        }
                                        Debug.Log( $"Houdini HDA Save Helper : Scene [{scene.name}] {outputList[0].name} baked{Environment.NewLine}Asset ={asset.name}{Environment.NewLine}Success ={success}{Environment.NewLine}Count ={outputList.Count}" );
                                    } );
                                    HoudiniRootAssets[i]._houdiniAsset.BakeToNewStandalone();

                                    AssetIndexToBake = i;
                                }
                                EditorGUILayout.EndHorizontal();
                            }

                            if(AssetIndexToBake != -1)
                            {
                                DestroyImmediate( HoudiniRootAssets[AssetIndexToBake].gameObject );
                                HoudiniRootAssets.RemoveAt( AssetIndexToBake );
                                HoudiniRootAssetsToogle.RemoveAt( AssetIndexToBake );
                            }
                            EditorGUILayout.EndVertical();
                        }
                    }
                }
                EditorGUILayout.EndToggleGroup();
            }
            EditorGUILayout.EndVertical();
        }
        private void SetAssetName(HEU_HoudiniAssetRoot root, string name)
        {
            root.gameObject.name = name;
            root._houdiniAsset.name = name;
            HEU_HoudiniAsset f = root._houdiniAsset;
        }
    }
    private static List<HoudiniHdaSaver_DataContainer> HoudiniHDASaver_Data = new List<HoudiniHdaSaver_DataContainer>();
    private static bool HoudiniHdaSaver_IsInit = false;
    private static string HoudiniHdaSaver_FolderPath = "";
#endif
    private static bool HoudiniHdaSaver_WindowToogle = false;
#endregion
}
