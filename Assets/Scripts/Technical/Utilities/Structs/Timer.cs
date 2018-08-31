using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public struct Timer
{
    [SerializeField, HideInInspector]
    private float time;

    public Timer(float time)
    {
        this.time = UtilityFunctions.ClampMin(time, 0f);
        Counter = 0f;
        isFinished = false;
    }


    [ShowInInspector, PropertyOrder(-1)]
    public float Time
    {
        get
        {
            return time;
        }

        set
        {
            time = UtilityFunctions.ClampMin(value, 0f);
        }
    }


    [ReadOnly, ProgressBar(0f, "Time")]
    public float Counter;
    [ReadOnly, ShowInInspector]
    public bool isFinished { get; private set; }
    [Button(ButtonSizes.Medium), ButtonGroup("G1")]
    public void Update()
    {
        if (isFinished)
        {
            Counter = Time;
            return;
        }
        Counter += UnityEngine.Time.deltaTime;
        if (Counter >= Time)
        {
            isFinished = true;
            Counter = Time;
        }
        else
            isFinished = false;


    }
    [Button(ButtonSizes.Medium), ButtonGroup("G1")]
    public void Reset()
    {
        Counter = 0f;
        isFinished = false;
    }
}
