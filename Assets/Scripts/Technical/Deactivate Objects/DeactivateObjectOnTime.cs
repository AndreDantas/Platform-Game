using UnityEngine;
using System.Collections;

public class DeactivateObjectOnTime : DeactivateObject
{

    public float deactivateTime = 0;
    float timeToDeactivate = 0;

    void Update()
    {

        if (deactivateTime > 0)
        {
            timeToDeactivate += Time.deltaTime;
            if (timeToDeactivate >= deactivateTime)
            {
                timeToDeactivate = 0;
                Deactivate();

            }
        }
    }

    private void OnDisable()
    {
        timeToDeactivate = 0;
    }

    private void OnEnable()
    {
        timeToDeactivate = 0;
    }

    public void Reset()
    {

        timeToDeactivate = 0;

    }
}
