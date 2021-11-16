using UnityEditor;
using UnityEngine;

namespace FishingCactus
{
    [CustomPropertyDrawer(typeof(IntegerRange), true)]
    [CustomPropertyDrawer(typeof(FloatRange), true)]
    public class RangeDrawer : PropertyDrawer
    {
        // -- PRIVATE

        private float OneLineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // -- UNITY

        public override float GetPropertyHeight(
            SerializedProperty property, GUIContent label
            )
        {
            return property.isExpanded ? 3.0f * OneLineHeight : OneLineHeight;
        }

        public override void OnGUI(
            Rect position,
            SerializedProperty property,
            GUIContent label
            )
        {
            Rect local_rectangle = position;
            SerializedProperty minimum_value_property = property.FindPropertyRelative( "_MinimumValue" );
            SerializedProperty maximum_value_property = property.FindPropertyRelative( "_MaximumValue" );

            EditorGUI.BeginChangeCheck();

            local_rectangle.height = EditorGUIUtility.singleLineHeight;

            if( maximum_value_property.propertyType == SerializedPropertyType.Integer )
            {
                property.isExpanded = EditorGUI.Foldout(
                    local_rectangle,
                    property.isExpanded,
                    $"{label.text}   [{minimum_value_property.intValue};{maximum_value_property.intValue}["
                    );
            }
            else
            {
                property.isExpanded = EditorGUI.Foldout(
                    local_rectangle,
                    property.isExpanded,
                    $"{label.text}   [{minimum_value_property.floatValue};{maximum_value_property.floatValue}]"
                    );
            }

            if( property.isExpanded )
            {
                local_rectangle = EditorGUI.IndentedRect( local_rectangle );

                local_rectangle.y += OneLineHeight;
                EditorGUI.PropertyField( local_rectangle, minimum_value_property );

                local_rectangle.y += OneLineHeight;
                EditorGUI.PropertyField( local_rectangle, maximum_value_property );
            }

            if (EditorGUI.EndChangeCheck())
            {
                switch( minimum_value_property.propertyType )
                {
                    case SerializedPropertyType.Float:
                    {
                        if( minimum_value_property.floatValue > maximum_value_property.floatValue )
                        {
                            maximum_value_property.floatValue = minimum_value_property.floatValue  + 1.0f;
                        }
                    }
                    break;

                    case SerializedPropertyType.Integer:
                    {
                        if (minimum_value_property.intValue > maximum_value_property.intValue )
                        {
                            maximum_value_property.intValue = minimum_value_property.intValue + 1;
                        }
                    }
                    break;

                    default:
                    {
                        Debug.LogWarning( $"{property.name} : Range parameter type {minimum_value_property.propertyType.ToString()} not supported." );
                    }
                    break;
                }
            }
        }
    }
}
