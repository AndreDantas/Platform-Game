using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityController : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
        QualitySettings.antiAliasing = 0;
    }


}
