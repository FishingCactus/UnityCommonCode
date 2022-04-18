// source : https://forum.unity.com/threads/conditional-compiling-check-if-a-namespace-plugin-is-available-using-if-endif-c.477121/

using UnityEditor;

namespace FishingCactus
{
    public static class EditorUtils
    {
        // PUBLIC

        public static void AddDefineIfNecessary( string define, BuildTargetGroup build_target_group )
        {
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup( build_target_group );

            if( defines == null ) { defines = define; }
            else if( defines.Length == 0 ) { defines = define; }
            else { if( defines.IndexOf( define, 0 ) < 0 ) { defines += ";" + define; } }

            PlayerSettings.SetScriptingDefineSymbolsForGroup( build_target_group, defines );
        }

        public static void RemoveDefineIfNecessary( string define, BuildTargetGroup build_target_group )
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup( build_target_group );

            if( defines.StartsWith( define + ";" ) )
            {
                // First of multiple defines.
                defines = defines.Remove( 0, define.Length + 1 );
            }
            else if( defines.StartsWith( define ) )
            {
                // The only define.
                defines = defines.Remove( 0, define.Length );
            }
            else if( defines.EndsWith( ";" + define ) )
            {
                // Last of multiple defines.
                defines = defines.Remove( defines.Length - define.Length - 1, define.Length + 1 );
            }
            else
            {
                // Somewhere in the middle or not defined.
                var index = defines.IndexOf( define, 0, System.StringComparison.Ordinal );
                if( index >= 0 ) { defines = defines.Remove( index, define.Length + 1 ); }
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup( build_target_group, defines );
        }

        public static bool IsDefined( string define, BuildTargetGroup build_target_group )
        {
            bool found = false;
            string[] defines = PlayerSettings.GetScriptingDefineSymbolsForGroup( build_target_group ).Split( ';' );
            int index = 0;
            for( ; index < defines.Length && !defines[index].Equals( define ); index++ ) ;
            if( index != defines.Length )
            {
                found = true;
            }
            return found;
        }
    }
}