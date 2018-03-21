using UnityEngine;

namespace FishingCactus
{
    public delegate void Task();

    public static class MonoBehaviourExtensions
    {
        public static void SetParent(
            this MonoBehaviour monobehaviour,
            Transform parent,
            bool reset_local_position = true,
            bool reset_local_rotation = true,
            bool reset_local_scale = false
            )
        {
            monobehaviour.transform.SetParent( parent, reset_local_position, reset_local_rotation, reset_local_scale );
        }

        public static void Invoke(
            this MonoBehaviour monobehaviour,
            Task task, float time
            )
        {
            monobehaviour.Invoke( task.Method.Name, time );
        }

        public static void InvokeRepeating(
            this MonoBehaviour monobehaviour,
            Task task,
            float time,
            float repeat_rate
            )
        {
            monobehaviour.InvokeRepeating( task.Method.Name, time, repeat_rate );
        }

        public static T GetOrAddComponent<T>(
            this MonoBehaviour monobehaviour
            ) where T : MonoBehaviour
        {
            return monobehaviour.GetComponent<T>() ?? monobehaviour.gameObject.AddComponent<T>();
        }
    }
}