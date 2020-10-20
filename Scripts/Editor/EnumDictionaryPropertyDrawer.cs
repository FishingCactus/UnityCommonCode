using UnityEngine;
using UnityEditor;

public class EnumDictionaryPropertyDrawer : PropertyDrawer
{
    // -- FIELDS

    private static float IndentationWidth = 12.0f;

    // -- UNITY

    public override void OnGUI(
        Rect position,
        SerializedProperty property,
        GUIContent label
        )
    {
        Rect gui_rectangle;
        SerializedProperty type_property = property.FindPropertyRelative("EnumNameTable");
        SerializedProperty values_property = property.FindPropertyRelative("ValueTable");

        Debug.Assert( type_property.arraySize == values_property.arraySize, "EnumDictionary array sizes mismatched." );

        gui_rectangle = position;

        EditorGUI.BeginProperty( position, label, property );

        gui_rectangle = position;
        gui_rectangle.y += 10.0f;
        gui_rectangle.height = 18.0f;

        property.isExpanded = EditorGUI.Foldout( gui_rectangle, property.isExpanded, label, InternalEditorStyle.Foldout );

        if( property.isExpanded )
        {
            gui_rectangle.x += IndentationWidth;
            gui_rectangle.width -= IndentationWidth;

            gui_rectangle.y += 18.0f;

            Rect element_rectangle = gui_rectangle;

            for( int name_index = 0; name_index < type_property.arraySize; name_index++ )
            {
                SerializedProperty property_to_draw = values_property.GetArrayElementAtIndex(name_index);

                if( property_to_draw.hasChildren )
                {
                    EditorGUI.PropertyField( gui_rectangle, property_to_draw, new GUIContent( type_property.GetArrayElementAtIndex(name_index).stringValue ), true );
                    gui_rectangle.y += EditorGUI.GetPropertyHeight( property_to_draw, GUIContent.none );
                }
                else
                {
                    element_rectangle.x = gui_rectangle.x;
                    element_rectangle.width = gui_rectangle.width;

                    const float label_width_percentage = 0.33f;
                    element_rectangle.width = gui_rectangle.width * label_width_percentage;
                    EditorGUI.LabelField(element_rectangle, new GUIContent( type_property.GetArrayElementAtIndex(name_index).stringValue ) );

                    element_rectangle.x += element_rectangle.width;
                    element_rectangle.width = gui_rectangle.width * ( 1.0f - label_width_percentage );
                    EditorGUI.PropertyField( element_rectangle, property_to_draw, GUIContent.none );
                    element_rectangle.y += EditorGUI.GetPropertyHeight( property_to_draw, GUIContent.none );
                }
            }

            gui_rectangle.x -= IndentationWidth;
            gui_rectangle.width += IndentationWidth;
        }

        EditorGUI.EndProperty();

        property.serializedObject.ApplyModifiedProperties();
    }

    public override float GetPropertyHeight(
        SerializedProperty property,
        GUIContent label
        )
    {
        float height_to_use = 28.0f;

        if (property.isExpanded)
        {
            SerializedProperty list_property = property.FindPropertyRelative("ValueTable");

            foreach( SerializedProperty element_property in list_property )
            {
                height_to_use += EditorGUI.GetPropertyHeight(element_property, GUIContent.none);
            }
        }

        height_to_use += 4.0f;

        return height_to_use;
    }
}
