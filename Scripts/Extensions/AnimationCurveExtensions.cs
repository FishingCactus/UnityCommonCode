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

            Keyframe highest_value_keyframe = animation_curve.keys[0];

            foreach( Keyframe keyframe_to_check in animation_curve.keys )
            {
                if( keyframe_to_check.value > highest_value_keyframe.value )
                {
                    highest_value_keyframe = keyframe_to_check;
                }
            }

            return highest_value_keyframe.time;
        }
    }
}
