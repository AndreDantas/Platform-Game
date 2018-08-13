using UnityEngine;
using System.Collections;

public class DeactivateObjectOffCamera : DeactivateObject
{

    DeactivateObjectOnTime deact;

    void Start()
    {
        deact = gameObject.GetComponent<DeactivateObjectOnTime>();
    }
    void OnBecameInvisible()
    {
        if (deact)
            deact.Reset();
        Deactivate();
    }
}
