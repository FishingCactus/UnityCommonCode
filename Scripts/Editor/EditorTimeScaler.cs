using UnityEngine;

public class EditorTimeScaler : MonoBehaviour
{
#if UNITY_EDITOR
    // -- PUBLIC

    [Range(0.01f, 2.0f)]
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
#endif
}
