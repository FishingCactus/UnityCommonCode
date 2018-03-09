using System.Linq;
using UnityEngine;

namespace FishingCactus
{
    public static class TransformExtensions
    {
        public static void SetParent( this Transform transform, Transform parent, bool reset_local_position = true, bool reset_local_rotation = true, bool reset_local_scale = false )
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

            if ( reset_local_scale )
            {
                transform.localScale = Vector3.one;
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

        public static void Reset( this Transform transform )
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        public static void ResetLocal( this Transform transform )
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public static void DestroyChildren( this Transform transform )
        {
            foreach ( Transform child in transform )
            {
                GameObject.Destroy( child.gameObject );
            }
        }

        public static void DestroyImmediateChildren( this Transform transform )
        {
            var list = transform.Cast<Transform>().ToList();

            foreach ( Transform child in list )
            {
                GameObject.DestroyImmediate( child.gameObject );
            }
        }
    }
}