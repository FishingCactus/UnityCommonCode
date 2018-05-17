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

        [System.Obsolete( "This is an obsolete method, please use 'GetCheckedComponent'." )]
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

        public static T GetCheckedComponent<T>(
            this GameObject game_object
            ) where T : MonoBehaviour
        {
            T wanted_component = game_object.GetComponent<T>();

            if( wanted_component == null )
            {
                Debug.LogError( string.Format( "Expected to find component of type '{0}' but found none in {1}.", typeof( T ), game_object.name ) );
            }

            return wanted_component;
        }

        public static T GetCheckedComponentInParent<T>(
            this GameObject game_object
            ) where T : MonoBehaviour
        {
            T wanted_component = game_object.GetComponentInParent<T>();

            if( wanted_component == null )
            {
                Debug.LogError( string.Format( "Expected to find component of type '{0}' but found none in {1}.", typeof( T ), game_object.name ) );
            }

            return wanted_component;
        }

        public static T GetCheckedComponentInChildren<T>(
            this GameObject game_object
            ) where T : MonoBehaviour
        {
            T wanted_component = game_object.GetComponentInChildren<T>();

            if( wanted_component == null )
            {
                Debug.LogError( string.Format( "Expected to find component of type '{0}' but found none in {1}.", typeof( T ), game_object.name ) );
            }

            return wanted_component;
        }

        public static T GetOrAddComponent<T>(
            this GameObject game_object
            ) where T : MonoBehaviour
        {
            return game_object.GetComponent<T>() ?? game_object.AddComponent<T>();
        }

        public static Component GetOrAddComponent(
            this GameObject game_object,
            System.Type component_type
            )
        {
            return game_object.GetComponent( component_type ) ?? game_object.AddComponent( component_type );
        }
    }
}