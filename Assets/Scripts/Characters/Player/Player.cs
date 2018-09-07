using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Player : FightCharacter
{
    [InlineEditor(InlineEditorModes.LargePreview)]
    public GameObject bulletPrefab;
    public PlayerMovements movements;
    public float bulletForce = 16f;
    protected BoxCollider2D col;
    Vector2 movementDirection = Vector2.right;

    /// <summary>
    /// Player input reference
    /// </summary>
    public Inputs input = new Inputs(false);
    protected bool joystickActive = false;
    public void JoystickActive(bool active)
    {
        joystickActive = active;
    }
    [BoxGroup("Movement"), SerializeField, ReadOnly] float moveX;
    [BoxGroup("Movement"), SerializeField, ReadOnly] float moveY;
    [BoxGroup("Movement")] public float deacceleration = 0.1f;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<BoxCollider2D>();
        movements = GetComponent<PlayerMovements>();
        faction.value = Faction.Type.Player;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GetPlayerDirection();

        if (Input.GetButtonDown("Fire"))
        {
            Shoot();
        }

        //-------------------- TEST ---------------------\\
        if (canMove)
        {
            if (!joystickActive)
            {
                moveX = Input.GetAxisRaw("Horizontal");
                if (moveX != 0)
                {
                    input.Right = moveX > 0;
                    input.Left = moveX < 0;

                }
                else
                {
                    input.Right = input.Left = false;
                }

                moveY = Input.GetAxisRaw("Vertical");
                if (moveY != 0)
                {
                    input.Up = moveY > 0;
                    input.Down = moveY < 0;
                }
                else
                {
                    input.Down = input.Up = false;
                }
            }
            if (Input.GetButtonDown("Jump"))
            {
                movements.PressJumpButton(true);
                movements.Jump();
            }
            else if (Input.GetButtonUp("Jump"))
            {
                movements.PressJumpButton(false);
            }
        }
        else
        {
            NoInput();
        }

    }

    public void Shoot()
    {
        //TEST
        if (bulletPrefab)
        {

            GameObject b = Instantiate(bulletPrefab);
            b.transform.position = transform.position + new Vector3(0.25f * movementDirection.x, 0, 0);
            b.GetComponent<Projectile>().Shoot(new Vector2(movementDirection.x, 0f), bulletForce, faction.value, attack);
        }
    }

    //TEST
    void GetPlayerDirection()
    {
        if (movements)
        {
            if (movements.movementState == MovementState.WallClimbing)
            {
                if (movements.WallOnLeft() && !movements.WallOnRight())
                {
                    movementDirection = Vector2.right;
                }
                else
                {
                    movementDirection = Vector2.left;
                }
            }
            else
            {
                if (InputRight() && !InputLeft())
                {
                    movementDirection = Vector2.right;
                }
                else if (!InputRight() && InputLeft())
                {
                    movementDirection = Vector2.left;
                }
            }
        }

    }
    public override IEnumerator Immobilize(float duration = 1)
    {
        NoInput();
        yield return base.Immobilize(duration);
    }

    public override void OnDeath()
    {

    }
    public override float ModifyDamage(float damage)
    {
        return damage;
    }
    public override void AfterDamage(float damage)
    {

    }
    public override void BeforeDamage(float damage)
    {

    }

    public override Collider2D GetCollider()
    {
        return col;
    }

    /// <summary>
    /// Checks for horizontal input
    /// </summary>
    /// <returns></returns>
    public bool CheckMovementHorizontal()
    {
        if (input.Right && !input.Left)
        {
            if (moveX == 0)
                moveX = 1;
            return true;
        }
        else if (input.Left && !input.Right)
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
    public bool CheckMovementVertical()
    {
        if (input.Up && !input.Down)
        {
            if (moveY == 0)
                moveY = 1;
            return true;
        }
        else if (input.Down && !input.Up)
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
    /// <summary>
    /// If there is a horizontal input.
    /// </summary>
    /// <returns></returns>
    public bool IsMovingHorizontal()
    {
        return ((input.Left || input.Right) && (!input.Right || !input.Left));
    }

    public void ReduceHorizontalVelocity()
    {
        if (rb)
            rb.velocity = new Vector2(ChangeToValue.ChangeToFloat(rb.velocity.x, 0, deacceleration), rb.velocity.y);
    }

    /// <summary>
    /// If there is a vertical input.
    /// </summary>
    /// <returns></returns>
    public bool IsMovingVertical()
    {
        return ((input.Down || input.Up) && (!input.Down || !input.Up));
    }

    public void SetMoveX(float x)
    {
        moveX = x;
    }
    public void SetMoveY(float y)
    {
        moveY = y;
    }

    public float GetMoveX()
    {
        return moveX;
    }
    public float GetMoveY()
    {
        return moveY;
    }

    public void SetInputRight(bool input)
    {
        this.input.Right = input;
    }

    public void SetInputLeft(bool input)
    {
        this.input.Left = input;
    }
    public void SetInputUp(bool input)
    {
        this.input.Up = input;
    }

    public void SetInputDown(bool input)
    {
        this.input.Down = input;
    }
    public bool InputRight()
    {
        return input.Right;
    }
    public bool InputLeft()
    {
        return input.Left;
    }
    public bool InputDown()
    {
        return input.Down;
    }
    public bool InputUp()
    {
        return input.Up;
    }

    public bool HasInput()
    {
        return input.Left || input.Right || input.Down || input.Up;
    }

    public void NoInput()
    {

        input.Left = input.Right = input.Up = input.Down = false;
    }

}
