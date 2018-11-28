using UnityEngine;
using System.Collections.Generic;

namespace FishingCactus
{
    [System.Serializable]
    public abstract class AnimatorParameterLink
    {
        // -- PUBLIC

#if UNITY_EDITOR
        public static Dictionary<string, AnimatorControllerParameterType> TypeTable = new Dictionary<string, AnimatorControllerParameterType>
        {
            { "BooleanAnimatorParameterLink", AnimatorControllerParameterType.Bool },
            { "IntegerAnimatorParameterLink", AnimatorControllerParameterType.Int},
            { "FloatAnimatorParameterLink", AnimatorControllerParameterType.Float },
            { "TriggerAnimatorParameterLink", AnimatorControllerParameterType.Trigger}
        };
#endif

        public string ParameterName { get { return _ParameterName; } }

        // -- PRIVATE

        [SerializeField]
        private string _ParameterName;

        #pragma warning disable 0414
        [SerializeField]
        private RuntimeAnimatorController LinkedAnimatorController = null;
        #pragma warning restore 0414
    }

    [System.Serializable]
    public class BooleanAnimatorParameterLink : AnimatorParameterLink
    {
    }

    [System.Serializable]
    public class IntegerAnimatorParameterLink : AnimatorParameterLink
    {
    }

    [System.Serializable]
    public class FloatAnimatorParameterLink : AnimatorParameterLink
    {
    }

    [System.Serializable]
    public class TriggerAnimatorParameterLink : AnimatorParameterLink
    {
    }
}
