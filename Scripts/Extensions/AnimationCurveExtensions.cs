using UnityEngine;

namespace FishingCactus
{
    public static class AnimationCurveExtensions
    {
        // -- PUBLIC

        public static float GetDuration(
            this AnimationCurve animation_curve
            )
        {
            return animation_curve.GetEndTime() - animation_curve.GetStartTime();
        }

        public static float GetStartTime(
            this AnimationCurve animation_curve
            )
        {
            return animation_curve.keys[0].time;
        }

        public static float GetStartValue(
            this AnimationCurve animation_curve
            )
        {
            return animation_curve.keys[0].value;
        }

        public static float GetEndTime(
            this AnimationCurve animation_curve
            )
        {
            return animation_curve.keys[ animation_curve.length - 1 ].time;
        }

        public static float GetEndValue(
            this AnimationCurve animation_curve
            )
        {
            return animation_curve.keys[animation_curve.length - 1].value;
        }

        public static float GetMaximumValueKeyTime(
            this AnimationCurve animation_curve
            )
        {
            if( animation_curve.length == 0 )
            {
                return 0.0f;
            }

            int highest_key_index = 0;

            for( int key_index = 0; key_index < animation_curve.length; key_index++ )
            {
                if( animation_curve.keys[key_index].value > animation_curve.keys[highest_key_index].value )
                {
                    highest_key_index = key_index;
                }
            }

            return animation_curve.keys[highest_key_index].time;
        }
    }
}
