using System;
using System.Collections.Generic;
using System.Linq;

namespace FishingCactus
{
    public class EnumHelper
    {
        public static T ParseEnum<T>( string value )
        {
            return ( T )Enum.Parse( typeof( T ), value, true );
        }

        public static IEnumerable< string > GetValueTable< T >()
        {
            return Enum.GetValues( typeof( T ) ).Cast<T>().Select( value => value.ToString() );
        }

        public static T GetRandom<T>()
        {
            Array array = Enum.GetValues( typeof( T ) );
            return (T)array.GetValue( UnityEngine.Random.Range( 0, array.Length ) );
        }
    }
}