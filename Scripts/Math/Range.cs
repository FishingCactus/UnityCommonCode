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

        // -- METHODS

        public abstract T GetRandomValue();
        public abstract bool Contains( T value_to_check );
    }

    [System.Serializable]
    public class IntegerRange : Range< int >
    {
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
}
