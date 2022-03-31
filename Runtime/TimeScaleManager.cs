using System;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    public static TimeScaleManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        enabled = false;
    }

    void Update()
    {
        var delta_time = Time.realtimeSinceStartup - timeScaleLerpLastTime;
        timeScaleLerpLastTime = Time.realtimeSinceStartup;
        Time.timeScale = Mathf.MoveTowards( Time.timeScale, targetTimeScale, delta_time * lerpSpeed );

        if ( Time.timeScale == targetTimeScale )
        {
            RunCallback();

            enabled = false;
        }
    }

    public void LerpTimeScale( float target_timescale, float lerp_speed, Action callback )
    {
        if ( lerp_speed <= 0.0f )
        {
            Time.timeScale = target_timescale;
            RunCallback();
            return;
        }

        timeScaleLerpLastTime = Time.realtimeSinceStartup;
        targetTimeScale = target_timescale;
        lerpSpeed = lerp_speed;
        this.callback = callback;

        enabled = true;
    }

    private void RunCallback()
    {
        if ( callback != null )
        {
            callback();
        }
    }

    private float timeScaleLerpLastTime;
    private float targetTimeScale;
    private float lerpSpeed;
    private Action callback;
}
