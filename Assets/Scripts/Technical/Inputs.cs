using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class Inputs
{

    #region Fields/Constructors
    [SerializeField, HideInInspector]
    private bool left, right, up, down;

    public bool oneDirectionEnabledPerAxis = true;

    public Inputs(bool oneDirectionEnabledPerAxis)
    {
        this.oneDirectionEnabledPerAxis = oneDirectionEnabledPerAxis;
    }

    public Inputs()
    {
    }

    [ShowInInspector]
    public bool Left
    {
        get
        {
            return left;
        }

        set
        {
            left = value;
            if (oneDirectionEnabledPerAxis && value)
                Right = false;

        }
    }

    [ShowInInspector]
    public bool Right
    {
        get
        {
            return right;
        }

        set
        {
            right = value;
            if (oneDirectionEnabledPerAxis && value)
                Left = false;
        }
    }

    [ShowInInspector]
    public bool Up
    {
        get
        {
            return up;
        }

        set
        {
            up = value;
            if (oneDirectionEnabledPerAxis && value)
                Down = false;
        }
    }

    [ShowInInspector]
    public bool Down
    {
        get
        {
            return down;
        }

        set
        {
            down = value;
            if (oneDirectionEnabledPerAxis && value)
                Up = false;
        }
    }

    #endregion

    public override string ToString()
    {
        return "Right: " + right + " | Left: " + left + " | Up: " + up + " | Down: " + down;
    }
}
