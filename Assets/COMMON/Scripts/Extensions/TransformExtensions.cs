using UnityEngine;

namespace FishingCactus
{
    public static class TransformExtensions
    {
        public static void SetParent( this Transform transform, Transform parent, bool reset_local_position = true, bool reset_local_rotation = true )
        {
            transform.parent = parent;

            if ( reset_local_position )
            {
                transform.localPosition = Vector3.zero;
            }

            if ( reset_local_rotation )
            {
                transform.localRotation = Quaternion.identity;
            }
        }

        public static void SetX( this Transform transform, float x )
        {
            Vector3 newPosition = new Vector3( x, transform.position.y, transform.position.z );

            transform.position = newPosition;
        }

        public static void SetY( this Transform transform, float y )
        {
            Vector3 newPosition = new Vector3( transform.position.x, y, transform.position.z );

            transform.position = newPosition;
        }

        public static void SetZ( this Transform transform, float z )
        {
            Vector3 newPosition = new Vector3( transform.position.x, transform.position.y, z );

            transform.position = newPosition;
        }

        public static void SetLocalX( this Transform transform, float x )
        {
            Vector3 newPosition = new Vector3( x, transform.localPosition.y, transform.localPosition.z );

            transform.localPosition = newPosition;
        }

        public static void SetLocalY( this Transform transform, float y )
        {
            Vector3 newPosition = new Vector3( transform.localPosition.x, y, transform.localPosition.z );

            transform.localPosition = newPosition;
        }

        public static void SetLocalZ( this Transform transform, float z )
        {
            Vector3 newPosition = new Vector3( transform.localPosition.x, transform.localPosition.y, z );
            transform.localPosition = newPosition;
        }

        public static void CopyTransform( this Transform transform, Transform source )
        {
            transform.position = source.position;
            transform.rotation = source.rotation;
        }

        public static void CopyLocalTransform( this Transform transform, Transform source )
        {
            transform.localPosition = source.localPosition;
            transform.localRotation = source.localRotation;
        }
    }
}