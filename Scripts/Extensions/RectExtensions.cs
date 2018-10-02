using UnityEngine;

namespace FishingCactus
{
    public static class RectExtensions
    {
        public static Vector2 TopLeft(
            this Rect initial_rectangle
            )
        {
            return new Vector2( initial_rectangle.xMin, initial_rectangle.yMin );
        }

        public static Vector2 BottomRight(
            this Rect initial_rectangle
            )
        {
            return new Vector2( initial_rectangle.xMax, initial_rectangle.yMax );
        }

        public static Rect Scale(
            this Rect initial_rectangle,
            float scale
            )
        {
            return initial_rectangle.Scale( new Vector2( scale, scale ), initial_rectangle.center );
        }

        public static Rect Scale(
            this Rect initial_rectangle,
            Vector2 scale
            )
        {
            return initial_rectangle.Scale( scale, initial_rectangle.center );
        }

        public static Rect Scale(
            this Rect initial_rectangle,
            float scale,
            Vector2 pivotPoint
            )
        {
            return initial_rectangle.Scale( new Vector2( scale, scale ), pivotPoint );
        }

        public static Rect Scale(
            this Rect initial_rectangle,
            Vector2 scale,
            Vector2 pivotPoint
            )
        {
            Rect result = initial_rectangle;

            result.x -= pivotPoint.x;
            result.y -= pivotPoint.y;

            result.xMin *= scale.x;
            result.xMax *= scale.x;
            result.yMin *= scale.y;
            result.yMax *= scale.y;

            result.x += pivotPoint.x;
            result.y += pivotPoint.y;

            return result;
        }
    }
}
