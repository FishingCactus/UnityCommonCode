using UnityEngine;

namespace FishingCactus
{
    public static class GameObjectExtensions
    {
        public static void SetParent(
            this GameObject game_object,
            Transform parent,
            bool reset_local_position = true,
            bool reset_local_rotation = true,
            bool reset_local_scale = false
            )
        {
            game_object.transform.SetParent( parent, reset_local_position, reset_local_rotation, reset_local_scale );
        }

        public static T GetSafeComponent<T>(
            this GameObject game_object
            ) where T : MonoBehaviour
        {
            T wanted_component = game_object.GetComponent<T>();

            if (wanted_component == null )
            {
                Debug.LogError( string.Format( "Expected to find component of type '{0}' but found none in {1}.", typeof(T), game_object.name ) );
            }

            return wanted_component;
        }

        public static T GetOrAddComponent<T>(
            this GameObject game_object
            ) where T : MonoBehaviour
        {
            return game_object.GetComponent<T>() ?? game_object.AddComponent<T>();
        }
    }
}