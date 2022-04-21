using UnityEngine;

namespace FishingCactus
{
    public class Singleton<_INSTANCE_> : MonoBehaviour
        where _INSTANCE_ : Singleton<_INSTANCE_>
    {
        // -- FIELDS

        private static _INSTANCE_ instance;

        // -- METHODS

        public static _INSTANCE_ Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = FindObjectOfType<_INSTANCE_>();
                }
                return instance;
            }
        }

        public static bool HasInstance
        {
            get{ return instance != null; }
        }

        protected virtual bool IsPersistentSingleton{ get{ return true; } }

        // -- UNITY

        public virtual void Awake()
        {
            if( IsPersistentSingleton )
            {
                DontDestroyOnLoad( gameObject );
            }

            if ( instance == null )
            {
                instance = this as _INSTANCE_;
            }
            else if(instance != this)
            {
                Debug.LogError( $"{instance.name} is added two times.", this );

                Destroy( this );
            }
        }
    }
}