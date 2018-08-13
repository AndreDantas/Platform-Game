using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class DeactivateObject : MonoBehaviour
{

    public bool destroy = false;

    public void Deactivate()
    {
        if (!destroy)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }
}
