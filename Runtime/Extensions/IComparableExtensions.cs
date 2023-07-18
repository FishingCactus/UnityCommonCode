using System;

namespace FishingCactus
{
    public static class IComparableExtensions
    {
        public static T Clamp<T>(
            this T value,
            T min,
            T max
            ) where T : IComparable
        {
            T clamped_value;

            if( value.CompareTo( min ) < 0 )
            {
                clamped_value = min;
            }
            else if( value.CompareTo( max ) > 0 )
            {
                clamped_value = max;
            }
            else
            {
                clamped_value = value;
            }

            return clamped_value;
        }
    }
}
