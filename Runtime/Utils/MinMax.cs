using System;
using UnityEngine;

[Serializable]
public class MinMaxFloats
{
    // -- FIELDS

    [SerializeField] private float _min;
    [SerializeField] private float _max;

    // -- PROPERTIES

    public float Min => _min;
    public float Max => _max;
    public float Random => UnityEngine.Random.Range( _min, _max );

    // -- CONSTRUCTORS

    public MinMaxFloats(
        float min,
        float max
        )
    {
        _min = min;
        _max = max;
    }
}

[Serializable]
public class MinMaxInts
{
    // -- FIELDS

    [SerializeField] private int _min;
    [SerializeField] private int _max;

    // -- PROPERTIES

    public int Min => _min;
    public int Max => _max;
    public int Random => UnityEngine.Random.Range( _min, _max );

    // -- CONSTRUCTORS

    public MinMaxInts(
        int min,
        int max
        )
    {
        _min = min;
        _max = max;
    }
}
