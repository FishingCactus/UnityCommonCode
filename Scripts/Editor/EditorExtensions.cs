using UnityEngine;
using UnityEditor;

namespace FishingCactus
{
    public static class EditorExtensions
    {
        public static void DrawUILine(
            this Editor editor_to_update,
            Color color,
            int thickness = 2,
            int padding = 10
            )
        {
            Rect ligne_rectangle = EditorGUILayout.GetControlRect( GUILayout.Height( padding + thickness ) );

            ligne_rectangle.height = thickness;
            ligne_rectangle.y += padding / 2.0f;
            ligne_rectangle.width -= 8.0f;

            EditorGUI.DrawRect( ligne_rectangle, color );
        }
    }
}
