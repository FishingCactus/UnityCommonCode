using UnityEditor;
using UnityEngine;

namespace FishingCactus
{
    [CustomPropertyDrawer( typeof( TagLink ), true )]
    public class TagLinkDrawer : PropertyDrawer
    {
        // -- UNITY

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            SerializedProperty tag_name_property = property.FindPropertyRelative( "_TagName" );

            tag_name_property.stringValue = EditorGUI.TagField( position, label, tag_name_property.stringValue );
        }
    }
}
