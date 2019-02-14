using UnityEngine;

namespace FishingCactus
{
    [System.Serializable]
    public abstract class Range< T >
    {
        // -- PUBLIC

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

        public abstract T GetRandomValue();

        // -- PROTECTED

        [SerializeField]
        protected T _MinimumValue;
        [SerializeField]
        protected T _MaximumValue;
    }

    [System.Serializable]
    public class IntegerRange : Range< int >
    {
        // -- PUBLIC

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
    }

    [System.Serializable]
    public class FloatRange : Range<float>
    {
        // -- PUBLIC

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
    }
}
