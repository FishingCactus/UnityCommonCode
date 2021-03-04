using UnityEngine;
using UnityEditor;

public class InternalEditorStyle
{
    // -- PUBLIC

    public static float MinimalButtonWidth = 24.0f;

    public static GUIStyle Foldout
    {
        get
        {
            if( CustomFoldoutStyle == null )
            {
                CustomFoldoutStyle = new GUIStyle( EditorStyles.foldout );
                CustomFoldoutStyle.fontStyle = FontStyle.Normal;

                SetTextsColor( CustomFoldoutStyle, GroupColor );
            }

            return CustomFoldoutStyle;
        }
    }

    public static GUIStyle BoldFoldout
    {
        get
        {
            if( CustomBoldFoldoutStyle == null )
            {
                CustomBoldFoldoutStyle = new GUIStyle( EditorStyles.foldout );
                CustomBoldFoldoutStyle.fontStyle = FontStyle.Bold;

                SetTextsColor( CustomBoldFoldoutStyle, GroupColor );
            }

            return CustomBoldFoldoutStyle;
        }
    }

    public static GUIStyle DebugFoldout
    {
        get
        {
            if( CustomDebugFoldoutStyle == null )
            {
                CustomDebugFoldoutStyle = new GUIStyle( EditorStyles.foldout );
                CustomDebugFoldoutStyle.fontStyle = FontStyle.Bold;

                SetTextsColor( CustomDebugFoldoutStyle, Color.red );
            }

            return CustomDebugFoldoutStyle;
        }
    }

    public static GUIStyle Group
    {
        get
        {
            if ( CustomGroupStyle == null )
            {
                CustomGroupStyle = new GUIStyle( EditorStyles.boldLabel );
                CustomGroupStyle.fontStyle = FontStyle.Bold;

                SetTextsColor( CustomGroupStyle, GroupColor );
            }

            return CustomGroupStyle;
        }
    }

    public static GUIStyle ErrorGroup
    {
        get
        {
            if( CustomErrorGroupStyle == null )
            {
                CustomErrorGroupStyle = new GUIStyle( EditorStyles.boldLabel );
                CustomErrorGroupStyle.fontStyle = FontStyle.Bold;

                SetTextsColor( CustomErrorGroupStyle, Color.red );
            }

            return CustomErrorGroupStyle;
        }
    }

    public static GUIStyle SubGroup
    {
        get
        {
            if( CustomSubGroupStyle == null )
            {
                CustomSubGroupStyle = new GUIStyle( EditorStyles.boldLabel );

                CustomSubGroupStyle.fontSize = 10;
                CustomSubGroupStyle.fontStyle = FontStyle.Italic;

                SetTextsColor( CustomSubGroupStyle, GroupColor );
            }

            return CustomSubGroupStyle;
        }
    }

    public static GUIStyle LockerToggle
    {
        get
        {
            if( CustomSubGroupStyle == null )
            {
                CustomLockerToggleStyle = new GUIStyle( EditorStyles.toggle );

                CustomLockerToggleStyle.alignment = TextAnchor.MiddleCenter;

                CustomLockerToggleStyle.normal.background = EditorGUIUtility.IconContent( "LockIcon-On" ).image as Texture2D;
                CustomLockerToggleStyle.hover.background = EditorGUIUtility.IconContent( "LockIcon-On" ).image as Texture2D;
                CustomLockerToggleStyle.focused.background = EditorGUIUtility.IconContent( "LockIcon-On" ).image as Texture2D;
                CustomLockerToggleStyle.active.background = EditorGUIUtility.IconContent( "LockIcon-On" ).image as Texture2D;

                CustomLockerToggleStyle.onFocused.background = EditorGUIUtility.IconContent( "LockIcon" ).image as Texture2D;
                CustomLockerToggleStyle.onNormal.background = EditorGUIUtility.IconContent( "LockIcon" ).image as Texture2D;
                CustomLockerToggleStyle.onHover.background = EditorGUIUtility.IconContent( "LockIcon" ).image as Texture2D;
                CustomLockerToggleStyle.onActive.background = EditorGUIUtility.IconContent( "LockIcon" ).image as Texture2D;
            }

            return CustomLockerToggleStyle;
        }
    }

    public static GUIStyle CenteredToggle
    {
        get
        {
            if( CustomCenteredToggleStyle == null )
            {
                CustomCenteredToggleStyle = new GUIStyle( EditorStyles.toggle );
                CustomCenteredToggleStyle.alignment = TextAnchor.MiddleCenter;
            }

            return CustomCenteredToggleStyle;
        }
    }

    public static GUIStyle EyeButton
    {
        get
        {
            if( CustomEyeButtonStyle == null )
            {
                CustomEyeButtonStyle = new GUIStyle( EditorStyles.miniButton );
                CustomEyeButtonStyle.alignment = TextAnchor.UpperCenter;
                CustomEyeButtonStyle.normal.background = null;
            }

            return CustomEyeButtonStyle;
        }
    }

    public static GUIStyle CenteredLabelStyle
    {
        get
        {
            if( CustomCenteredLabelStyle == null )
            {
                CustomCenteredLabelStyle = new GUIStyle( EditorStyles.label );
                CustomCenteredLabelStyle.alignment = TextAnchor.MiddleCenter;
            }

            return CustomCenteredLabelStyle;
        }
    }

    public static GUIStyle RightLabelStyle
    {
        get
        {
            if( CustomRightLabelStyle == null )
            {
                CustomRightLabelStyle = new GUIStyle( EditorStyles.label );
                CustomRightLabelStyle.alignment = TextAnchor.MiddleRight;
            }

            return CustomRightLabelStyle;
        }
    }

    public static GUIStyle BoldLabelStyle
    {
        get
        {
            if( CustomBoldLabelStyle == null )
            {
                CustomBoldLabelStyle = new GUIStyle( EditorStyles.boldLabel );
                CustomBoldLabelStyle.fontStyle = FontStyle.Bold;
            }

            return CustomBoldLabelStyle;
        }
    }

    public static GUIStyle BoldGreenLabelStyle
    {
        get
        {
            if( CustomBoldGreenLabelStyle == null )
            {
                CustomBoldGreenLabelStyle = new GUIStyle( EditorStyles.boldLabel );
                CustomBoldGreenLabelStyle.fontStyle = FontStyle.Bold;

                SetTextsColor( CustomBoldGreenLabelStyle, new Color( 0.1f, 1.0f, 0.1f, 1.0f ) );
            }

            return CustomBoldGreenLabelStyle;
        }
    }

    // -- PRIVATE

    private static GUIStyle CustomFoldoutStyle;
    private static GUIStyle CustomBoldFoldoutStyle;
    private static GUIStyle CustomDebugFoldoutStyle;
    private static GUIStyle CustomGroupStyle;
    private static GUIStyle CustomErrorGroupStyle;
    private static GUIStyle CustomSubGroupStyle;
    private static GUIStyle CustomLockerToggleStyle;
    private static GUIStyle CustomCenteredToggleStyle;
    private static GUIStyle CustomEyeButtonStyle;
    private static GUIStyle CustomCenteredLabelStyle;
    private static GUIStyle CustomRightLabelStyle;
    private static GUIStyle CustomBoldLabelStyle;
    private static GUIStyle CustomBoldGreenLabelStyle;

    private static Color GroupColor = new Color( 0.29f, 0.69f, 1.0f, 1.0f );

    private static void SetTextsColor(
        GUIStyle style_to_update,
        Color new_text_color
        )
    {
        style_to_update.normal.textColor = new_text_color;
        style_to_update.onNormal.textColor = new_text_color;
        style_to_update.hover.textColor = new_text_color;
        style_to_update.onHover.textColor = new_text_color;
        style_to_update.focused.textColor = new_text_color;
        style_to_update.onFocused.textColor = new_text_color;
        style_to_update.active.textColor = new_text_color;
        style_to_update.onActive.textColor = new_text_color;
    }
}
