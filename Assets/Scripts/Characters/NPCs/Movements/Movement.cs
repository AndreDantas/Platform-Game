using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public abstract class Movement : MonoBehaviour
{

    public Character character;
    public float speed = 5f;

    public abstract void Move();

    protected virtual void Start()
    {
        if (!character)
            character = GetComponent<Character>();
    }

}
