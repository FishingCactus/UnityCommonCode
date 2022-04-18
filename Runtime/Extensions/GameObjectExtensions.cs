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
                Debug.LogError( string.Format( "Expected to find component of type '{0}' but found none in {1}.", typeof(T), game_object.name ), game_object );
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
                Debug.LogError( string.Format( "Expected to find component of type '{0}' but found none in {1}.", typeof( T ), game_object.name ), game_object );
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
                Debug.LogError( string.Format( "Expected to find component of type '{0}' but found none in {1}.", typeof( T ), game_object.name ), game_object );
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
                Debug.LogError( string.Format( "Expected to find component of type '{0}' but found none in {1}.", typeof( T ), game_object.name ), game_object );
            }

            return wanted_component;
        }

        public static T GetOrAddComponent<T>(
            this GameObject game_object
            ) where T : Component
        {
            var component = game_object.GetComponent<T>();
            if (component == null)
            {
                return game_object.AddComponent<T>();
            }

            return component;
        }

        public static Component GetOrAddComponent(
            this GameObject game_object,
            System.Type component_type
            )
        {
            return game_object.GetComponent( component_type ) ?? game_object.AddComponent( component_type );
        }

        public static void SafeDestroy(
            this GameObject game_object_instance
            )
        {
            if( game_object_instance )
            {
                if( Application.isPlaying )
                {
                    Object.Destroy( game_object_instance );
                }
                else
                {
                    Object.DestroyImmediate( game_object_instance );
                }

                game_object_instance = null;
            }
        }
    }
}