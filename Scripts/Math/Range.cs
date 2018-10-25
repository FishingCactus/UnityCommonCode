using UnityEngine;

namespace FishingCactus
{
    [System.Serializable]
    public abstract class Range< T >
    {
        // -- PUBLIC

        public T MinimumValue { get; private set; }
        public T MaximumValue { get; private set; }

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

        public override int GetRandomValue()
        {
            return Random.Range( MinimumValue, MaximumValue );
        }
    }

    [System.Serializable]
    public class FloatRange : Range<float>
    {
        // -- PUBLIC

        public override float GetRandomValue()
        {
            return Random.Range( MinimumValue, MaximumValue );
        }
    }
}
