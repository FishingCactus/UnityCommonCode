using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
    using UnityEditor.Animations;
#endif

namespace FishingCactus
{
    public static class AnimatorExtensions
    {
        // -- PUBLIC

        public static bool HasParameter(
            this Animator animator,
            string parameter_name
            )
        {
            foreach ( AnimatorControllerParameter param in animator.parameters )
            {
                if ( param.name == parameter_name )
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasParameter(
            this Animator animator,
            int  parameter_hash_id
            )
        {
            foreach ( AnimatorControllerParameter param in animator.parameters )
            {
                if ( param.nameHash == parameter_hash_id )
                {
                    return true;
                }
            }

            return false;
        }

        public static void ResetAllTriggerParameters(
            this Animator animator
            )
        {
            foreach( AnimatorControllerParameter parameter in animator.parameters )
            {
                if( parameter.type == AnimatorControllerParameterType.Trigger )
                {
                    animator.ResetTriggerParameter( parameter.name );
                }
            }
        }

        public static void SetTriggerParameter(
            this Animator animator,
            string cached_parameter_name
            )
        {
            animator.SetTrigger( animator.GetCachedAnimatorParameterId( cached_parameter_name ) );
        }

        public static void ResetTriggerParameter(
            this Animator animator,
            string cached_parameter_name
            )
        {
            animator.ResetTrigger( animator.GetCachedAnimatorParameterId( cached_parameter_name ) );
        }

        public static bool GetBooleanParameter(
            this Animator animator,
            string cached_parameter_name
            )
        {
            return animator.GetBool( animator.GetCachedAnimatorParameterId( cached_parameter_name ) );
        }

        public static void SetBooleanParameter(
            this Animator animator,
            string cached_parameter_name,
            bool it_is_true
            )
        {
            animator.SetBool(
                animator.GetCachedAnimatorParameterId( cached_parameter_name ),
                it_is_true
                );
        }

        public static float GetFloatParameter(
            this Animator animator,
            string cached_parameter_name
            )
        {
            return animator.GetFloat( animator.GetCachedAnimatorParameterId( cached_parameter_name ) );
        }

        public static void SetFloatParameter(
            this Animator animator,
            string cached_parameter_name,
            float value
            )
        {
            animator.SetFloat(
                animator.GetCachedAnimatorParameterId( cached_parameter_name ),
                value
                );
        }

        public static float GetIntegerParameter(
            this Animator animator,
            string cached_parameter_name
            )
        {
            return animator.GetInteger( animator.GetCachedAnimatorParameterId( cached_parameter_name ) );
        }

        public static void SetIntegerParameter(
            this Animator animator,
            string cached_parameter_name,
            int value
            )
        {
            animator.SetInteger(
                animator.GetCachedAnimatorParameterId( cached_parameter_name ),
                value
                );
        }

        public static void ToggleBooleanParameter(
            this Animator animator,
            string cached_parameter_name
            )
        {
            int parameter_id = animator.GetCachedAnimatorParameterId( cached_parameter_name );

            animator.SetBool(
                parameter_id,
                !animator.GetBool( parameter_id )
                );
        }

#if UNITY_EDITOR
        public static void ClearAllTransitions(
            this Animator animator
            )
        {
            AnimatorController animation_controller = animator.runtimeAnimatorController as AnimatorController;

            foreach( AnimatorControllerLayer layer_to_clear in animation_controller.layers )
            {
                foreach( ChildAnimatorState state in layer_to_clear.stateMachine.states )
                {
                    foreach( AnimatorStateTransition transition in state.state.transitions )
                    {
                        transition.hasExitTime = false;
                        transition.exitTime = 1.0f;

                        transition.hasFixedDuration = false;
                        transition.duration = 0.0f;
                    }
                }
            }
        }
#endif

        // -- PRIVATE

        private static Dictionary<string, int> ParameterNameIdMap = new Dictionary<string, int>();

        private static int GetCachedAnimatorParameterId(
            this Animator animator,
            string parameter_name
            )
        {
            int parameter_value;

            if( !ParameterNameIdMap.TryGetValue( parameter_name, out parameter_value ) )
            {
                parameter_value = Animator.StringToHash( parameter_name );
                ParameterNameIdMap.Add( parameter_name, parameter_value );
            }

            Debug.Assert( animator.HasParameter( parameter_value ), $"Animator '{animator.name}' has no parameters '{parameter_name}'" );

            return parameter_value;
        }
    }
}
