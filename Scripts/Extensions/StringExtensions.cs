using System;

public static class StringExtensions
{
    public static int CountOccurence( this string the_string, string find_string )
    {
        int count = 0;
        int find_index;

        for ( int index = 0; index < the_string.Length; index++ )
        {
            for ( find_index = 0; find_index < find_string.Length; find_index++ )
            {
                if ( the_string[ index + find_index ] != find_string[ find_index ] )
                {
                    break;
                }
            }

            if ( find_index == find_string.Length )
            {
                count++;
            }
        }

        return count;
    }
}

