using UnityEditor;
using UnityEngine;

namespace FishingCactus
{
    [CustomPropertyDrawer( typeof( MinMaxFloats ), true )]
    [CustomPropertyDrawer( typeof( MinMaxInts ), true )]
    public class MinMaxDrawer : PropertyDrawer
    {
        // -- FIELDS

        private const string MIN_PROPERTY = "_min";
        private const string MIN_LABEL = "Min";
        private const string MAX_PROPERTY = "_max";
        private const string MAX_LABEL = "Max";
        private const float LABEL_WIDTH = 50f;

        // -- UNITY

        public override void OnGUI(
            Rect position,
            SerializedProperty property,
            GUIContent label
            )
        {
            EditorGUI.BeginProperty( position, label, property );

            position = EditorGUI.PrefixLabel( position, GUIUtility.GetControlID( FocusType.Passive ), label );

            position.width /= 2f;
            EditorGUIUtility.labelWidth = LABEL_WIDTH;

            EditorGUI.PropertyField( position, property.FindPropertyRelative( MIN_PROPERTY ), new GUIContent( MIN_LABEL ) );

            position.x += position.width;

            EditorGUI.PropertyField( position, property.FindPropertyRelative( MAX_PROPERTY ), new GUIContent( MAX_LABEL ) );

            EditorGUI.EndProperty();
            EditorGUIUtility.labelWidth = 0f;
        }

        public override float GetPropertyHeight(
            SerializedProperty property,
            GUIContent label
            )
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
