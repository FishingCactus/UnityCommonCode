using UnityEngine;

namespace FishingCactus
{
    public static class GameObjectExtensions
    {
        public static void SetParent( this GameObject game_object, Transform parent, bool reset_local_position = true, bool reset_local_rotation = true, bool reset_local_scale = false )
        {
            game_object.transform.SetParent( parent, reset_local_position, reset_local_rotation, reset_local_scale );
        }

        public static T GetSafeComponent<T>( this GameObject obj )
            where T : MonoBehaviour
        {
            T component = obj.GetComponent<T>();

            if ( component == null )
            {
                Debug.LogError( "Expected to find component of type " + typeof( T ) + " but found none", obj );
            }

            return component;
        }
    }
}