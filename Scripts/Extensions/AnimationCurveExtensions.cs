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
    }
}
