using UnityEngine;

public sealed class MaxAttribute : PropertyAttribute
{
    public readonly float Max;

    public MaxAttribute(
        float max
        )
    {
        Max = max;
    }
}
