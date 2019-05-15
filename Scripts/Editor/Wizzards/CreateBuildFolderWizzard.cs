using UnityEngine;
using UnityEditor;
using System.IO;

namespace FishingCactus
{
    public class CreateBuildFolderWizzard : ScriptableWizard
    {
        // -- PUBLIC

        public string PreojectBuildFolderPath = null;

        // -- PRIVATE

        private readonly string BuildFolderKey = "FC_BuildFolderKey";
        private string BuildVersionFolderPath = null;

        // -- UNITY

        [MenuItem( "FishingCactus/Build/Create Version Folder", false, 100 )]
        static void CreateWindow()
        {
            DisplayWizard<CreateBuildFolderWizzard>("Create Build Version Folder.", "Create" );
        }

        private void Awake()
        {
            PreojectBuildFolderPath = EditorPrefs.GetString( BuildFolderKey, Path.Combine( Application.dataPath, $"../_Builds" ) );
            BuildVersionFolderPath = Path.Combine( PreojectBuildFolderPath, $"v.{Application.version}" );
        }

        protected override bool DrawWizardGUI()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( "Project Build Folder : ", PreojectBuildFolderPath );

            if( GUILayout.Button( "..." ) )
            {
                EditorGUI.BeginChangeCheck();

                PreojectBuildFolderPath = EditorUtility.OpenFolderPanel("Select Build folder", PreojectBuildFolderPath, "");

                if( !string.IsNullOrEmpty( PreojectBuildFolderPath ) )
                {
                    EditorPrefs.SetString( BuildFolderKey, PreojectBuildFolderPath );
                    BuildVersionFolderPath = Path.Combine( PreojectBuildFolderPath, $"v.{Application.version}" );
                }
            }
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField( $"Current build version : {Application.version}" );
            EditorGUILayout.LabelField( "Version Folder : ", Path.Combine( PreojectBuildFolderPath, $"v.{Application.version}" ) );

            isValid = !Directory.Exists( BuildVersionFolderPath );

            return true;
        }

        void OnWizardCreate()
        {
            Directory.CreateDirectory( BuildVersionFolderPath );

            Debug.Log( $"Build Folder created : {BuildVersionFolderPath}" );
        }
    }
}
