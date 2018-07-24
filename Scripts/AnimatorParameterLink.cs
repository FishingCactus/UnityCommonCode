using UnityEngine;
using UnityEditor.Animations;

namespace FishingCactus
{
    [System.Serializable]
    public abstract class AnimatorParameterLink
    {
        // -- PUBLIC

#if UNITY_EDITOR
        public AnimatorController LinkedAnimatorController = null;
        public string TypeName;
#endif

        public string ParameterName { get; }

        // -- PRIVATE

        [SerializeField]
        private string _ParameterName;
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
