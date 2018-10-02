using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler
{
    // -- PUBLIC

    [OnOpenAssetAttribute( 1 )]
    public static bool step1( int instanceID, int line )
    {
        Object selected_object = EditorUtility.InstanceIDToObject( instanceID );

        if( selected_object.GetType() == typeof( NodeGraph ) )
        {
            System.Type[] dock_table = { typeof( SceneView ) };

            NodeGraphWindow scene_controller_window = EditorWindow.GetWindow< NodeGraphWindow>(
                "LogicalGraph", true, dock_table
                ) as NodeGraphWindow;

            scene_controller_window.Show();

            return true;
        }

        return false;
    }
}
