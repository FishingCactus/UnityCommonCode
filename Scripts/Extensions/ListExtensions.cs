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
            list.Sort( ( a, b ) => Random.Range( 0, 2 ) == 0 ? 1 : -1 );
        }
    }
}
