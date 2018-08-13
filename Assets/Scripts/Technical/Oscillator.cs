using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
public enum ObjectRotation { Normal, NoRotation }

/// <summary>
/// Rotates an object around a world point and/or around it's own axis.
/// </summary>
public class Oscillator : MonoBehaviour
{

    public ObjectRotation objectRotation;
    public bool usePosition = true;
    [ShowIf("usePosition")]
    public Transform followPosition;
    public bool rotate = true;
    public float startAngle = 0;
    float rotateObject = 0;
    float rotatePosition = 0;
    Vector3 localPos;
    [HideInInspector]
    public Vector3 defaultFollowPosition;
    public float objRotationSpeed = 5;
    public float positionRotationSpeed = 5;
    public float width;
    public float height;
    float angle;
    float cos, sin;


    Vector3 gizmoPos;
    Vector3 pos;

    void OnValidate()
    {
        objRotationSpeed = Mathf.Clamp(objRotationSpeed, -100, 100);
        width = Mathf.Clamp(width, 0, 10000);
        height = Mathf.Clamp(height, 0, 10000);
        startAngle = Mathf.Clamp(startAngle, 0, 360);
    }
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            gizmoPos = transform.position;
        else
        {
            if (usePosition)
                gizmoPos = localPos;
            else
                gizmoPos = transform.position;
        }

        Gizmos.color = Color.red;
        float theta = 0;
        float x = width * Mathf.Cos(theta);
        float y = height * Mathf.Sin(theta);
        Vector3 pos = gizmoPos + new Vector3(x, y, 0);
        Vector3 newPos = pos;
        Vector3 lastPos = pos;
        for (theta = 0.1f; theta < (Mathf.PI * 2); theta += 0.1f)
        {
            x = width * Mathf.Cos(theta);
            y = height * Mathf.Sin(theta);
            newPos = gizmoPos + new Vector3(x, y, 0);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }
        Gizmos.DrawLine(pos, lastPos);
    }
#endif
    public void SetValues(int normalRotationSpeed,
        float speed,
        float width,
        float height,
        ObjectRotation objRotation = ObjectRotation.Normal,
        bool rotate = true,
        float startAngle = 0)
    {
        this.objRotationSpeed = speed;
        this.width = width;
        this.height = height;
        this.rotate = rotate;
        this.objectRotation = objRotation;
        this.startAngle = startAngle;


    }
    // Use this for initialization
    void Start()
    {
        if (followPosition == null)
        {
            localPos = transform.position;
            gizmoPos = transform.position;
        }
        defaultFollowPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //Starting angle of object
        rotatePosition = rotateObject = startAngle * Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            Rotate();
        }
    }

    public void Rotate()
    {
        cos = Mathf.Cos(rotateObject);
        sin = Mathf.Sin(rotateObject);

        //0 - 180
        if (1 >= sin && sin >= 0)
            angle = (Mathf.Acos(cos) * 180) / Mathf.PI;
        //180 - 270
        else if (-1 < cos && cos < 0)
            angle = ((Mathf.Abs(Mathf.Asin(sin) * 180))) / Mathf.PI + 180;
        //270 - 360 
        else
            angle = ((Mathf.Abs(Mathf.Acos(-cos) * 180))) / Mathf.PI + 180;

        if (objectRotation == ObjectRotation.NoRotation)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if (objectRotation == ObjectRotation.Normal)
            transform.eulerAngles = new Vector3(0, 0, angle);

        if (rotateObject * Mathf.Rad2Deg > 360)
            rotateObject = (rotateObject * Mathf.Rad2Deg - 360) * Mathf.Deg2Rad;
        else if (rotateObject * Mathf.Rad2Deg < 0)
            rotateObject = (rotateObject * Mathf.Rad2Deg + 360) * Mathf.Deg2Rad;

        rotateObject += Time.deltaTime * objRotationSpeed;

        if (usePosition)
        {
            if (rotatePosition * Mathf.Rad2Deg > 360)
                rotatePosition = (rotatePosition * Mathf.Rad2Deg - 360) * Mathf.Deg2Rad;
            else if (rotateObject * Mathf.Rad2Deg < 0)
                rotatePosition = (rotatePosition * Mathf.Rad2Deg + 360) * Mathf.Deg2Rad;
            rotatePosition += Time.deltaTime * positionRotationSpeed;

            if (followPosition != null)
            {
                localPos = followPosition.position;
                gizmoPos = followPosition.position;
            }
            else
            {
                localPos = defaultFollowPosition;
                gizmoPos = defaultFollowPosition;
            }


            pos = new Vector3(Mathf.Cos(rotatePosition) * width, Mathf.Sin(rotatePosition) * height, 0);
            transform.position = new Vector3(pos.x, pos.y, pos.z) + localPos;
        }
    }
}
