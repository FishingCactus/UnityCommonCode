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

                    if ( instance == null )
                    {
                        GameObject obj = new GameObject();
                        obj.hideFlags = HideFlags.HideAndDontSave;
                        instance = obj.AddComponent<_INSTANCE_>();
                    }
                }
                return instance;
            }
        }

        private static _INSTANCE_ instance;

        public virtual void Awake()
        {
            DontDestroyOnLoad( this.gameObject );

            if ( instance == null )
            {
                instance = this as _INSTANCE_;
            }
            else
            {
                Destroy( gameObject );
            }
        }
    }
}