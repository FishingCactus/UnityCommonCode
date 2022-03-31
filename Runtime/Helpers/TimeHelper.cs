using System;
using System.Collections.Generic;

namespace FishingCactus
{
    public static class TimeHelper
    {
        public static int GetMinutesFromSeconds( double seconds )
        {
            return TimeSpan.FromSeconds( seconds ).Minutes;
        }

        public static int GetHoursFromSeconds( double seconds )
        {
            return TimeSpan.FromSeconds( seconds ).Hours;
        }

        public static string GetFormattedTimeFromSeconds( double seconds, bool show_hours = true, bool show_minutes = true, bool show_seconds = true )
        {
            TimeSpan t = TimeSpan.FromSeconds( seconds );
            List<string> tokens = new List<string>();
            List<object> parameters = new List<object>();

            int format_index = 0;

            if ( show_hours )
            {
                tokens.Add( "{" + format_index + ":D2}" );
                parameters.Add( t.Hours );
                format_index++;
            }
            if ( show_minutes )
            {
                tokens.Add( "{" + format_index + ":D2}" );
                parameters.Add( t.Minutes );
                format_index++;
            }
            if ( show_seconds )
            {
                tokens.Add( "{" + format_index + ":D2}" );
                parameters.Add( t.Seconds );
                format_index++;
            }

            var format_string = string.Join( ":", tokens.ToArray() );

            return string.Format( format_string, parameters.ToArray() );
        }
    }
}