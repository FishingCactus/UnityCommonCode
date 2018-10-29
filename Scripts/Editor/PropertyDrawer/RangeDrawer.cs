using UnityEditor;
using UnityEngine;

namespace FishingCactus
{
    [CustomPropertyDrawer(typeof(IntegerRange), true)]
    [CustomPropertyDrawer(typeof(FloatRange), true)]
    public class RangeDrawer : PropertyDrawer
    {
        // -- UNITY

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

            local_rectangle.width *= 0.4f;
            EditorGUI.LabelField( local_rectangle, $"{label.text}" );

            local_rectangle.x += local_rectangle.width;
            local_rectangle.width = position.width * 0.05f;
            EditorGUI.LabelField( local_rectangle, "[" );

            local_rectangle.x += local_rectangle.width;
            local_rectangle.width = position.width * 0.20f;
            EditorGUI.PropertyField( local_rectangle, minimum_value_property, new GUIContent( "" ) );

            local_rectangle.x += local_rectangle.width;
            local_rectangle.width = position.width * 0.05f;
            EditorGUI.LabelField( local_rectangle, " ;" );

            local_rectangle.x += local_rectangle.width;
            local_rectangle.width = position.width * 0.20f;
            EditorGUI.PropertyField( local_rectangle, maximum_value_property, new GUIContent( "" ) );

            local_rectangle.x += local_rectangle.width;
            local_rectangle.width = position.width * 0.05f;
            EditorGUI.LabelField( local_rectangle, maximum_value_property.propertyType == SerializedPropertyType.Integer ? " [" : " ]" );

            if (EditorGUI.EndChangeCheck())
            {
                switch( minimum_value_property.propertyType )
                {
                    case SerializedPropertyType.Float:
                    {
                        if( minimum_value_property.floatValue > maximum_value_property.floatValue )
                        {
                            minimum_value_property.floatValue = maximum_value_property.floatValue - 0.1f;
                        }
                    }
                    break;

                    case SerializedPropertyType.Integer:
                    {
                        if (minimum_value_property.intValue > maximum_value_property.intValue )
                        {
                            minimum_value_property.intValue = maximum_value_property.intValue - 1;
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
