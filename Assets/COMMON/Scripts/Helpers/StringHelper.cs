public static class StringHelper
{
    public static string Capitalize( string str )
    {
        if ( str == null )
            return null;

        if ( str.Length > 1 )
            return char.ToUpper( str[ 0 ] ) + str.Substring( 1 );

        return str.ToUpper();
    }
}