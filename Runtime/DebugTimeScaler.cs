using UnityEngine;

public class DebugTimeScaler : MonoBehaviour
{
    // -- PUBLIC

    [Range(0.01f, 10.0f)]
    public float TimeScale = 1.0f;

    // -- PRIVATE

    private float InitialFixedDeltaTime;

    // -- UNITY

    void Awake()
    {
        InitialFixedDeltaTime = Time.fixedDeltaTime;
    }

    void LateUpdate()
    {
        Time.timeScale = TimeScale;
        Time.fixedDeltaTime = InitialFixedDeltaTime * TimeScale;
    }
}
