using UnityEngine;
using UnityEditor;

public class InternalEditorStyle
{
    // -- PUBLIC

    public static float MinimalButtonWidth = 24.0f;

    public static GUIStyle BlackBoldLabel
    {
        get
        {
            if( BlackBoldLabelStyle == null )
            {
                BlackBoldLabelStyle = new GUIStyle( EditorStyles.boldLabel );
                BlackBoldLabelStyle.fontStyle = FontStyle.Bold;

                SetTextsColor( BlackBoldLabelStyle, Color.black );
            }

            return BlackBoldLabelStyle;
        }
    }

    public static GUIStyle BlueFoldout
    {
        get
        {
            if( BlueFoldoutStyle == null )
            {
                BlueFoldoutStyle = new GUIStyle( EditorStyles.foldout );
                BlueFoldoutStyle.fontStyle = FontStyle.Normal;

                SetTextsColor( BlueFoldoutStyle, BlueColor );
            }

            return BlueFoldoutStyle;
        }
    }

    public static GUIStyle BlueBoldFoldout
    {
        get
        {
            if( BlueBoldFoldoutStyle == null )
            {
                BlueBoldFoldoutStyle = new GUIStyle( EditorStyles.foldout );
                BlueBoldFoldoutStyle.fontStyle = FontStyle.Bold;

                SetTextsColor( BlueBoldFoldoutStyle, BlueColor );
            }

            return BlueBoldFoldoutStyle;
        }
    }

    public static GUIStyle BlueGroupLabel
    {
        get
        {
            if ( BlueGroupLabelStyle == null )
            {
                BlueGroupLabelStyle = new GUIStyle( EditorStyles.boldLabel );
                BlueGroupLabelStyle.fontStyle = FontStyle.Bold;

                SetTextsColor( BlueGroupLabelStyle, BlueColor );
            }

            return BlueGroupLabelStyle;
        }
    }

    public static GUIStyle BlueSubGroupLabel
    {
        get
        {
            if( BlueSubGroupLabelStyle == null )
            {
                BlueSubGroupLabelStyle = new GUIStyle( EditorStyles.boldLabel );

                BlueSubGroupLabelStyle.fontSize = 10;
                BlueSubGroupLabelStyle.fontStyle = FontStyle.Italic;

                SetTextsColor( BlueSubGroupLabelStyle, BlueColor );
            }

            return BlueSubGroupLabelStyle;
        }
    }

    public static GUIStyle GreenBoldLabel
    {
        get
        {
            if( GreenBoldLabelStyle == null )
            {
                GreenBoldLabelStyle = new GUIStyle( EditorStyles.boldLabel );
                GreenBoldLabelStyle.fontStyle = FontStyle.Bold;

                SetTextsColor( GreenBoldLabelStyle, GreenColor );
            }

            return GreenBoldLabelStyle;
        }
    }

    public static GUIStyle OrangeItalicLabel
    {
        get
        {
            if( OrangeItalicLabelStyle == null )
            {
                OrangeItalicLabelStyle = new GUIStyle( EditorStyles.boldLabel );
                OrangeItalicLabelStyle.fontStyle = FontStyle.Italic;

                SetTextsColor( OrangeItalicLabelStyle, OrangeColor );
            }

            return OrangeItalicLabelStyle;
        }
    }

    public static GUIStyle RedBoldFoldout
    {
        get
        {
            if( RedBoldFoldoutStyle == null )
            {
                RedBoldFoldoutStyle = new GUIStyle( EditorStyles.foldout );
                RedBoldFoldoutStyle.fontStyle = FontStyle.Bold;

                SetTextsColor( RedBoldFoldoutStyle, RedColor );
            }

            return RedBoldFoldoutStyle;
        }
    }

    public static GUIStyle RedGroupLabel
    {
        get
        {
            if( RedGroupLabelStyle == null )
            {
                RedGroupLabelStyle = new GUIStyle( EditorStyles.boldLabel );
                RedGroupLabelStyle.fontStyle = FontStyle.Bold;

                SetTextsColor( RedGroupLabelStyle, RedColor );
            }

            return RedGroupLabelStyle;
        }
    }

    public static GUIStyle RedItalicLabel
    {
        get
        {
            if( RedItalicLabelStyle == null )
            {
                RedItalicLabelStyle = new GUIStyle( EditorStyles.boldLabel );
                RedItalicLabelStyle.fontStyle = FontStyle.Italic;

                SetTextsColor( RedItalicLabelStyle, RedColor );
            }

            return RedItalicLabelStyle;
        }
    }

    public static GUIStyle LockerToggle
    {
        get
        {
            if( LockerToggleStyle == null )
            {
                LockerToggleStyle = new GUIStyle( EditorStyles.toggle );

                LockerToggleStyle.alignment = TextAnchor.MiddleCenter;

                LockerToggleStyle.normal.background = EditorGUIUtility.IconContent( "LockIcon-On" ).image as Texture2D;
                LockerToggleStyle.hover.background = EditorGUIUtility.IconContent( "LockIcon-On" ).image as Texture2D;
                LockerToggleStyle.focused.background = EditorGUIUtility.IconContent( "LockIcon-On" ).image as Texture2D;
                LockerToggleStyle.active.background = EditorGUIUtility.IconContent( "LockIcon-On" ).image as Texture2D;

                LockerToggleStyle.onFocused.background = EditorGUIUtility.IconContent( "LockIcon" ).image as Texture2D;
                LockerToggleStyle.onNormal.background = EditorGUIUtility.IconContent( "LockIcon" ).image as Texture2D;
                LockerToggleStyle.onHover.background = EditorGUIUtility.IconContent( "LockIcon" ).image as Texture2D;
                LockerToggleStyle.onActive.background = EditorGUIUtility.IconContent( "LockIcon" ).image as Texture2D;
            }

            return LockerToggleStyle;
        }
    }

    public static GUIStyle CenteredToggle
    {
        get
        {
            if( CenteredToggleStyle == null )
            {
                CenteredToggleStyle = new GUIStyle( EditorStyles.toggle );
                CenteredToggleStyle.alignment = TextAnchor.MiddleCenter;
            }

            return CenteredToggleStyle;
        }
    }

    public static GUIStyle EyeButton
    {
        get
        {
            if( EyeButtonStyle == null )
            {
                EyeButtonStyle = new GUIStyle( EditorStyles.miniButton );
                EyeButtonStyle.alignment = TextAnchor.UpperCenter;
                EyeButtonStyle.normal.background = null;
            }

            return EyeButtonStyle;
        }
    }

    public static GUIStyle CenteredLabel
    {
        get
        {
            if( CenteredLabelStyle == null )
            {
                CenteredLabelStyle = new GUIStyle( EditorStyles.label );
                CenteredLabelStyle.alignment = TextAnchor.MiddleCenter;
            }

            return CenteredLabelStyle;
        }
    }

    public static GUIStyle RightLabel
    {
        get
        {
            if( RightLabelStyle == null )
            {
                RightLabelStyle = new GUIStyle( EditorStyles.label );
                RightLabelStyle.alignment = TextAnchor.MiddleRight;
            }

            return RightLabelStyle;
        }
    }

    public static GUIStyle ItalicLabel
    {
        get
        {
            if( ItalicLabelStyle == null )
            {
                ItalicLabelStyle = new GUIStyle( EditorStyles.boldLabel );
                ItalicLabelStyle.fontStyle = FontStyle.Italic;
            }

            return ItalicLabelStyle;
        }
    }

    public static GUIStyle BoldLabel
    {
        get
        {
            if( BoldLabelStyle == null )
            {
                BoldLabelStyle = new GUIStyle( EditorStyles.boldLabel );
                BoldLabelStyle.fontStyle = FontStyle.Bold;
            }

            return BoldLabelStyle;
        }
    }

    // -- PRIVATE

    private static GUIStyle BlueFoldoutStyle;
    private static GUIStyle BlueBoldFoldoutStyle;
    private static GUIStyle BlueGroupLabelStyle;
    private static GUIStyle BlueSubGroupLabelStyle;

    private static GUIStyle RedBoldFoldoutStyle;
    private static GUIStyle RedGroupLabelStyle;
    private static GUIStyle RedItalicLabelStyle;

    private static GUIStyle GreenBoldLabelStyle;

    private static GUIStyle OrangeItalicLabelStyle;

    private static GUIStyle BlackBoldLabelStyle;

    private static GUIStyle LockerToggleStyle;
    private static GUIStyle CenteredToggleStyle;
    private static GUIStyle EyeButtonStyle;
    private static GUIStyle CenteredLabelStyle;
    private static GUIStyle RightLabelStyle;
    private static GUIStyle BoldLabelStyle;
    private static GUIStyle ItalicLabelStyle;


    private static Color BlueColor = new Color( 0.29f, 0.69f, 1.0f, 1.0f );
    private static Color GreenColor = new Color( 0.2f, 0.8f, 0.2f, 1.0f );
    private static Color RedColor = new Color( 1.1f, 0.2f, 0.2f, 1.0f );
    private static Color OrangeColor = new Color( 0.8f, 0.8f, 0.2f, 1.0f );

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
