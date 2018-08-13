using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBounds : MonoBehaviour
{

    public float minX, maxX, minY, maxY;
    float vertExtent;
    float horzExtent;
    void Start()
    {
        ClampBounds();
        CalculateCameraBounds();

    }

    void CalculateCameraBounds()
    {
        vertExtent = GetComponent<Camera>().orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }

    private void OnValidate()
    {
        ClampBounds();
    }
    void LateUpdate()
    {
        ClampBounds();
        var v3 = transform.position;
        v3.x = Mathf.Clamp(v3.x, minX + horzExtent, maxX - horzExtent);
        v3.y = Mathf.Clamp(v3.y, minY + vertExtent, maxY - vertExtent);
        transform.position = v3;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(minX, minY), new Vector2(maxX, minY));//Bottom
        Gizmos.DrawLine(new Vector2(minX, maxY), new Vector2(maxX, maxY));//Top
        Gizmos.DrawLine(new Vector2(minX, minY), new Vector2(minX, maxY));//Left
        Gizmos.DrawLine(new Vector2(maxX, minY), new Vector2(maxX, maxY));//Right
    }

    void ClampBounds()
    {
        minX = UtilityFunctions.ClampMax(minX, maxX);
        maxX = UtilityFunctions.ClampMin(maxX, minX);
        minY = UtilityFunctions.ClampMax(minY, maxY);
        maxY = UtilityFunctions.ClampMin(maxY, minY);
    }

}
