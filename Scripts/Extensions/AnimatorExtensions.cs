using UnityEngine;

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
    }
}
