using UnityEngine;
using System.Collections.Generic;

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

        public static void SetTriggerParameter(
            this Animator animator,
            string cached_parameter_name
            )
        {
            int parameter_id = GetCachedAnimatorParameterId( cached_parameter_name );

            Debug.Assert( animator.HasParameter( parameter_id ), string.Format( "Animator '{0}' has no parameters '{0}'", animator.name, cached_parameter_name ) );

            animator.SetTrigger( parameter_id );
        }

        public static bool GetBooleanParameter(
            this Animator animator,
            string cached_parameter_name
            )
        {
            int parameter_id = GetCachedAnimatorParameterId( cached_parameter_name );

            Debug.Assert( animator.HasParameter( parameter_id ), string.Format( "Animator '{0}' has no parameters '{0}'", animator.name, cached_parameter_name ) );

            return animator.GetBool( parameter_id );
        }

        public static void SetBooleanParameter(
            this Animator animator,
            string cached_parameter_name,
            bool it_is_true
            )
        {
            int parameter_id = GetCachedAnimatorParameterId( cached_parameter_name );

            Debug.Assert( animator.HasParameter( parameter_id ), string.Format( "Animator '{0}' has no parameters '{0}'", animator.name, cached_parameter_name ) );

            animator.SetBool( parameter_id, it_is_true );
        }

        public static void SetFloatParameter(
            this Animator animator,
            string cached_parameter_name,
            float value
            )
        {
            int parameter_id = GetCachedAnimatorParameterId( cached_parameter_name );

            Debug.Assert( animator.HasParameter( parameter_id ), string.Format( "Animator '{0}' has no parameters '{0}'", animator.name, cached_parameter_name ) );

            animator.SetFloat( parameter_id, value );
        }

        public static void SetIntegerParameter(
            this Animator animator,
            string cached_parameter_name,
            int value
            )
        {
            int parameter_id = GetCachedAnimatorParameterId( cached_parameter_name );

            Debug.Assert( animator.HasParameter( parameter_id ), string.Format( "Animator '{0}' has no parameters '{0}'", animator.name, cached_parameter_name ) );

            animator.SetInteger( parameter_id, value );
        }

        public static void ToggleBooleanParameter(
            this Animator animator,
            string cached_parameter_name
            )
        {
            int parameter_id = GetCachedAnimatorParameterId( cached_parameter_name );

            Debug.Assert( animator.HasParameter( parameter_id ), string.Format( "Animator '{0}' has no parameters '{0}'", animator.name, cached_parameter_name ) );

            animator.SetBool( parameter_id, !animator.GetBool( parameter_id ) );
        }

        // -- PRIVATE

        private static Dictionary<string, int> ParameterNameIdMap = new Dictionary<string, int>();

        private static int GetCachedAnimatorParameterId(
            string parameter_name
            )
        {
            int parameter_value;

            if( !ParameterNameIdMap.TryGetValue( parameter_name, out parameter_value ) )
            {
                parameter_value = Animator.StringToHash( parameter_name );
                ParameterNameIdMap.Add( parameter_name, parameter_value );
            }

            return parameter_value;
        }
    }
}
