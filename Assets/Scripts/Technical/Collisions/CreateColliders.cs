using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class CreateColliders : MonoBehaviour
{

    public float offSetX = 0.05f; // X offset from the BoxCollider2D vertices
    public float offSetY = 0.05f; // Y offset from the BoxCollider2D vertices
    public bool reduceBoxCollider { get; set; }// Reduces the size of the BoxCollider2D to be in the center (between the colliders)
    public bool makeTrigger { get; set; } // Makes the colliders trigger
    BoxCollider2D boxColliderRef;
    float width, height;
    public PhysicsMaterial2D material; // Material to be applicated on the colliders
    public bool collisionUp, collisionDown, collisionLeft, collisionRight; // Collision status of each collider
    [HideInInspector]
    public GameObject upCol, downCol, leftCol, rightCol; // Objects that collided with the colliders
    [HideInInspector]
    public GameObject leftCollider, rightCollider, upCollider, downCollider; // Colliders references
    CheckCollision checkUp, checkDown, checkLeft, checkRight; // Colliders 'CheckCollision' script references
    Collider2D left, right, up, down;

    void Awake()
    {

        boxColliderRef = GetComponent<BoxCollider2D>();
        boxColliderRef.transform.position = transform.position;

        width = boxColliderRef.size.x; // Width of the BoxCollider2D
        height = boxColliderRef.size.y; // Height of the BoxXollider2D

        // Creation of the colliders
        leftCollider = CreatePolygonCollider(Vector2.left);
        rightCollider = CreatePolygonCollider(Vector2.right);
        upCollider = CreatePolygonCollider(Vector2.up);
        downCollider = CreatePolygonCollider(Vector2.down);

        left = leftCollider.GetComponent<Collider2D>();
        right = rightCollider.GetComponent<Collider2D>();
        up = upCollider.GetComponent<Collider2D>();
        down = downCollider.GetComponent<Collider2D>();
    }

    void Start()
    {

        GameObject colliders = new GameObject(); // Colliders' parent
        colliders.name = "Colliders";
        colliders.transform.parent = transform;
        leftCollider.transform.parent = rightCollider.transform.parent = upCollider.transform.parent = downCollider.transform.parent = colliders.transform;

        // References to the collision status of the colliders
        checkUp = upCollider.GetComponent<CheckCollision>();
        checkDown = downCollider.GetComponent<CheckCollision>();
        checkLeft = leftCollider.GetComponent<CheckCollision>();
        checkRight = rightCollider.GetComponent<CheckCollision>();

        if (reduceBoxCollider)
            boxColliderRef.size = new Vector2(boxColliderRef.size.x / 2, boxColliderRef.size.y / 2); // Reducing BoxCollider2D size
    }


    void Update()
    {

        left.isTrigger = right.isTrigger = down.isTrigger = up.isTrigger = makeTrigger;

        // Update the collision status
        collisionUp = checkUp.collision;
        collisionDown = checkDown.collision;
        collisionLeft = checkLeft.collision;
        collisionRight = checkRight.collision;

        // Update the objects
        upCol = checkUp.col;
        downCol = checkDown.col;
        leftCol = checkLeft.col;
        rightCol = checkRight.col;

        offSetX = Mathf.Clamp(offSetX, 0, boxColliderRef.size.x / 2);
        offSetY = Mathf.Clamp(offSetY, 0, boxColliderRef.size.y / 2);
    }

    GameObject CreatePolygonCollider(Vector2 direction)
    {
        GameObject col = new GameObject();
        col.AddComponent<PolygonCollider2D>();
        col.AddComponent<CheckCollision>();
        col.transform.position = new Vector2(boxColliderRef.bounds.center.x - width / 2, boxColliderRef.bounds.center.y - height / 2);

        if (this.material != null)
            col.GetComponent<CheckCollision>().material = this.material;

        PolygonCollider2D pol;

        if (direction.normalized == Vector2.up)
        {
            col.name = "UpCollider";
            pol = col.GetComponent<PolygonCollider2D>();
            pol.pathCount = 1;
            Vector2[] upPoints = new Vector2[4];
            upPoints[0] = new Vector2(0 + offSetX, height);
            upPoints[1] = new Vector2(width - offSetX, height);
            upPoints[2] = new Vector2(width * 3 / 4, height * 3 / 4);
            upPoints[3] = new Vector2(width / 4, height * 3 / 4);
            pol.SetPath(0, upPoints);

        }
        else if (direction.normalized == Vector2.down)
        {
            col.name = "DownCollider";
            pol = col.GetComponent<PolygonCollider2D>();
            pol.pathCount = 1;
            Vector2[] downPoints = new Vector2[4];
            downPoints[0] = new Vector2(0 + offSetX, 0);
            downPoints[1] = new Vector2(width - offSetX, 0);
            downPoints[2] = new Vector2(width * 3 / 4, height / 4);
            downPoints[3] = new Vector2(width / 4, height / 4);
            pol.SetPath(0, downPoints);

        }
        else if (direction.normalized == Vector2.left)
        {
            col.name = "LeftCollider";
            pol = col.GetComponent<PolygonCollider2D>();
            pol.pathCount = 1;
            Vector2[] leftPoints = new Vector2[4];
            leftPoints[0] = new Vector2(0, 0 + offSetY);
            leftPoints[1] = new Vector2(0, height - offSetY);
            leftPoints[2] = new Vector2(width / 4, height * 3 / 4);
            leftPoints[3] = new Vector2(width / 4, height / 4);
            pol.SetPath(0, leftPoints);

        }
        else if (direction.normalized == Vector2.right)
        {
            col.name = "RightCollider";
            pol = col.GetComponent<PolygonCollider2D>();
            pol.pathCount = 1;
            Vector2[] rightPoints = new Vector2[4];
            rightPoints[0] = new Vector2(width, height - offSetY);
            rightPoints[1] = new Vector2(width, 0 + offSetY);
            rightPoints[2] = new Vector2(width * 3 / 4, height / 4);
            rightPoints[3] = new Vector2(width * 3 / 4, height * 3 / 4);
            pol.SetPath(0, rightPoints);
        }


        return col;

    }
}
