using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Oscillator))]
public class Shuriken : StuckOnHit
{
    Oscillator osc;
    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        osc = GetComponent<Oscillator>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        projectileGravity = 0f;
    }

    protected override void OnStuck(Collision2D collision)
    {
        osc.rotate = false;
    }
    protected override void OnStuck(Collider2D col)
    {
        osc.rotate = false;
    }
}
