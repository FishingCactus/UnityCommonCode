using System;

public static class StringExtensions
{
    public static int CountOccurence( this string main, string pattern )
    {
        int count = 0;
        int previous_index = 0;

        if ( !string.IsNullOrEmpty( pattern ) )
        {
            while ( ( previous_index = main.IndexOf( pattern, previous_index ) ) != -1 )
            {
                ++previous_index;
                ++count;
            }
        }

        return count;
    }
}

