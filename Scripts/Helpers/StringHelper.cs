namespace FishingCactus
{
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

        public static string SafeFormat( string format, params object[] args )
        {
            if ( !string.IsNullOrEmpty( format ) )
            {
                return string.Format( format, args );
            }

            string result = string.Empty;
            bool first_arg = true;

            foreach( var arg in args )
            {
                if ( !first_arg )
                {
                    result += " - ";
                }

                if ( arg != null )
                {
                    result += arg.ToString();
                }

                first_arg = false;
            }

            return result;
        }
    }
}