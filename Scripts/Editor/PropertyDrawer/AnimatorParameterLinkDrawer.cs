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

        private static RuntimeAnimatorController LastUsedAnimationController = null;

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

        // -- UNITY

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            Rect local_position = position;
            SerializedProperty controller_link_property = property.FindPropertyRelative( "LinkedAnimatorController" );
            SerializedProperty name_property = property.FindPropertyRelative( "_ParameterName" );
            RuntimeAnimatorController linked_controller = controller_link_property.objectReferenceValue as AnimatorController;

            if( LastUsedAnimationController != null
                && linked_controller == null
                )
            {
                linked_controller = LastUsedAnimationController;
                controller_link_property.objectReferenceValue = LastUsedAnimationController;
            }
            else if( LastUsedAnimationController != linked_controller )
            {
                LastUsedAnimationController = linked_controller;
            }

            if( LastUsedAnimationController == null )
            {
                GUI.enabled = false;
                EditorGUI.TextField( local_position, label, name_property.stringValue );
                GUI.enabled = true;
            }
            else
            {
                FillParameterNameTable(
                    LastUsedAnimationController as AnimatorController,
                    name_property.stringValue,
                    AnimatorParameterLink.TypeTable[property.type]
                    );

                local_position.width = position.width - InternalEditorStyle.MinimalButtonWidth;

                EditorGUI.BeginChangeCheck();
                SelectionIndex = EditorGUI.Popup( local_position, label, SelectionIndex, ParameterNameTable.ToArray() );
                if( EditorGUI.EndChangeCheck() )
                {
                    name_property.stringValue = ParameterNameTable[SelectionIndex].text;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }

            GUI.enabled = true;
            local_position.x += local_position.width;
            local_position.width = InternalEditorStyle.MinimalButtonWidth;

            if( GUI.Button( local_position, "..." ) )
            {
                ItHasRequestAssetPicking = true;
                EditorGUIUtility.ShowObjectPicker<UnityEngine.Object>( null, true, string.Format( "t:{0}", typeof( AnimatorController ).Name ), EditorGUIUtility.GetControlID( FocusType.Passive ) + 100 );
            }

            if( ItHasRequestAssetPicking
                && Event.current.commandName == "ObjectSelectorClosed"
                )
            {
                controller_link_property.objectReferenceValue = EditorGUIUtility.GetObjectPickerObject();
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
