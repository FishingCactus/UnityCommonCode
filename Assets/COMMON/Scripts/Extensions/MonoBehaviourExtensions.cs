using UnityEngine;

namespace FishingCactus
{
    public static class MonoBehaviourExtensions
    {
        public static void SetParent( this MonoBehaviour monobehaviour, Transform parent, bool reset_local_position = true, bool reset_local_rotation = true )
        {
            monobehaviour.transform.SetParent( parent, reset_local_position, reset_local_rotation );
        }
    }
}