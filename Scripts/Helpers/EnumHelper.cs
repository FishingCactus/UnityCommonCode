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
            return Enum.GetValues( typeof( CardType ) ).Cast<T>().Select( value => value.ToString() );
        }
    }
}