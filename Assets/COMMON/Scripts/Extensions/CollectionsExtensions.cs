﻿using UnityEngine;
using System.Collections.Generic;

namespace FishingCactus
{
    public static class CollectionsExtensions
    {
        public static T GetItemByIndexClamped<T>( this List<T> list, int index )
        {
            return list[ Mathf.Clamp( index, 0, list.Count - 1 ) ];
        }

        public static T GetRandomItem<T>( this List<T> list )
        {
            return list[ Random.Range( 0, list.Count ) ];
        }

        public static T GetRandomItem<T>( this List<T> list, out int index )
        {
            index = Random.Range( 0, list.Count );
            return list[ index ];
        }
    }
}