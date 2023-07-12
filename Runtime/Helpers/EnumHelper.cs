using System;
using System.Collections.Generic;
using System.Linq;

namespace FishingCactus
{
    public class EnumHelper
    {
        // -- FIELDS

        private static int int_all_flags = ~0;
        private static long long_all_flags = ~0;

        // -- METHODS

        public static T ParseEnum<T>( string value ) where T : Enum
        {
            return ( T )Enum.Parse( typeof( T ), value, true );
        }

        public static IEnumerable<string> GetValueTable<T>() where T : Enum
        {
            return Enum.GetValues( typeof( T ) ).Cast<T>().Select( value => value.ToString() );
        }

        public static T GetRandom<T>() where T : Enum
        {
            Array array = Enum.GetValues( typeof( T ) );
            return ( T )array.GetValue( UnityEngine.Random.Range( 0, array.Length ) );
        }

        // Return a random enum value between min_inclusive and max_inclusive
        public static T GetRandom<T>(
            T min_inclusive,
            T max_inclusive
            ) where T : Enum
        {
            if( min_inclusive.CompareTo( max_inclusive ) > 0 )
            {
                throw new ArgumentException( "min_inclusive must be less than or equal to max_inclusive" );
            }

            Array array = Enum.GetValues( typeof( T ) );
            Array.Sort( array );

            int min_index = Array.IndexOf( array, min_inclusive );
            int max_index = Array.IndexOf( array, max_inclusive );

            if( min_index == -1 || max_index == -1 )
            {
                throw new ArgumentException( "min_inclusive or max_inclusive are not valid enum values" );
            }

            return ( T )array.GetValue( UnityEngine.Random.Range( min_index, max_index + 1 ) );
        }

        public static uint GetToggledFlagsCount<T>(
            T flags
            ) where T : Enum
        {
            long flags_as_long = Convert.ToInt64( flags );
            uint count = 0;

            if( flags_as_long == int_all_flags
                || flags_as_long == long_all_flags
                )
            {
                count = T.GetValues( typeof( T ) ).Length;
            }

            while( flags_as_long != 0 )
            {
                count += flags_as_long & 1;
                flags_as_long >>= 1;
            }

            return count;
        }

        public static T Clamp<T>(
            T value,
            T min,
            T max
            ) where T : Enum
        {
            T clamped;

            if( value.CompareTo( min ) < 0 )
            {
                clamped = min;
            }
            else if( value.CompareTo( max ) > 0 )
            {
                clamped = max;
            }
            else
            {
                clamped = value;
            }

            return clamped;
        }
    }
}
