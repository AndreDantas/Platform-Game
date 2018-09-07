using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class ObjectBounds : MonoBehaviour
{

    public Bounds bounds = new Bounds(Vector3.zero, Vector3.one);
    public bool on = true;

    private void Update()
    {
        if (on)
        {
            CheckForBounds();
        }
    }

    public void CheckForBounds()
    {
        if (!bounds.Contains(transform.position))
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, bounds.min.x, bounds.max.x),
                                            Mathf.Clamp(transform.position.y, bounds.min.y, bounds.max.y), transform.position.z);
    }

    private void OnDrawGizmosSelected()
    {
        UtilityFunctions.GizmosDrawBounds(bounds, Color.red);
    }
}
