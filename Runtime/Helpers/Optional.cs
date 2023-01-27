using UnityEngine;

namespace FishingCactus
{
    [System.Serializable]
    public struct Optional<T>
    {
        // -- FIELDS

        [SerializeField] private bool _Enabled;
        [SerializeField] private T _Value;

        // -- PROPERTIES

        public bool Enabled => _Enabled;
        public T Value => _Value;

        // -- CONSTRUCTORS

        public Optional(
            bool enabled,
            T value
            )
        {
            _Enabled = enabled;
            _Value = value;
        }

        // -- METHODS

        public bool HasValue(
            out T value
            )
        {
            value = _Value;

            return Enabled;
        }
    }
}
