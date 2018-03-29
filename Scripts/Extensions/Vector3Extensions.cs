using UnityEngine;

namespace FishingCactus
{
    public static class Vector3Extensions
    {
// -- PUBLIC

        public static float GetHorizontalMagnitude(
            this Vector3 vector,
            Vector3 other_vector
            )
        {
            Vector3 distance_vector = vector - other_vector;
            distance_vector.y = 0;

            return distance_vector.magnitude;
        }

        public static float GetSquaredHorizontalMagnitude(
            this Vector3 vector,
            Vector3 other_vector
            )
        {
            Vector3 distance_vector = vector - other_vector;
            distance_vector.y = 0;

            return distance_vector.sqrMagnitude;
        }

        public static float GetVerticalMagnitude(
            this Vector3 vector,
            Vector3 other_vector
            )
        {
            return Mathf.Abs( vector.y - other_vector.y );
        }

        public static float GetSquaredVerticalMagnitude(
            this Vector3 vector,
            Vector3 other_vector
            )
        {
            float y_distance = vector.y - other_vector.y;

            return y_distance* y_distance;
        }
    }
}
