using System;
using UnityEngine;
using UnityEditor;

namespace FishingCactus
{
    [CustomPropertyDrawer( typeof( EnumFlagsAttribute ) )]
    class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(
            Rect position,
            SerializedProperty property,
            GUIContent label
            )
        {
            label = EditorGUI.BeginProperty( position, label, property );

            var old_value = (Enum)fieldInfo.GetValue( property.serializedObject.targetObject );
            var new_value = EditorGUI.EnumFlagsField( position, label, old_value );

            if( !new_value.Equals( old_value ) )
            {
                property.intValue = (int)Convert.ChangeType( new_value, fieldInfo.FieldType );
            }

            EditorGUI.EndProperty();
        }
    }
}
