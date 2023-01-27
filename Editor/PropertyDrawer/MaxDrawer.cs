using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer( typeof( MaxAttribute ) )]
public class MaxDrawer : PropertyDrawer
{
    public override void OnGUI(
        Rect position,
        SerializedProperty property,
        GUIContent label
        )
    {
        MaxAttribute max_attribute = attribute as MaxAttribute;

        switch( property.propertyType )
        {
            case SerializedPropertyType.Integer:
                property.intValue = Mathf.Min( ( int )max_attribute.Max, EditorGUI.IntField( position, label, property.intValue ) );
                break;

            case SerializedPropertyType.Float:
                property.floatValue = Mathf.Min( max_attribute.Max, EditorGUI.FloatField( position, label, property.floatValue ) );
                break;

            default:
                EditorGUI.PropertyField( position, property, label, true );
                position.height = EditorGUIUtility.singleLineHeight;
                position.y += EditorGUI.GetPropertyHeight( property, label, true ) + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.HelpBox( position, "Max attribute can only be used on integer or float fields", MessageType.Error );
                break;
        }
    }

    public override float GetPropertyHeight(
        SerializedProperty property,
        GUIContent label
        )
    {
        return property.propertyType switch
        {
            SerializedPropertyType.Integer => EditorGUI.GetPropertyHeight( property, label, false ),
            SerializedPropertyType.Float => EditorGUI.GetPropertyHeight( property, label, false ),
            _ => EditorGUI.GetPropertyHeight( property, label, true ) + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
        };
    }
}
