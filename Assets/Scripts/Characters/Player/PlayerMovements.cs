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

    [Range(0f, 100f), BoxGroup("Wall Climb"), ShowIf("EnableWallClimbing")] public float wallClimbSpeed = 2f;
    [Range(0f, 100f), BoxGroup("Wall Climb"), MinValue(0), ShowIf("EnableWallClimbing")] public float wallSlidingSpeed = 0f;
    [Range(0f, 2f), BoxGroup("Wall Climb"), MinValue(0), ShowIf("EnableWallClimbing")] public float wallReleaseTime = 0.2f;
    protected float wallReleaseCount = 0f;

    [SerializeField, ReadOnly, BoxGroup("Wall Climb"), ShowIf("EnableWallClimbing")] bool wallOnLeft;
    [SerializeField, ReadOnly, BoxGroup("Wall Climb"), ShowIf("EnableWallClimbing")] bool wallOnRight;

    [BoxGroup("Movement"), PropertyOrder(0)] public bool canMove { get { return (player?.CanMove() ?? false); } set { player?.SetCanMove(value); } }
    [BoxGroup("Movement"), Range(0f, 10f)] public float speed = 4f;
    [BoxGroup("Movement"), SerializeField, ReadOnly] float moveX;
    [BoxGroup("Movement"), SerializeField, ReadOnly] float moveY;
    [BoxGroup("Movement")] public float maxVelocityX = 10f;
    [BoxGroup("Movement")] public float maxVelocityY = 20f;
    [BoxGroup("Movement")] public float deacceleration = 0.1f;


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


    /// <summary>
    /// Player input reference
    /// </summary>
    [SerializeField, ReadOnly, BoxGroup("Inputs")]
    protected bool inputRight, inputLeft, inputUp, inputDown;


    // -------------------- TO DO ------------------- \\
    /* 
     * Create enemy AI
     * Make enemy Movement
     * 
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

        // -------------------- TEST ---------------------\\
        if (player != null ? player.CanMove() : false)
        {
            if (!IsMovingHorizontal()) //Keyboard horizontal Input
            {
                moveX = Input.GetAxisRaw("Horizontal");
                if (moveX != 0)
                {
                    inputRight = moveX > 0;
                    inputLeft = moveX < 0;

                }
                else
                {
                    inputRight = inputLeft = false;
                }
            }
            if (!IsMovingVertical()) //Keyboard vertical Input
            {
                moveY = Input.GetAxisRaw("Vertical");
                if (moveY != 0)
                {
                    inputUp = moveY > 0;
                    inputDown = moveY < 0;
                }
                else
                {
                    inputDown = inputUp = false;
                }
            }

            if (Input.GetButtonDown("Jump"))
            {
                PressJumpButton(true);
                Jump();
            }
            else if (Input.GetButtonUp("Jump"))
            {
                PressJumpButton(false);
            }
        }
        else
        {
            NoInput();
        }
        // ---------------------------------------------- \\ 

        switch (movementState)
        {
            case MovementState.Normal:

                if (CheckMovementHorizontal())
                {
                    if (CheckForWall(Mathf.Sign(moveX)))
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
                CheckMovementVertical();
                if (!HandleWallClimb())
                    return;
                break;
            default:
                break;
        }
    }

    protected void ExecuteMovement()
    {
        switch (movementState)
        {
            case MovementState.Normal:
                if ((player?.CanMove() ?? false) && IsMovingHorizontal())
                    MoveHorizontal(moveX);
                else
                    ReduceHorizontalVelocity();
                break;
            case MovementState.WallClimbing:
                if (player?.CanMove() ?? false)
                    ClimbWall(moveY);
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


    /// <summary>
    /// Checks for horizontal input
    /// </summary>
    /// <returns></returns>
    bool CheckMovementHorizontal()
    {
        if (inputRight && !inputLeft)
        {
            if (moveX == 0)
                moveX = 1;
            return true;
        }
        else if (inputLeft && !inputRight)
        {
            if (moveX == 0)
                moveX = -1;
            return true;
        }
        else
        {
            moveX = ChangeToValue.ChangeToFloat(moveX, 0f, deacceleration);
            return false;
        }
    }

    /// <summary>
    /// Checks for vertical input
    /// </summary>
    /// <returns></returns>
    bool CheckMovementVertical()
    {
        if (inputUp && !inputDown)
        {
            if (moveY == 0)
                moveY = 1;
            return true;
        }
        else if (inputDown && !inputUp)
        {
            if (moveY == 0)
                moveY = -1;
            return true;
        }
        else
        {
            moveY = ChangeToValue.ChangeToFloat(moveY, 0f, 0.04f);
            return false;
        }
    }

    public void SetMoveX(float speed)
    {
        this.moveX = speed;
    }
    public void SetMoveY(float speed)
    {
        this.moveY = speed;
    }

    public void SetInputRight(bool input)
    {
        this.inputRight = input;
    }

    public void SetInputLeft(bool input)
    {
        this.inputLeft = input;
    }
    public void SetInputUp(bool input)
    {
        this.inputUp = input;
    }

    public void SetInputDown(bool input)
    {
        this.inputDown = input;
    }
    public bool InputRight()
    {
        return inputRight;
    }
    public bool InputLeft()
    {
        return inputLeft;
    }
    public bool InputDown()
    {
        return inputDown;
    }
    public bool InputUp()
    {
        return inputUp;
    }

    public void NoInput()
    {

        inputLeft = inputRight = inputUp = inputDown = false;
    }

    public void PressJumpButton(bool pressed)
    {
        this.holdingJumpButton = pressed;
    }

    protected void ReduceHorizontalVelocity()
    {
        if (rb)
            rb.velocity = new Vector2(ChangeToValue.ChangeToFloat(rb.velocity.x, 0, deacceleration), rb.velocity.y);
    }

    /// <summary>
    /// If there is a horizontal input.
    /// </summary>
    /// <returns></returns>
    public bool IsMovingHorizontal()
    {
        return ((inputLeft || inputRight) && (!inputRight || !inputLeft));
    }

    /// <summary>
    /// If there is a vertical input.
    /// </summary>
    /// <returns></returns>
    public bool IsMovingVertical()
    {
        return ((inputDown || inputUp) && (!inputDown || !inputUp));
    }

    public bool GroundCheck()
    {
        List<GameObject> collisions = Check2DCollisions.CheckCollisionDown(boxCol, 5, 0.02f);
        foreach (var item in collisions)
        {
            if (LayerMask.LayerToName(item.layer) == "Ground")
                return true;
        }

        return false;
    }


    /// <summary>
    /// Used to move the player with no restrictions.
    /// </summary>
    /// <param name="endPos">Position to move the player</param>
    /// <param name="time">Time to move</param>
    /// <returns></returns>
    IEnumerator MovePlayer(Vector2 endPos, float time)
    {
        if (time < 0f)
            time = 0f;
        float i = 0.0f;
        float rate = 1.0f / time;
        Vector3 startPos = transform.position;
        rb.isKinematic = true;
        while (i < 1.0)
        {
            i += Time.deltaTime * rate;
            this.transform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
        rb.velocity = Vector2.zero;
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
    public void WallJump()
    {

        if (movementState != MovementState.WallClimbing || !rb)
            return;

        TransitionToNormal();
        //Only vertical input
        if (IsMovingVertical() && !IsMovingHorizontal())
        {
            rb.velocity = new Vector2(0f, 0f);
            rb.AddForce(new Vector2(wallJumpPower / 10f * (wallOnLeft ? 1f : -1f), wallJumpPower * ((inputUp ? 3.5f : -1f) / 5f)), ForceMode2D.Impulse);

        }
        //Only horizontal input
        else if (IsMovingHorizontal() && !IsMovingVertical())
        {
            rb.velocity = new Vector2(0f, 0f);
            StartCoroutine(player.Immobilize(0.1f));
            rb.AddForce(new Vector2(wallJumpPower * (inputRight ? wallOnLeft ? 1f / 3f : -2f / 5f : wallOnRight ? -1f / 3f : 2f / 5f),
                                    wallJumpPower * 3f / 4f), ForceMode2D.Impulse);

        }
        //Horizontal and vertical inputs
        else if (IsMovingHorizontal() && IsMovingVertical())
        {
            if (inputUp)
            {
                rb.velocity = new Vector2(0f, 0f);
                StartCoroutine(player.Immobilize(0.1f));
                rb.AddForce(new Vector2(wallJumpPower * (inputRight ? wallOnLeft ? 1f / 3f : -2f / 5f : wallOnRight ? -1f / 3f : 2f / 5f),
                                        wallJumpPower * 3f / 4f), ForceMode2D.Impulse);
            }
            else
            {
                StartCoroutine(player.Immobilize(0.1f));
                rb.velocity = new Vector2(0f, 0f);
                rb.AddForce(new Vector2(wallJumpPower / 10f * (wallOnLeft ? 1f : -1f), -wallJumpPower / 10f), ForceMode2D.Impulse);
            }
        }
        //No inputs
        else
        {
            rb.velocity = new Vector2(0f, 0f);
            rb.AddForce(new Vector2(wallJumpPower / 10f * (wallOnLeft ? 1f : -1f), -wallJumpPower / 10f), ForceMode2D.Impulse);
        }

        wallOnLeft = wallOnRight = false;
    }
    public void ClimbWall(float direction)
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
    bool TransitionToWallclimb()
    {
        //Debug.Log("Transition wallclimb");
        if (movementState == MovementState.WallClimbing || !EnableWallClimbing)
            return false;
        movementState = MovementState.WallClimbing;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        wallReleaseCount = 0f;
        moveX = 0f;
        moveY = 0f;

        return true;
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
        LayerMask ignore = LayerMask.GetMask("Characters", "Hitboxes", "Projectiles");
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
            if ((moveX < 0 && wallOnRight) || (moveX > 0 && wallOnLeft))
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

    bool CheckForRelease()
    {
        if (!EnableWallClimbing || movementState != MovementState.WallClimbing)
        {
            wallReleaseCount = 0f;
            return true;
        }
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
