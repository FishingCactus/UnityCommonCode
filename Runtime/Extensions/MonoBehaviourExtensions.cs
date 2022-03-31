using UnityEngine;

namespace FishingCactus
{
    public delegate void Task();

    public static class MonoBehaviourExtensions
    {
        public static void SetParent(
            this MonoBehaviour mono_behaviour,
            Transform parent,
            bool reset_local_position = true,
            bool reset_local_rotation = true,
            bool reset_local_scale = false
            )
        {
            mono_behaviour.transform.SetParent( parent, reset_local_position, reset_local_rotation, reset_local_scale );
        }

        public static void Invoke(
            this MonoBehaviour mono_behaviour,
            Task task,
            float time
            )
        {
            mono_behaviour.Invoke( task.Method.Name, time );
        }

        public static void InvokeRepeating(
            this MonoBehaviour mono_behaviour,
            Task task,
            float time,
            float repeat_rate
            )
        {
            mono_behaviour.InvokeRepeating( task.Method.Name, time, repeat_rate );
        }

        public static T GetOrAddComponent<T>(
            this MonoBehaviour mono_behaviour
            ) where T : MonoBehaviour
        {
            return mono_behaviour.GetComponent<T>() ?? mono_behaviour.gameObject.AddComponent<T>();
        }

        public static Component GetOrAddComponent(
            this MonoBehaviour mono_behaviour,
            System.Type component_type
            )
        {
            return mono_behaviour.GetComponent( component_type ) ?? mono_behaviour.gameObject.AddComponent( component_type );
        }
    }
}