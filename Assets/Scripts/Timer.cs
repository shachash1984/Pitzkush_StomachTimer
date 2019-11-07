using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    [SerializeField] bool isRunning;
    public bool IsRunning { get => isRunning; set => isRunning = value; }
    public float Elapsed { get; private set; }
    
    public void Run()
    {
        IsRunning = true;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void Tick(float AddedTime)
    {
        Elapsed += AddedTime;
    }

    public void ResetTimer()
    {
        Elapsed = 0f;
    }

    public override string ToString()
    {
        int hours = (int)Elapsed / 3600;
        int minutes = ((int)Elapsed / 60) % 60;
        int seconds = (int)Elapsed % 60;
        return string.Format("{0}:{1}:{2}", hours, minutes, seconds);
    }

    public static string ToString(float time)
    {
        int hours = (int)time / 3600;
        int minutes = ((int)time / 60) % 60;
        int seconds = (int)time % 60;
        return string.Format("{0}:{1}:{2}", hours, minutes, seconds);
    }
}
