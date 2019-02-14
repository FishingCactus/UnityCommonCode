using UnityEngine;
using System.Collections.Generic;

namespace FishingCactus
{
    public static class ListExtensions
    {
        // -- PUBLIC

        public static void Shuffle<T>(
            this List<T> list
            )
        {
            //  for i from 0 to n−2 do
            //      j ← random integer such that i ≤ j < n
            //      exchange a[i] and a[j]

            for( int index = 0; index < list.Count - 1; index++ )
            {
                int index_to_swap = Random.Range( index, list.Count );
                T backup_value = list[index];

                list[index] = list[index_to_swap];
                list[index_to_swap] = backup_value;
            }
        }

        public static void MoveDown<T>(
            this List<T> list,
            int index
            )
        {
            if( index > 0 
                && list.Count > 1
                )
            {
                T first_item = list[index - 1];
                list.RemoveAt( index - 1 );
                list.Insert( index, first_item );
            }
        }
    }
}
