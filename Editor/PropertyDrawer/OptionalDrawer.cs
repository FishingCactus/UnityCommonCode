using UnityEditor;
using UnityEngine;

namespace FishingCactus
{
    [CustomPropertyDrawer( typeof( Optional<> ) )]
    public sealed class OptionalDrawer : PropertyDrawer
    {
        public override void OnGUI(
            Rect position,
            SerializedProperty property,
            GUIContent label
        )
        {
            EditorGUI.BeginProperty( position, label, property );

            string label_string = label.text;

            var enabled_property = property.FindPropertyRelative( "_Enabled" );
            var value_property = property.FindPropertyRelative( "_Value" );

            Rect enabled_property_rect = position;
            enabled_property_rect.height = EditorGUI.GetPropertyHeight( enabled_property );
            enabled_property_rect.width = enabled_property_rect.height;

            EditorGUI.PropertyField( enabled_property_rect, enabled_property, GUIContent.none );

            position.x = enabled_property_rect.x + enabled_property_rect.width + 15f;
            position.width -= enabled_property_rect.width + 15f;

            EditorGUI.BeginDisabledGroup( !enabled_property.boolValue );
            EditorGUI.PropertyField( position, value_property, new GUIContent( label_string ), true );
            EditorGUI.EndDisabledGroup();

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(
            SerializedProperty property,
            GUIContent label
            )
        {
            var value_property = property.FindPropertyRelative( "_Value" );
            return EditorGUI.GetPropertyHeight( value_property );
        }
    }
}
