using System;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    public static TimeScaleManager Get { get; private set; }

    private float timeScaleLerpLastTime;
    private float targetTimeScale;
    private float lerpSpeed;
    private Action callback;

    void Awake()
    {
        Get = this;

        enabled = false;
    }

    void Update()
    {
        var delta_time = Time.realtimeSinceStartup - timeScaleLerpLastTime;
        timeScaleLerpLastTime = Time.realtimeSinceStartup;
        Time.timeScale = Mathf.MoveTowards( Time.timeScale, targetTimeScale, delta_time * lerpSpeed );

        if ( Time.timeScale == targetTimeScale )
        {
            if ( callback != null )
            {
                callback();
            }

            enabled = false;
        }
    }

    public void LerpTimeScale( float target_timescale, float lerp_speed, Action callback )
    {
        timeScaleLerpLastTime = Time.realtimeSinceStartup;
        targetTimeScale = target_timescale;
        lerpSpeed = lerp_speed;
        this.callback = callback;

        enabled = true;
    }
}
