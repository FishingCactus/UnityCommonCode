using UnityEngine;
using UnityEditor;

public class EnumDictionaryPropertyDrawer : PropertyDrawer
{
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
            gui_rectangle = EditorGUI.IndentedRect(gui_rectangle);

            float element_x = gui_rectangle.x;
            float element_width = gui_rectangle.width;

            gui_rectangle.width = element_width * 0.5f;

            for( int name_index = 0; name_index < type_property.arraySize; name_index++ )
            {
                gui_rectangle.y += 18.0f;

                gui_rectangle.x = element_x;
                EditorGUI.LabelField(gui_rectangle, new GUIContent(type_property.GetArrayElementAtIndex(name_index).stringValue));

                gui_rectangle.x += gui_rectangle.width;
                EditorGUI.PropertyField(gui_rectangle, values_property.GetArrayElementAtIndex(name_index), GUIContent.none );
            }

            gui_rectangle.x = element_x;
            gui_rectangle.width = element_width;
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
            SerializedProperty list_property = property.FindPropertyRelative("EnumNameTable");

            height_to_use += 20.0f * list_property.arraySize;
        }

        return height_to_use;
    }
}
