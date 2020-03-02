using UnityEngine;

namespace FishingCactus
{
    public class Singleton<_INSTANCE_> : MonoBehaviour
        where _INSTANCE_ : Singleton<_INSTANCE_>
    {
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

        private static _INSTANCE_ instance;

        public virtual void Awake()
        {
            DontDestroyOnLoad( gameObject );

            if ( instance == null )
            {
                instance = this as _INSTANCE_;
            }
            else if(instance != this)
            {
                Debug.LogError( $"{instance.name} is added two times." );
            }
        }
    }
}