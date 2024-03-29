﻿using UnityEngine;

namespace FishingCactus
{
    public static class Math
    {
        public static Vector3 CatmulRomInterpolation( Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float percentage )
        {
            var a = 0.5f * ( 2.0f * point1 );
            var b = 0.5f * ( point2 - point0 );
            var c = 0.5f * ( 2.0f * point0 - 5.0f * point1 + 4.0f * point2 - point3 );
            var d = 0.5f * ( -point0 + 3.0f * point1 - 3.0f * point2 + point3 );

            var pos = a + ( b * percentage ) + ( c * percentage * percentage ) + ( d * percentage * percentage * percentage );

            return pos;
        }

        public static bool SameSign(
            int first_value,
            int second_value
            )
        {
            return ( first_value > 0 && second_value > 0 )
                || ( first_value < 0 && second_value < 0 )
                || ( first_value == 0 && second_value == 0 );
        }

        public static bool SameSign(
            float first_value,
            float second_value
            )
        {
            return ( first_value > 0.0f && second_value > 0.0f )
                || ( first_value < 0.0f && second_value < 0.0f )
                || ( first_value == 0.0f && second_value == 0.0f );
        }

        /// <summary>
        /// This ensures to always have a positive result:<br/>
        /// MathMod( 8, 3 ) returns 2<br/>
        /// MathMod( -8, 3 ) returns 2<br/>
        /// This can be used to have the same behaviour as Mathf.Repeat() but with integers.
        /// </summary>
        public static int MathMod(
            int dividend,
            int modulus
            )
        {
            return ( dividend % modulus + modulus ) % modulus;
        }
    }
}