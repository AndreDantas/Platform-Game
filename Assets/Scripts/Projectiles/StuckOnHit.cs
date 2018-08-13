using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
/// <summary>
/// This script makes the projectile get stuck when it hits another collider.
/// </summary>
public abstract class StuckOnHit : Projectile
{
    [SerializeField]
    [ReadOnly]

    protected bool stuck;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnTriggerEnter2D(Collider2D col)
    {

        if (!CheckLayer(col.gameObject.layer))
        {
            if (rb && !stuck)
            {
                rb.simulated = false;
                stuck = true;
                this.transform.parent = col.gameObject.transform;
                OnStuck(col);

            }
        }
        base.OnTriggerEnter2D(col);
    }
    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (!CheckLayer(col.gameObject.layer))
        {
            if (rb && !stuck)
            {
                rb.simulated = false;
                stuck = true;
                this.transform.parent = col.gameObject.transform;
                OnStuck(col);

            }
        }
        base.OnCollisionEnter2D(col);
    }

    protected void OnEnable()
    {
        this.transform.parent = null;
        stuck = false;
        if (rb)
        {
            rb.simulated = true;
        }
    }

    public bool IsStuck()
    {
        return stuck;
    }

    protected abstract void OnStuck(Collision2D collision);

    protected abstract void OnStuck(Collider2D col);
}
