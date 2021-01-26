using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    float initTime;
    float time;

    public Timer(float time) => this.time = time;

    public void Start()
    {
        initTime = Time.realtimeSinceStartup;
    }
    public bool IsFinished()
    {
        if (Time.realtimeSinceStartup - initTime >= time)
        {
            return true;
        }

        return false;
    }
}
