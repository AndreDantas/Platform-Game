using System;
using UnityEngine;
using System.Collections;

public class DeactiveObjectOnCollision : DeactivateObject
{

    DeactivateObjectOnTime deact;
    public LayerMask ignore;

    void Start()
    {
        deact = gameObject.GetComponent<DeactivateObjectOnTime>();

    }

    public bool CheckLayer(int layer)
    {

        if (((1 << layer) & ignore) == 0)
        {
            return false;
        }
        else
            return true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!CheckLayer(col.gameObject.layer))
        {
            if (deact != null)
                deact.Reset();
            Deactivate();
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (!CheckLayer(col.gameObject.layer))
        {
            if (deact != null)
                deact.Reset();
            Deactivate();
        }
    }


}
