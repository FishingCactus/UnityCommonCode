using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace FishingCactus
{
    [CustomPropertyDrawer( typeof( BooleanAnimatorParameterLink ), true )]
    [CustomPropertyDrawer( typeof( TriggerAnimatorParameterLink ), true )]
    [CustomPropertyDrawer( typeof( IntegerAnimatorParameterLink ), true )]
    [CustomPropertyDrawer( typeof( FloatAnimatorParameterLink ), true )]
    public class AnimatorParameterLinkDrawer : PropertyDrawer
    {
        // -- PRIVATE

        private bool ItHasRequestAssetPicking = false;
        private List<GUIContent> ParameterNameTable = new List<GUIContent>();
        private int SelectionIndex = 0;

        private void FillParameterNameTable(
            AnimatorController selected_animator_controller,
            string current_name,
            AnimatorControllerParameterType type_to_filter
            )
        {
            ParameterNameTable.Clear();

            if( selected_animator_controller )
            {
                foreach( AnimatorControllerParameter parameter in selected_animator_controller.parameters )
                {
                    if( parameter.type == type_to_filter )
                    {
                        ParameterNameTable.Add( new GUIContent( parameter.name ) );
                    }
                }
            }

            SelectionIndex = ArrayUtility.FindIndex( ParameterNameTable.ToArray(), x => x.text == current_name );
        }

        private AnimatorControllerParameterType GetFilterType(
            string property_type_name
            )
        {
            if( property_type_name == typeof( BooleanAnimatorParameterLink ).Name )
            {
                return AnimatorControllerParameterType.Bool;
            }
            else if( property_type_name == typeof( IntegerAnimatorParameterLink ).Name )
            {
                return AnimatorControllerParameterType.Int;
            }
            else if( property_type_name == typeof( FloatAnimatorParameterLink ).Name )
            {
                return AnimatorControllerParameterType.Float;
            }

            return AnimatorControllerParameterType.Trigger;
        }

        // -- UNITY

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            Rect local_position = position;
            SerializedProperty link_property = property.FindPropertyRelative( "LinkedAnimatorController" );
            SerializedProperty name_property = property.FindPropertyRelative( "_ParameterName" );

            if( ParameterNameTable.Count == 0
                && link_property.objectReferenceValue
                )
            {
                FillParameterNameTable(
                    link_property.objectReferenceValue ? link_property.objectReferenceValue as AnimatorController : null,
                    name_property.stringValue,
                    GetFilterType( property.type )
                    );
            }

            GUI.enabled = link_property.objectReferenceValue ? true : false;
            local_position.width = position.width - 24.0f;

            EditorGUI.BeginChangeCheck();
            SelectionIndex = EditorGUI.Popup( local_position, label, SelectionIndex, ParameterNameTable.ToArray() );
            if( EditorGUI.EndChangeCheck() )
            {
                name_property.stringValue = ParameterNameTable[SelectionIndex].text;
                property.serializedObject.ApplyModifiedProperties();
            }

            GUI.enabled = true;
            local_position.x += local_position.width;
            local_position.width = 24.0f;

            if( GUI.Button( local_position, "..." ) )
            {
                ItHasRequestAssetPicking = true;
                EditorGUIUtility.ShowObjectPicker<UnityEngine.Object>( null, true, string.Format( "t:{0}", typeof( AnimatorController ).Name ), EditorGUIUtility.GetControlID( FocusType.Passive ) + 100 );
            }

            if( ItHasRequestAssetPicking
                && Event.current.commandName == "ObjectSelectorClosed"
                )
            {
                link_property.objectReferenceValue = EditorGUIUtility.GetObjectPickerObject();
                property.serializedObject.ApplyModifiedProperties();

                if( link_property.objectReferenceValue == null )
                {
                    ParameterNameTable.Clear();

                    name_property.stringValue = null;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
