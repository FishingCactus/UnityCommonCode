using UnityEngine;
using UnityEditor;

namespace FishingCactus
{
    [CustomPropertyDrawer( typeof( ExposedScriptableObjectAttribute ), true )]
    public class ExposedScriptableObjectAttributeDrawer : PropertyDrawer
    {
        // -- FIELDS

        private readonly float IndentationSize = 12.0f;

        // -- PROPERTIES

        ExposedScriptableObjectAttribute scriptable_attribute
        {
            get{ return ( ExposedScriptableObjectAttribute )attribute; }
        }

        // -- UNITY

        public override void OnGUI(
            Rect position,
            SerializedProperty property,
            GUIContent label
            )
        {
            Rect property_position = new Rect( position );
            property_position.x += IndentationSize;
            property_position.width -= IndentationSize;
            property_position.height = EditorGUIUtility.singleLineHeight;

            Rect header_position = property_position;
            header_position.width = EditorGUIUtility.labelWidth - IndentationSize;

            if( property.objectReferenceValue == null )
            {
                GUI.enabled = false;
                property.isExpanded = false;
            }

            var property_is_expanded = EditorGUI.Foldout( header_position, property.isExpanded, label, true, InternalEditorStyle.BlueFoldout );
            GUI.enabled = true;

            if( scriptable_attribute.ItMustDisplayObjectSelector )
            {
                header_position.x += header_position.width;
                header_position.width = property_position.width - EditorGUIUtility.labelWidth + IndentationSize;

                EditorGUI.PropertyField( header_position, property, GUIContent.none, true );
            }

            if( property.objectReferenceValue == null)
            {
                return;
            }

            property.isExpanded = property_is_expanded;

            if( !property.isExpanded )
            {
                return;
            }

            property_position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            SerializedObject serialized_object = new SerializedObject( property.objectReferenceValue );
            SerializedProperty child_property = serialized_object.GetIterator();

            GUI.enabled = false;

            child_property.NextVisible( true );

            do
            {
                if( child_property.isArray )
                {
                    EditorGUI.indentLevel++;
                }

                EditorGUI.PropertyField( property_position, child_property, true);

                property_position.y += EditorGUI.GetPropertyHeight( child_property );
                property_position.y += EditorGUIUtility.standardVerticalSpacing;

                GUI.enabled = true;

                if( child_property.isArray )
                {
                    EditorGUI.indentLevel--;
                }
            }
            while( child_property.NextVisible( false ) );

            serialized_object.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(
            SerializedProperty property,
            GUIContent label
            )
        {
            float total_height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if( property.objectReferenceValue == null
                || !property.isExpanded
                )
            {
                return total_height;
            }

            SerializedObject serialized_object = new SerializedObject( property.objectReferenceValue );
            SerializedProperty child_property = serialized_object.GetIterator();

            child_property.NextVisible( true );

            do
            {
                total_height += EditorGUI.GetPropertyHeight( child_property );
                total_height += EditorGUIUtility.standardVerticalSpacing;
            }
            while( child_property.NextVisible( false ) );

            total_height += EditorGUIUtility.standardVerticalSpacing;

            return total_height;
        }
    }
}