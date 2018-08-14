﻿using UnityEngine;
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

            gui_rectangle.y += 18.0f;

            for( int name_index = 0; name_index < type_property.arraySize; name_index++ )
            {
                const float label_width_percentage = 0.25f;

                gui_rectangle.x = element_x;
                gui_rectangle.width = element_width * label_width_percentage;
                EditorGUI.LabelField(gui_rectangle, new GUIContent(type_property.GetArrayElementAtIndex(name_index).stringValue));

                gui_rectangle.x += gui_rectangle.width;
                gui_rectangle.width = element_width * ( 1.0f - label_width_percentage );
                EditorGUI.PropertyField(gui_rectangle, values_property.GetArrayElementAtIndex(name_index), GUIContent.none );
                gui_rectangle.y += EditorGUI.GetPropertyHeight( values_property.GetArrayElementAtIndex(name_index), GUIContent.none);
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
            SerializedProperty list_property = property.FindPropertyRelative("ValueTable");

            foreach( SerializedProperty element_property in list_property )
            {
                height_to_use += EditorGUI.GetPropertyHeight(element_property, GUIContent.none);
            }
        }

        return height_to_use;
    }
}
