using UnityEditor;
using UnityEngine;
using FishingCactus;

[CustomEditor( typeof( NodeGraph ), true )]
public class NodeGraphDrawer : Editor
{
    // -- PRIVATE

    private SerializedProperty SelectedNodeProperty;
    private SerializedProperty SelectedTransitionProperty;
    private SerializedProperty LinearProperty;
    // -- UNITY

    public void OnEnable()
    {
        SelectedNodeProperty = serializedObject.FindProperty( "SelectedNode" );
        SelectedTransitionProperty = serializedObject.FindProperty( "SelectedTransition" );
        LinearProperty = serializedObject.FindProperty( "ItIsLinear" );
    }

    public override void OnInspectorGUI()
    {
        float label_witdh_backup = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth *= 1.3f;

        serializedObject.Update();
        NodeGraph edited_node_graph = serializedObject.targetObject as NodeGraph;

        EditorGUI.BeginChangeCheck();
        //DrawDefaultInspector();

        EditorGUILayout.LabelField( $"{serializedObject.targetObject.name}" );
        this.DrawUILine( Color.grey );
        EditorGUILayout.LabelField( $"Type : {edited_node_graph.GetType().ToString()}");
        EditorGUILayout.LabelField( $"Node count : {edited_node_graph.NodeTable.Count}");
        EditorGUILayout.LabelField( $"Transition count : {edited_node_graph.TransitionTable.Count}" );
        EditorGUILayout.LabelField( $"Selected Element Index : {edited_node_graph.SelectedElementIndex}" );

        EditorGUILayout.Space();
        this.DrawUILine( Color.grey );
        EditorGUILayout.PropertyField( LinearProperty, new GUIContent( "Linear mode" ) );

        EditorGUILayout.Space();
        this.DrawUILine( Color.grey );

        if( edited_node_graph.SelectedNode.Index != 0 )
        {
            SelectedNodeProperty.isExpanded = true;
            EditorGUILayout.PropertyField( SelectedNodeProperty, new GUIContent( "Selected Node :" ), true );
        }
        else if( edited_node_graph.SelectedTransition.Index != 0 )
        {
            SelectedTransitionProperty.isExpanded = true;
            EditorGUILayout.PropertyField( SelectedTransitionProperty, new GUIContent( "Selected Transition :" ), true );
        }

        if( EditorGUI.EndChangeCheck() )
        {
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUIUtility.labelWidth = label_witdh_backup;
    }
}
