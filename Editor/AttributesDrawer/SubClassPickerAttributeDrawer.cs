using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FishingCactus
{
    public class SubClassPickerAttributeDrawer : MonoBehaviour
    {
        [CustomPropertyDrawer( typeof( SubClassPicker ) )]
        public class SubClassPickerDrawer : PropertyDrawer
        {
            // -- FIELDS

            private const string DefaultDisplay = "Pick a type";

            // -- METHODS

            private IEnumerable<Type> GetSubClasses(
                Type base_type
                )
            {
                return Assembly.GetAssembly( base_type )
                    .GetTypes()
                    .Where( type => type.IsClass && !type.IsAbstract && base_type.IsAssignableFrom( type ) );
            }

            // -- UNITY

            public override float GetPropertyHeight(
                SerializedProperty property,
                GUIContent label
                )
            {
                return EditorGUI.GetPropertyHeight( property, label );
            }

            public override void OnGUI(
                Rect position,
                SerializedProperty property,
                GUIContent label
                )
            {
                Type field_type = fieldInfo.FieldType;

                string type_name = property.managedReferenceValue?.GetType().Name ?? DefaultDisplay;

                Rect drop_down_rect = position;

                drop_down_rect.x += EditorGUIUtility.labelWidth + 2;
                drop_down_rect.width -= EditorGUIUtility.labelWidth + 2;
                drop_down_rect.height = EditorGUIUtility.singleLineHeight;

                if( EditorGUI.DropdownButton( drop_down_rect, new( type_name ), FocusType.Keyboard ) )
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem( new GUIContent( "None" ), property.managedReferenceValue == null, () =>
                    {
                        property.managedReferenceValue = null;
                        property.serializedObject.ApplyModifiedProperties();
                    } );

                    foreach( Type type in GetSubClasses( field_type ) )
                    {
                        menu.AddItem( new GUIContent( type.Name ), type_name == type.Name, () =>
                        {
                            property.managedReferenceValue = type.GetConstructor( Type.EmptyTypes ).Invoke( null );
                            property.serializedObject.ApplyModifiedProperties();
                        } );

                        menu.ShowAsContext();
                    }
                }

                EditorGUI.PropertyField( position, property, label, true );
            }
        }
    }
}