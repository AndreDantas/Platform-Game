using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    public GameObject target;
    public float smoothFollow = 0.15f;
    public bool follow = true;
    public float mininumDistance = 1f;
    public Vector2 offset;
    Vector3 velocity;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (follow && target != null)
        {
            Vector3 followPos = Vector3.SmoothDamp(transform.position, target.transform.position, ref velocity, smoothFollow);
            transform.position = new Vector3(followPos.x + offset.x, followPos.y + offset.y, transform.position.z);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, target.transform.position.x - (mininumDistance + offset.x),
                                                        target.transform.position.x + (mininumDistance + offset.x)),
                                            Mathf.Clamp(transform.position.y, target.transform.position.y - (mininumDistance + offset.y),
                                                        target.transform.position.y + (mininumDistance + offset.y)),
                                            transform.position.z);

        }
    }
}
