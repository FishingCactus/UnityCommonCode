using UnityEngine;

namespace FishingCactus
{
    public class GyroscopeHelper
    {
        public Vector3 ManualRotation = Vector3.zero;

        public void Enable()
        {
            ManualRotation = Vector3.zero;

            canUseGyroscope = SystemInfo.supportsGyroscope;

#if UNITY_EDITOR
            canUseGyroscope = false;
#endif

            Input.gyro.enabled = true;

            UpdateCalibration( true );

            RecalculateReferenceRotation();
        }

        public void Disable()
        {
            canUseGyroscope = false;
            Input.gyro.enabled = false;
        }

        public Quaternion GetCorrectedGyroRotation()
        {
            Quaternion attitude;

            if ( canUseGyroscope )
            {
                attitude = Input.gyro.attitude;
            }
            else
            {
                attitude = baseIdentity * Quaternion.Euler( ManualRotation );
            }

            return ( ConvertRotation( referenceRotation * attitude ) );
        }

        private void UpdateCalibration( bool only_horizontal )
        {
            if ( !canUseGyroscope )
            {
                calibration = Quaternion.identity;
                return;
            }

            if ( only_horizontal )
            {
                var forward = ( Input.gyro.attitude ) * ( -Vector3.forward );
                forward.z = 0;

                if ( forward == Vector3.zero )
                {
                    calibration = Quaternion.identity;
                }
                else
                {
                    calibration = ( Quaternion.FromToRotation( baseOrientationRotationFix * Vector3.up, forward ) );
                }
            }
            else
            {
                calibration = Input.gyro.attitude;
            }
        }

        private Quaternion ConvertRotation( Quaternion q )
        {
            return new Quaternion( q.x, q.y, -q.z, -q.w );
        }

        private void RecalculateReferenceRotation()
        {
            referenceRotation = Quaternion.Inverse( baseIdentity ) * Quaternion.Inverse( calibration );
        }

        private readonly Quaternion baseIdentity = Quaternion.Euler( 90, 0, 0 );
        private Quaternion calibration = Quaternion.identity;
        private Quaternion baseOrientationRotationFix = Quaternion.identity;
        private Quaternion referenceRotation = Quaternion.identity;
        private bool canUseGyroscope = false;
    }
}