﻿using UnityEngine;

namespace FishingCactus
{
    public delegate void Task();

    public static class MonoBehaviourExtensions
    {
        public static void SetParent( this MonoBehaviour monobehaviour, Transform parent, bool reset_local_position = true, bool reset_local_rotation = true )
        {
            monobehaviour.transform.SetParent( parent, reset_local_position, reset_local_rotation );
        }

        public static void Invoke( this MonoBehaviour monobehaviour, Task task, float time )
        {
            monobehaviour.Invoke( task.Method.Name, time );
        }

        public static void InvokeRepeating( this MonoBehaviour monobehaviour, Task task, float time, float repeat_rate )
        {
            monobehaviour.InvokeRepeating( task.Method.Name, time, repeat_rate );
        }
    }
}