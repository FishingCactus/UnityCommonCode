using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SerializableDictionary
{
}

[Serializable]
public class SerializableDictionary<TKey, TValue> :
    SerializableDictionary,
    IDictionary<TKey, TValue>,
    ISerializationCallbackReceiver
{
    // -- TYPES

    [Serializable]
    public struct SerializableKeyValuePair
    {
        public TKey Key;
        public TValue Value;

        public SerializableKeyValuePair( TKey key, TValue value )
        {
            Key = key;
            Value = value;
        }

        public void SetValue( TValue value )
        {
            Value = value;
        }
    }

    // -- FIELDS

    [SerializeField]
    private List<SerializableKeyValuePair> KeyValueList = new List<SerializableKeyValuePair>();

    private Lazy<Dictionary<TKey, uint>> _KeyPositions = null;

    // -- PROPERTIES

    private Dictionary<TKey, uint> KeyPositions => _KeyPositions.Value;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public ICollection<TKey> Keys => KeyValueList.Select( tuple => tuple.Key ).ToArray();
    public ICollection<TValue> Values => KeyValueList.Select( tuple => tuple.Value ).ToArray();
    public int Count => KeyValueList.Count;
    public bool IsReadOnly => false;

    public TValue this[ TKey key ]
    {
        get
        {
            return KeyValueList[ ( int )KeyPositions[ key ] ].Value;
        }
        set
        {
            if( KeyPositions.TryGetValue( key, out uint index ) )
            {
                var keyValuePair = KeyValueList[ ( int )index ];
                keyValuePair.SetValue( value );
                KeyValueList[ ( int )index ] = keyValuePair;
            }
            else
            {
                KeyPositions[ key ] = ( uint )KeyValueList.Count;

                KeyValueList.Add( new SerializableKeyValuePair( key, value ) );
            }
        }
    }

    // -- METHODS

    public SerializableDictionary()
    {
        _KeyPositions = new Lazy<Dictionary<TKey, uint>>( MakeKeyPositions );
    }

    public SerializableDictionary( IDictionary<TKey, TValue> dictionary )
    {
        _KeyPositions = new Lazy<Dictionary<TKey, uint>>( MakeKeyPositions );

        if( dictionary == null )
        {
            throw new ArgumentException( "The passed dictionary is null." );
        }

        foreach( KeyValuePair<TKey, TValue> pair in dictionary )
        {
            Add( pair.Key, pair.Value );
        }
    }

    private Dictionary<TKey, uint> MakeKeyPositions()
    {
        int entry_count = KeyValueList.Count;

        Dictionary<TKey, uint> result = new Dictionary<TKey, uint>( entry_count );

        for( int entry_index = 0; entry_index < entry_count; ++entry_index )
        {
            result[ KeyValueList[ entry_index ].Key ] = ( uint )entry_index;
        }

        return result;
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        // After deserialization, the key positions might be changed
        _KeyPositions = new Lazy<Dictionary<TKey, uint>>( MakeKeyPositions );
    }

    #region IDictionary

    public void Add( TKey key, TValue value )
    {
        if( KeyPositions.ContainsKey( key ) )
        {
            throw new ArgumentException( "An element with the same key already exists in the dictionary." );
        }
        else
        {
            KeyPositions[ key ] = ( uint )KeyValueList.Count;

            KeyValueList.Add( new SerializableKeyValuePair( key, value ) );
        }
    }

    public bool ContainsKey( TKey key )
    {
        return KeyPositions.ContainsKey( key );
    }

    public bool Remove( TKey key )
    {
        if( KeyPositions.TryGetValue( key, out uint index ) )
        {
            Dictionary<TKey, uint> key_positions = KeyPositions;

            key_positions.Remove( key );

            KeyValueList.RemoveAt( ( int )index );

            int entry_count = KeyValueList.Count;

            for( uint entry_index = index; entry_index < entry_count; entry_index++ )
            {
                key_positions[ KeyValueList[ ( int )entry_index ].Key ] = entry_index;
            }

            return true;
        }

        return false;
    }

    public bool TryGetValue(
        TKey key,
        out TValue value
        )
    {
        if( KeyPositions.TryGetValue( key, out uint index ) )
        {
            value = KeyValueList[ ( int )index ].Value;

            return true;
        }

        value = default;

        return false;
    }
    #endregion

    #region ICollection

    public void Add( KeyValuePair<TKey, TValue> key_value_pair )
    {
        Add( key_value_pair.Key, key_value_pair.Value );
    }

    public bool Contains(
        KeyValuePair<TKey, TValue> key_value_pair
        )
    {
        return KeyPositions.ContainsKey( key_value_pair.Key );
    }

    public bool Remove(
        KeyValuePair<TKey, TValue> key_value_pair
        )
    {
        return Remove( key_value_pair.Key );
    }

    public void Clear()
    {
        KeyValueList.Clear();
        KeyPositions.Clear();
    }

    public void CopyTo(
        KeyValuePair<TKey,
        TValue>[] array,
        int arrayIndex
        )
    {
        int numKeys = KeyValueList.Count;

        if( array.Length - arrayIndex < numKeys )
        {
            throw new ArgumentException( "arrayIndex" );
        }

        for( int i = 0; i < numKeys; ++i, ++arrayIndex )
        {
            SerializableKeyValuePair entry = KeyValueList[ i ];

            array[ arrayIndex ] = new KeyValuePair<TKey, TValue>( entry.Key, entry.Value );
        }
    }
    #endregion

    #region IEnumerable
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return KeyValueList.Select( ToKeyValuePair ).GetEnumerator();

        KeyValuePair<TKey, TValue> ToKeyValuePair( SerializableKeyValuePair key_value_pair )
        {
            return new KeyValuePair<TKey, TValue>( key_value_pair.Key, key_value_pair.Value );
        }
    }
    #endregion
}