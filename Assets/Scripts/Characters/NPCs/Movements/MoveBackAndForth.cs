using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class MoveBackAndForth : Movement
{

    [System.Serializable]
    protected enum MovingWaitingState
    {
        Moving,
        Waiting
    }

    public float currentSpeed;
    public Timer deadEndWait = new Timer(2f);

    protected MovingWaitingState movingState = MovingWaitingState.Moving;
    protected Directions movingDir = Directions.Right;
    protected BoxCollider2D col;
    protected Rigidbody2D rb;
    protected override void Start()
    {
        base.Start();
        if (character)
        {
            col = (BoxCollider2D)character.GetCollider();
            rb = character.GetRigidbody2D();
        }
    }
    public override void Move()
    {
        if (!character)
            return;
        if (!character.IsGrounded())
            return;


        if (movingDir == Directions.Right)
        {
            if (movingState == MovingWaitingState.Moving)
            {
                if (Check2DCollisions.CheckForFallRight(col, layerMask: ~LayerMask.GetMask(Character.IgnoreLayers)) ||
                    Check2DCollisions.CheckCollisionRight(col, layerMask: ~LayerMask.GetMask(Character.IgnoreLayers)).Count > 0)
                {
                    movingState = MovingWaitingState.Waiting;
                    rb.velocity = new Vector2(currentSpeed = 0f, rb.velocity.y);
                    deadEndWait.Reset();
                }
                else
                {
                    currentSpeed = ChangeToValue.ChangeToFloat(currentSpeed, speed, 0.05f);
                    if (rb)
                        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
                }
            }
            else
            {
                deadEndWait.Update();
                if (deadEndWait.isFinished)
                {
                    movingDir = Directions.Left;
                    movingState = MovingWaitingState.Moving;
                    deadEndWait.Reset();
                }
            }
        }
        else
        {
            if (movingState == MovingWaitingState.Moving)
            {
                if (Check2DCollisions.CheckForFallLeft(col, layerMask: ~LayerMask.GetMask(Character.IgnoreLayers)) ||
                    Check2DCollisions.CheckCollisionLeft(col, layerMask: ~LayerMask.GetMask(Character.IgnoreLayers)).Count > 0)
                {
                    movingState = MovingWaitingState.Waiting;
                    rb.velocity = new Vector2(currentSpeed = 0f, rb.velocity.y);
                    deadEndWait.Reset();
                }
                else
                {
                    currentSpeed = ChangeToValue.ChangeToFloat(currentSpeed, -speed, 0.05f);
                    if (rb)
                        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
                }
            }
            else
            {
                deadEndWait.Update();
                if (deadEndWait.isFinished)
                {
                    movingDir = Directions.Right;
                    movingState = MovingWaitingState.Moving;
                    deadEndWait.Reset();
                }
            }
        }
    }
}
