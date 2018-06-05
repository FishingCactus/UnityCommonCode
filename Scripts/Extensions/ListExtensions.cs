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

            int index_to_swap;
            T backup_value;

            for( int index = 0; index < list.Count - 1; index++ )
            {
                index_to_swap = Random.Range( index, list.Count );

                backup_value = list[index];
                list[index] = list[index_to_swap];
                list[index_to_swap] = backup_value;
            }
        }
    }
}
