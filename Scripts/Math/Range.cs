using UnityEngine;

namespace FishingCactus
{
    [System.Serializable]
    public abstract class Range< T >
    {
        // -- FIELDS

        [SerializeField] protected T _MinimumValue;
        [SerializeField] protected T _MaximumValue;

        // -- PROPERTIES

        public T MinimumValue
        {
            get { return _MinimumValue; }
            protected set { _MinimumValue = value; }
        }

        public T MaximumValue
        {
            get { return _MaximumValue; }
            protected set { _MaximumValue = value; }
        }

        public abstract T Amplitude{  get; }
        public abstract T Center{ get; }

        // -- METHODS

        public abstract T GetRandomValue();
        public abstract bool Contains( T value_to_check );
    }

    [System.Serializable]
    public class IntegerRange : Range< int >
    {
        // -- PROPERTIES

        public override int Amplitude{ get{ return _MaximumValue - _MinimumValue; } }
        public override int Center { get { return ( _MaximumValue + _MinimumValue ) / 2; } }

        // -- METHODS

        public IntegerRange(
            int minimum_value,
            int maximum_value
            )
        {
            MinimumValue = minimum_value;
            MaximumValue = maximum_value;
        }

        public override int GetRandomValue()
        {
            return Random.Range( MinimumValue, MaximumValue );
        }

        public override bool Contains(
            int value_to_check
            )
        {
            return MinimumValue <= value_to_check
                && value_to_check < MaximumValue;
        }
    }

    [System.Serializable]
    public class FloatRange : Range<float>
    {
        // -- PROPERTIES

        public override float Amplitude { get { return _MaximumValue - _MinimumValue; } }
        public override float Center { get { return ( _MaximumValue + _MinimumValue ) * 0.5f; } }

        // -- METHODS

        public FloatRange( 
            float minimum_value,
            float maximum_value
            )
        {
            MinimumValue = minimum_value;
            MaximumValue = maximum_value;
        }

        public override float GetRandomValue()
        {
            return Random.Range( MinimumValue, MaximumValue );
        }

        public override bool Contains(
            float value_to_check
            )
        {
            return MinimumValue <= value_to_check
                && value_to_check <= MaximumValue;
        }
    }

    [System.Serializable]
    public class Vector3Range : Range<Vector3>
    {
        // -- PROPERTIES

        public override Vector3 Amplitude { get { return _MaximumValue - _MinimumValue; } }
        public override Vector3 Center { get { return ( _MaximumValue + _MinimumValue ) * 0.5f; } }

        // -- METHODS

        public Vector3Range( 
            Vector3 minimum_value,
            Vector3 maximum_value
            )
        {
            MinimumValue = minimum_value;
            MaximumValue = maximum_value;
        }

        public override Vector3 GetRandomValue()
        {
            return new Vector3( Random.Range( MinimumValue.x, MaximumValue.x ), Random.Range( MinimumValue.y, MaximumValue.y ), Random.Range( MinimumValue.z, MaximumValue.z ) );
        }

        public override bool Contains(
            Vector3 value_to_check
            )
        {
            return MinimumValue.x <= value_to_check.x
                && value_to_check.x <= MaximumValue.x
                && MinimumValue.y <= value_to_check.y
                && value_to_check.y <= MaximumValue.y
                && MinimumValue.z <= value_to_check.z
                && value_to_check.z <= MaximumValue.z;
        }
    }
}
