using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using Sirenix.OdinInspector;
public enum MovementState
{
    Normal,
    WallClimbing,

}
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerMovements : MonoBehaviour
{
    [ReadOnly, PropertyOrder(-2)]
    public MovementState movementState = MovementState.Normal;

    [SerializeField, PropertyOrder(-1)]
    Player player;
    BoxCollider2D boxCol;
    Rigidbody2D rb;
    float rbGravityScale;

    [BoxGroup("Wall Climb")]
    public bool EnableWallClimbing = true;
    [BoxGroup("Wall Climb"), ShowIf("EnableWallClimbing")]
    public bool ResetAirJumps = false;

    [Range(0f, 20f), BoxGroup("Wall Climb"), ShowIf("EnableWallClimbing")] public float wallClimbSpeed = 2f;
    [Range(0f, 20f), BoxGroup("Wall Climb"), ShowIf("EnableWallClimbing")] public float wallSlidingSpeed = 0f;
    [Range(-1f, 2f), BoxGroup("Wall Climb"), ShowIf("EnableWallClimbing")] public float wallReleaseTime = 0.2f;
    protected float wallReleaseCount = 0f;

    [SerializeField, ReadOnly, BoxGroup("Wall Climb"), ShowIf("EnableWallClimbing")] bool wallOnLeft;
    [SerializeField, ReadOnly, BoxGroup("Wall Climb"), ShowIf("EnableWallClimbing")] bool wallOnRight;

    [BoxGroup("Ledge Climb")]
    public bool EnableLedgeClimbing = true;
    [BoxGroup("Ledge Climb"), Range(0f, 1f), ShowIf("EnableLedgeClimbing")] public float ledgeClimbTime = 0.2f;
    [BoxGroup("Ledge Climb"), Range(0f, 100f), ShowIf("EnableLedgeClimbing")] public float minimumGroundDistance = 1.5f;

    [BoxGroup("Movement"), PropertyOrder(0)] public bool canMove { get { return (player?.CanMove() ?? false); } set { player?.SetCanMove(value); } }
    [BoxGroup("Movement"), Range(0f, 10f)] public float speed = 4f;
    [BoxGroup("Movement")] public float maxVelocityX = 10f;
    [BoxGroup("Movement")] public float maxVelocityY = 20f;
    bool inputRight { get { return (player?.InputRight() ?? false); } }
    bool inputLeft { get { return (player?.InputLeft() ?? false); } }
    bool inputUp { get { return (player?.InputUp() ?? false); } }
    bool inputDown { get { return (player?.InputDown() ?? false); } }

    [BoxGroup("Movement"), ReadOnly]
    public bool grounded;

    [BoxGroup("Jump")] public float groundJumpPower = 20f;
    [BoxGroup("Jump")] public float airJumpPower = 20f;
    [BoxGroup("Jump")] public float wallJumpPower = 20f;
    [BoxGroup("Jump")] public float fallMultiplier = 2.5f;
    [BoxGroup("Jump")] public float lowJumpMultiplier = 2f;
    [BoxGroup("Jump"), Range(0, 99)] public int totalAirJump = 1;
    protected int airJumpCount;
    [ReadOnly, SerializeField, BoxGroup("Jump")]
    protected bool holdingJumpButton;




    // -------------------- TO DO ------------------- \\
    /* 
     * Create enemy AI
     * Make enemy Movement
     * 
    */
    // ----------------------------------------------- \\
    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        rbGravityScale = rb.gravityScale;
        boxCol = GetComponent<BoxCollider2D>();
        player = GetComponent<Player>();

    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rbGravityScale = rb.gravityScale;
        boxCol = GetComponent<BoxCollider2D>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        PrepareMovement();

    }
    private void FixedUpdate()
    {
        ExecuteMovement();

    }

    protected void PrepareMovement()
    {

        //Checking if the player is on the ground
        grounded = GroundCheck();

        bool touchingWall = CheckForWall(1) || CheckForWall(-1);

        switch (movementState)
        {
            case MovementState.Normal:


                if (player.IsMovingHorizontal())
                {

                    if (touchingWall)
                    {

                        rb.velocity = new Vector2(0, rb.velocity.y);
                        if (TransitionToWallclimb())
                            return;
                    }
                }

                if (!grounded)
                    CheckStopJump();
                else
                {
                    ResetJumps();
                }
                break;
            case MovementState.WallClimbing:
                //player.CheckMovementVertical();
                if (!HandleWallClimb())
                    return;
                break;
            default:
                break;
        }

        if (player.IsMovingHorizontal() || movementState == MovementState.WallClimbing)
        {
            if (touchingWall)
            {
                if (CheckForClimbableLedge(wallOnRight ? true : false))
                    ClimbLedge(wallOnRight ? true : false);
            }


        }
    }

    protected void ExecuteMovement()
    {
        switch (movementState)
        {
            case MovementState.Normal:
                if ((player?.CanMove() ?? false))
                {
                    if (player.IsMovingHorizontal())
                        MoveHorizontal(player.GetMoveX());
                    else
                        player.ReduceHorizontalVelocity();
                }
                break;
            case MovementState.WallClimbing:
                if (player?.CanMove() ?? false)
                    ClimbWall(player.GetMoveY());
                else
                    TransitionToNormal();
                break;
            default:
                break;
        }

        if (rb)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -Mathf.Abs(maxVelocityX), Mathf.Abs(maxVelocityX)),
                                   Mathf.Clamp(rb.velocity.y, -Mathf.Abs(maxVelocityY), Mathf.Abs(maxVelocityY)));
        }
    }

    public void PressJumpButton(bool pressed)
    {
        this.holdingJumpButton = pressed;
    }



    public bool GroundCheck()
    {
        List<GameObject> collisions = Check2DCollisions.CheckCollisionDown(boxCol, 5, 0.02f, layerMask: ~LayerMask.GetMask(Character.IgnoreLayers));
        //foreach (var item in collisions)
        //{
        //    if (LayerMask.LayerToName(item.layer) == "Platform")
        //        return true;
        //}

        return collisions.Count > 0;
    }


    /// <summary>
    /// Used to move the player with no restrictions.
    /// </summary>
    /// <param name="endPos">Position to move the player</param>
    /// <param name="time">Time to move</param>
    /// <returns></returns>
    IEnumerator MovePlayer(Vector2 endPos, float time, bool canMove = true)
    {
        if (time < 0f)
            time = 0f;
        float i = 0.0f;
        float rate = 1.0f / time;
        Vector3 startPos = transform.position;
        this.canMove = canMove;
        rb.isKinematic = true;
        while (i < 1.0)
        {
            i += Time.deltaTime * rate;
            this.transform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
        rb.velocity = Vector2.zero;
        this.canMove = true;
        rb.isKinematic = false;
    }




    // -------------------------------- Normal Movement ----------------------------------\\
    public void MoveHorizontal(float direction)
    {
        if (rb)
        {
            rb.velocity = new Vector2(speed * Time.deltaTime * 100f * direction, rb.velocity.y);
        }
    }

    void TransitionToNormal()
    {
        if (movementState == MovementState.Normal)
            return;
        movementState = MovementState.Normal;
        rb.gravityScale = rbGravityScale;
    }


    // ------------------------------------------------------------------------------------\\

    // -------------------------------- Wallclimb Movement ----------------------------------\\

    bool TransitionToWallclimb()
    {
        //Debug.Log("Transition wallclimb");
        if (movementState == MovementState.WallClimbing || !EnableWallClimbing)
            return false;
        movementState = MovementState.WallClimbing;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        wallReleaseCount = 0f;
        player.SetMoveX(0);
        player.SetMoveY(0);

        return true;
    }

    void WallJump()
    {
        if (movementState != MovementState.WallClimbing || !rb)
            return;

        TransitionToNormal();
        if (grounded)
        {
            rb.velocity = new Vector2(0f, 0f);
            StartCoroutine(player.Immobilize(0.2f));
            rb.AddForce(new Vector2(0, groundJumpPower), ForceMode2D.Impulse);
            return;
        }


        if (Mathf.Abs(player.GetMoveX()) >= 0.7f)
        {
            rb.velocity = new Vector2(0f, 0f);


            var force = Vector2.zero;
            if (wallOnLeft)
            {
                if (inputRight)
                {
                    force = new Vector2(wallJumpPower * 0.8f, wallJumpPower * 0.6f); StartCoroutine(player.Immobilize(0.25f));
                }
                else
                {
                    force = new Vector2(wallJumpPower / 3f, wallJumpPower * 0.9f); StartCoroutine(player.Immobilize(0.15f));
                }
            }
            else
            {
                if (inputLeft)
                {
                    force = new Vector2(-wallJumpPower * 0.8f, wallJumpPower * 0.6f); StartCoroutine(player.Immobilize(0.25f));
                }
                else
                {
                    force = new Vector2(-wallJumpPower / 3f, wallJumpPower * 0.9f); StartCoroutine(player.Immobilize(0.15f));
                }
            }
            rb.AddForce(force, ForceMode2D.Impulse);
        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);
            StartCoroutine(player.Immobilize(0.1f));
            rb.AddForce(new Vector2(wallJumpPower / 5f * (wallOnLeft ? 1f : -1f), wallJumpPower / 8f * (inputUp ? 1f : -1f)), ForceMode2D.Impulse);
        }
    }
    void ClimbWall(float direction)
    {
        if (!EnableWallClimbing)
        {
            TransitionToNormal();
            return;
        }
        if (rb)
        {
            rb.velocity = new Vector2(rb.velocity.x, wallClimbSpeed * Time.deltaTime * 100f * direction);
            rb.AddForce(new Vector2(0, -wallSlidingSpeed * 10f));
        }
    }

    /// <summary>
    /// Checks for a wall on the left or right.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public bool CheckForWall(float direction)
    {
        if (!boxCol)
            return wallOnLeft = wallOnRight = false;

        //Check for wall on right
        if (Mathf.Sign(direction) == 1)
        {
            if (Check2DCollisions.CheckCollisionRight(boxCol, 5, 0.01f, 0.03f).Count != 0)
            {

                wallOnRight = true;
                wallOnLeft = false;
                return true;
            }
        }
        //Check for wall on left
        else
        {
            if (Check2DCollisions.CheckCollisionLeft(boxCol, 5, 0.01f, 0.03f).Count != 0)
            {
                wallOnRight = false;
                wallOnLeft = true;
                return true;
            }

        }
        return wallOnLeft = wallOnRight = false;
    }

    /// <summary>
    /// Handles wall interactions.
    /// </summary>
    /// <returns></returns>
    bool HandleWallClimb()
    {
        if (!EnableWallClimbing)
        {
            //Go back to normal

            TransitionToNormal();
            return false;
        }
        LayerMask ignore = LayerMask.GetMask(Character.IgnoreLayers);
        ignore = ~ignore;
        //If there are no walls on either side
        if (Check2DCollisions.CheckCollisionRight(boxCol, 5, 0.01f, 0.03f, ignore).Count == 0 &&
            Check2DCollisions.CheckCollisionLeft(boxCol, 5, 0.01f, 0.03f, ignore).Count == 0)
        {
            //Go back to normal
            TransitionToNormal();
            return false;
        }

        //If on ground and moving to opposite direction of wall
        if (grounded)
        {
            if ((player.GetMoveX() < 0 && wallOnRight) || (player.GetMoveX() > 0 && wallOnLeft))
            {
                //Go back to normal
                TransitionToNormal();
                return false;
            }
        }
        if (ResetAirJumps)
            ResetJumps();
        if (CheckForRelease())
            TransitionToNormal();

        return true;


    }

    bool CheckForClimbableLedge(bool onRight = true)
    {
        if (!EnableLedgeClimbing)
            return false;
        float width = (boxCol?.size.x ?? 1f) * (onRight ? 1 : -1);
        float height = boxCol?.size.y ?? 1f;

        Vector2 pos = new Vector2(transform.position.x + width * 0.98f, transform.position.y + height / 2f);
        var dir = onRight ? Directions.Right : Directions.Left;
        return !Check2DCollisions.CheckSquare2D(pos, Mathf.Abs(width), height, 10, ~LayerMask.GetMask(Character.IgnoreLayers), dir) &&
                (Check2DCollisions.CheckCollisionDown(boxCol, 5, 0.05f, minimumGroundDistance, ~LayerMask.GetMask(Character.IgnoreLayers)).Count == 0);



    }

    void ClimbLedge(bool onRight = true)
    {
        if (!EnableLedgeClimbing)
            return;

        float width = (boxCol?.size.x ?? 1f) * (onRight ? 1 : -1);
        float height = boxCol?.size.y ?? 1f;

        Vector2 checkPoint = new Vector2(transform.position.x + width, transform.position.y);
        var hit = Physics2D.Raycast(checkPoint, Vector2.down, height / 2f, layerMask: ~LayerMask.GetMask(Character.IgnoreLayers));

        float moveY = height / 2f - (hit ? hit.distance : 0f);

        Vector2 movePoint = new Vector2(transform.position.x + width, transform.position.y + player.GetMoveY());

        TransitionToNormal();

        StartCoroutine(MovePlayer(movePoint, ledgeClimbTime, false));
    }

    bool CheckForRelease()
    {

        if (!EnableWallClimbing || movementState != MovementState.WallClimbing)
        {
            wallReleaseCount = 0f;
            return true;
        }
        if (wallReleaseTime < 0)
            return false;
        if ((inputLeft && wallOnRight) || (inputRight && wallOnLeft))
        {
            if (wallReleaseCount >= wallReleaseTime)
            {
                wallReleaseCount = 0f;
                return true;
            }
            else
                wallReleaseCount += Time.deltaTime;
        }
        else
            wallReleaseCount = 0f;

        return false;

    }

    public bool WallOnLeft()
    {
        return wallOnLeft;
    }

    public bool WallOnRight()
    {
        return wallOnRight;
    }

    // ------------------------------------------------------------------------------------\\
    public void Jump()
    {
        if (rb && (player?.CanMove() ?? false))
        {
            switch (movementState)
            {
                case MovementState.Normal:
                    if (grounded)
                        NormalJump();
                    else
                        AirJump();
                    break;
                case MovementState.WallClimbing:
                    WallJump();
                    break;
                default:
                    break;
            }

        }
    }

    /// <summary>
    /// Jumps on air.
    /// </summary>
    public void AirJump()
    {
        if (airJumpCount >= totalAirJump)
            return;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(0, airJumpPower), ForceMode2D.Impulse);
        airJumpCount++;
    }

    public void NormalJump()
    {

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(0, groundJumpPower), ForceMode2D.Impulse);
    }

    void CheckStopJump()
    {
        if (!rb)
            return;
        if (rb.velocity.y < 0)
        {
            rb.AddForce(new Vector2(0, Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime), ForceMode2D.Force);
        }
        else if (rb.velocity.y > 0 && !holdingJumpButton)
            rb.AddForce(new Vector2(0, Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime), ForceMode2D.Force);

    }

    void ResetJumps()
    {
        airJumpCount = 0;
    }
}
