using UnityEditor;
using UnityEngine;

namespace DGamesCore
{
    public class WizzardUtils
    {
        // -- TYPES

        public enum PanelMode
        {
            File = 0,
            Folder
        }

        // -- METHODS

        public static string DisplayPathSelector(
            string original_path,
            string description,
            PanelMode panel_mode,
            string file_extension = ""
            )
        {
            string result_filepath = original_path;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( $"{description}", $"{original_path}" );

            if( GUILayout.Button( "...", GUILayout.Width( 20 ) ) )
            {
                string selected_path;

                if( panel_mode == PanelMode.File )
                {
                    selected_path = EditorUtility.OpenFilePanel( description, "", file_extension );
                }
                else
                {
                    selected_path = EditorUtility.OpenFolderPanel( description, original_path, "" );
                }

                if( !string.IsNullOrEmpty( selected_path ) )
                {
                    if( selected_path.StartsWith( Application.dataPath ) )
                    {
                        selected_path = "Assets" + selected_path.Substring( Application.dataPath.Length );
                    }

                    if( panel_mode == PanelMode.Folder
                        && !selected_path.EndsWith( "/" )
                        )
                    {
                        selected_path += '/';
                    }

                    result_filepath = selected_path;
                }
            }

            EditorGUILayout.EndHorizontal();

            return result_filepath;
        }
    }
}
