using System;

namespace FishingCactus
{
    public class EnumHelper
    {
        public static T ParseEnum<T>( string value )
        {
            return ( T )Enum.Parse( typeof( T ), value, true );
        }
    }
}