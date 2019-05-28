#if USING_HOUDINI
using HoudiniEngineUnity;
#endif
using UnityEngine.SceneManagement;
using System.IO;
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace FishingCactus
{
    public class HoudiniHdaSaver : EditorWindow
    {
        public static void Draw()
        {
            EditorGUILayout.BeginVertical();
            {
                DrawHeader();
#if USING_HOUDINI
                if( IsWindowToogle )
                {
                    DrawToolActions();
                    DrawDataListContent();
                }
#endif
            }
            EditorGUILayout.EndVertical();
        }

#if USING_HOUDINI
        private static void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                IsWindowToogle = GUILayout.Toggle( IsWindowToogle, $"== Houdini Save HDA Helper ({HoudiniHdaSaverHelperVersion}) ==", EditorStyles.boldLabel, GUILayout.ExpandWidth( true ) );
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
        }

        private static void DrawToolActions()
        {
            EditorGUILayout.HelpBox( $"The save functionnality is disabled if the save folder path is empty !{Environment.NewLine}The \"Refresh List\r button will tick automatically selected Houdini Root Object in the project hierarchy.", MessageType.Info );

            EditorGUILayout.BeginHorizontal();
            {
                if( GUILayout.Button( "Refresh from opened scenes" ) || !IsWindowInit )
                {
                    RefreshLists();
                    IsWindowInit = true;
                }
                if( GUILayout.Button( "Rename All" ) )
                {
                    RenameAll();
                }
                if( GUILayout.Button( "Save All" ) )
                {
                    SaveAll();
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = !IsFolderPathValidFlag ? Color.red : Color.black;

                GUILayout.Label( "Save Folder Path :", style, new GUILayoutOption[] { GUILayout.ExpandWidth( false ), GUILayout.Width( 120 ) } );
                GUI.contentColor = Color.black;
                StaticFolderPath = GUILayout.TextField( StaticFolderPath );

                if( GUILayout.Button( "...", new GUILayoutOption[] { GUILayout.ExpandWidth( false ), GUILayout.Width( 30 ) } ) )
                {
                    StaticFolderPath = EditorUtility.OpenFolderPanel( "Select Houdini HDA save folder...", StaticFolderPath, "" );
                }

                GUI.enabled = IsFolderPathValidFlag;
                if( GUILayout.Button( "Open", new GUILayoutOption[] { GUILayout.ExpandWidth( false ), GUILayout.Width( 50 ) } ) )
                {
                    EditorUtility.RevealInFinder( StaticFolderPath );
                }
                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();

            if( GUI.changed )
            {
                IsFolderPathValidFlag = Directory.Exists( StaticFolderPath );
            }
        }

        private static void DrawDataListContent()
        {
            foreach( SceneHoudiniRoot data in HoudiniHDASaver_Data )
            {
                data.DrawGui();
            }
        }

        private static void RenameAll()
        {
            string consoleLog = string.Empty;
            int affectedAssetsCount = 0;
            foreach( SceneHoudiniRoot data in HoudiniHDASaver_Data )
            {
                if( data.ToogleScene )
                {
                    string log = string.Empty;
                    affectedAssetsCount += data.RenameGameObjects( out log );
                    consoleLog += log;
                }
            }
            Debug.Log( $"Houdini HDA Save Helper : Rename All ({affectedAssetsCount} assets renamed) {Environment.NewLine}{consoleLog}" );
        }

        private static void RefreshLists()
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
                    HoudiniHDASaver_Data.Add( new SceneHoudiniRoot( currentAssetScene, root, assetIsInSelection ) );
                }
                else
                {
                    HoudiniHDASaver_Data[index].HoudiniRootAssets.Add( root );
                    HoudiniHDASaver_Data[index].HoudiniRootAssetsToogle.Add( assetIsInSelection );
                }
            }
            foreach( SceneHoudiniRoot dataContainer in HoudiniHDASaver_Data )
            {
                dataContainer.ToogleScene = dataContainer.HoudiniRootAssetsToogle.Any( x => x );
            }
        }

        private static void SaveAll()
        {
            string consoleLog = string.Empty;
            int savedAssetsCount = 0;
            foreach( SceneHoudiniRoot data in HoudiniHDASaver_Data )
            {
                if( data.ToogleScene )
                {
                    string log = string.Empty;
                    int result = data.SaveHoudiniAssets( out log );
                    if( result == -1 )
                    {
                        Debug.Log( $"Houdini HDA Save Helper : Save Failed !" );
                    }
                    else
                    {
                        savedAssetsCount += result;
                        consoleLog += log;
                    }
                }
            }
            Debug.Log( $"Houdini HDA Save Helper : Save All ({savedAssetsCount} assets saved) {Environment.NewLine}{consoleLog}" );
        }

        private static string CreateFullPath( string folder_path, string file_name, string file_extension = ".preset" )
        {
            string path = $"{folder_path}{Path.DirectorySeparatorChar}{file_name}.preset";
            Debug.Log( $"CreateFullPath : {path}" );
            return path;
        }
        private static string CheckFolderPathValidity( string folder_path, string scene_path, bool allow_create = true )
        {
            string path = null;
            if( !string.IsNullOrEmpty( folder_path ) && !string.IsNullOrEmpty( scene_path ) && Directory.Exists( folder_path ) )
            {
                char separator = Path.DirectorySeparatorChar;
                path = $"{ folder_path.TrimEnd( separator )}{separator}{scene_path.TrimEnd( separator )}";
                if( allow_create && !Directory.Exists( path ) )
                {
                    Directory.CreateDirectory( path );
                }
                if( Directory.Exists( path ) )
                {
                    IsFolderPathValidFlag = true;
                }
                else
                {
                    IsFolderPathValidFlag = false;
                    path = null;
                }
            }
            Debug.Log( $"CheckFolderPathValidity : {path}" );
            return path;
        }

        private static readonly string HoudiniHdaSaverHelperVersion = "28/05/2019";
        private static bool IsWindowToogle = false;
        private static bool IsFolderPathValidFlag = true;
        private static bool IsWindowInit = false;
        private static string StaticFolderPath = string.Empty;
        private static List<SceneHoudiniRoot> HoudiniHDASaver_Data = new List<SceneHoudiniRoot>();

        private class SceneHoudiniRoot
        {
            public bool ToogleScene = false;
            public bool ToogleAllAsset = false;
            public Scene scene;
            public List<HEU_HoudiniAssetRoot> HoudiniRootAssets = new List<HEU_HoudiniAssetRoot>();
            public List<bool> HoudiniRootAssetsToogle = new List<bool>();
            public string FileSaveNameBase = string.Empty;

            public SceneHoudiniRoot( Scene asset_scene, HEU_HoudiniAssetRoot asset, bool asset_toggle )
            {
                scene = asset_scene;
                HoudiniRootAssets.Add( asset );
                HoudiniRootAssetsToogle.Add( asset_toggle );
                FileSaveNameBase = asset.name;
            }

            public int RenameGameObjects( out string console_log )
            {
                string log = string.Empty;
                int affectedRow = 0;
                for( int i = 0; i < HoudiniRootAssets.Count; i++ )
                {
                    if( HoudiniRootAssetsToogle[i] )
                    {
                        affectedRow++;
                        log += HoudiniRootAssets[i].gameObject.name + "=>";
                        HoudiniRootAssets[i].gameObject.name = FileSaveNameBase + (affectedRow.ToString( "D2" ));
                        log += HoudiniRootAssets[i].gameObject.name + Environment.NewLine;
                    }
                }
                console_log = log;
                return affectedRow;
            }

            public int SaveHoudiniAssets( out string console_log )
            {
                string log = string.Empty;
                int affectedRow = -1;
                string folderPath = CheckFolderPathValidity( StaticFolderPath, scene.name );
                if( folderPath != null )
                {
                    affectedRow = 0;
                    for( int i = 0; i < HoudiniRootAssets.Count; i++ )
                    {
                        if( HoudiniRootAssetsToogle[i] )
                        {
                            affectedRow++;
                            string fullpath = CreateFullPath( folderPath, HoudiniRootAssets[i].gameObject.name );
                            log += fullpath + Environment.NewLine;
                            HEU_AssetPresetUtility.SaveAssetPresetToFile( HoudiniRootAssets[i]._houdiniAsset, fullpath );
                        }
                    }
                }
                else
                {
                    Debug.Log( $"Houdini HDA Save Helper : folder path  not valid. (path={StaticFolderPath})" );
                }
                console_log = log;
                return affectedRow;
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
                                        string consoleLog = string.Empty;
                                        int saveResult = SaveHoudiniAssets( out consoleLog );
                                        if( saveResult == -1 )
                                        {
                                            Debug.Log( $"Houdini HDA Save Helper : Save Failed !" );
                                        }
                                        else
                                        {
                                            Debug.Log( $"Houdini HDA Save Helper : Scene [{scene.name}]: ({saveResult} assets saved) {Environment.NewLine}{consoleLog}" );
                                        }
                                    }
                                    if( GUILayout.Button( "Rename", new GUILayoutOption[] { GUILayout.ExpandWidth( false ), GUILayout.Width( 70 ) } ) )
                                    {
                                        string consoleLog = string.Empty;
                                        int affectedRecords = RenameGameObjects( out consoleLog );
                                        Debug.Log( $"Houdini HDA Save Helper : Scene [{scene.name}]: ({affectedRecords} assets renamed) {Environment.NewLine}{consoleLog}" );
                                    }
                                    FileSaveNameBase = GUILayout.TextField( FileSaveNameBase, new GUILayoutOption[] { GUILayout.ExpandWidth( true ), GUILayout.MinWidth( 140 ) } );
                                }
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginVertical();
                                int AssetIndexToBake = -1;
                                for( int i = 0; i < HoudiniRootAssets.Count; i++ )
                                {
                                    HEU_HoudiniAssetRoot assetRoot = HoudiniRootAssets[i];

                                    EditorGUILayout.BeginHorizontal();
                                    HoudiniRootAssetsToogle[i] = EditorGUILayout.ToggleLeft( assetRoot.name, toggleJustChange ? ToogleAllAsset : HoudiniRootAssetsToogle[i] );
                                    GUILayout.FlexibleSpace();
                                    if( GUILayout.Button( "Save & replace with Baked", new GUILayoutOption[] { GUILayout.ExpandWidth( true ), GUILayout.Width( 170 ) } ) )
                                    {
                                        string folderPath = CheckFolderPathValidity( StaticFolderPath, scene.name );
                                        if( folderPath != null )
                                        {
                                            string assetGameObjectName = assetRoot.gameObject.name;
                                            string fullpath = CreateFullPath( folderPath, assetGameObjectName );
                                            Debug.Log( $"Houdini HDA Save Helper : Scene [{scene.name}] baking {assetGameObjectName}" );

                                            HEU_HoudiniAsset currentAsset = assetRoot._houdiniAsset;
                                            HEU_AssetPresetUtility.SaveAssetPresetToFile( currentAsset, fullpath );
                                            int siblingIndex = currentAsset.transform.parent.GetSiblingIndex();

                                            currentAsset._bakedEvent.AddListener( ( asset, success, outputList ) =>
                                            {
                                                for( int outputIndex = 0; outputIndex < outputList.Count; outputIndex++ )
                                                {
                                                    outputList[0].name = $"{assetGameObjectName}_baked";
                                                    outputList[0].transform.SetSiblingIndex( siblingIndex );
                                                }
                                                Debug.Log( $"Houdini HDA Save Helper : Scene [{scene.name}] {outputList[0].name} baked{Environment.NewLine}Asset ={asset.name}{Environment.NewLine}Success ={success}{Environment.NewLine}Count ={outputList.Count}" );
                                            } );

                                            SceneManager.SetActiveScene( currentAsset.gameObject.scene );
                                            currentAsset.BakeToNewStandalone();
                                            AssetIndexToBake = i;
                                        }
                                        else
                                        {
                                            Debug.Log( $"Houdini HDA Save Helper : folder path  not valid. ({StaticFolderPath})" );
                                        }
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }

                                if( AssetIndexToBake != -1 )
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
        }
#endif
    }
}