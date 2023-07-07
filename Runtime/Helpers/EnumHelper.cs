using System;
using System.Collections.Generic;
using System.Linq;

namespace FishingCactus
{
    public class EnumHelper
    {
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

            while( flags_as_long != 0 )
            {
                flags_as_long &= flags_as_long - 1;

                ++count;
            }

            return count;
        }
    }
}
