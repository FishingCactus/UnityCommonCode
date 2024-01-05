using UnityEngine;

namespace FishingCactus
{
    [System.Serializable]
    public struct Optional<T>
    {
        // -- FIELDS

        [SerializeField] private bool _Enabled;
        [SerializeField] private T _Value;

        // -- OPERATORS

        public static implicit operator Optional<T>( T value ) => new Optional<T>( enabled: true, value );

        // -- PROPERTIES

        public bool Enabled => _Enabled;
        public T Value => _Value;

        // -- CONSTRUCTORS

        public Optional(
            bool enabled
            )
        {
            _Enabled = enabled;
            _Value = default;
        }

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

        public readonly T TryGetValueOrDefault(
            T default_value
            )
        {
            if( Enabled )
            {
                return _Value;
            }

            return default_value;
        }
    }
}
