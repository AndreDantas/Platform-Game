using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
public class JoystickController : MonoBehaviour
{

    public SimpleJoystick joystick;
    public PlayerMovements player;
    [Range(0f, 1f)]
    public float deadzoneMinX = 0.25f;
    [Range(0f, 1f)]
    public float deadzoneMaxX = 0.8f;
    [Range(0f, 1f)]
    public float deadzoneMinY = 0.25f;
    [Range(0f, 1f)]
    public float deadzoneMaxY = 0.8f;
    private void Update()
    {
        if (player != null)
        {
            if (joystick != null ? joystick.IsTouching() : false)
            {
                float moveX = CnInputManager.GetAxis("Horizontal");
                moveX = CheckDeadzoneX(moveX);

                float moveY = CnInputManager.GetAxis("Vertical");
                moveY = CheckDeadzoneY(moveY);

                UpdatePlayerMovement(moveX, moveY);
            }
            else
            {
                player.NoInput();
            }
        }
    }

    float CheckDeadzoneX(float value)
    {
        if (Mathf.Abs(value) <= deadzoneMinX)
        {
            value = 0;
        }
        else if (Mathf.Abs(value) > deadzoneMaxX)
        {
            value = 1 * Mathf.Sign(value);
        }
        return value;
    }
    float CheckDeadzoneY(float value)
    {
        if (Mathf.Abs(value) <= deadzoneMinY)
        {
            value = 0;
        }
        else if (Mathf.Abs(value) > deadzoneMaxY)
        {
            value = 1 * Mathf.Sign(value);
        }
        return value;
    }
    void UpdatePlayerMovement(float moveX, float moveY)
    {
        if (!player)
            return;

        if (!player.canMove)
        {
            player.NoInput();
            return;
        }
        if (moveX == 0)
        {
            player.SetInputLeft(false);
            player.SetInputRight(false);
        }
        else if (moveX > 0)
        {
            player.SetInputLeft(false);
            player.SetInputRight(true);
        }
        else
        {
            player.SetInputLeft(true);
            player.SetInputRight(false);
        }

        if (moveY == 0)
        {
            player.SetInputUp(false);
            player.SetInputDown(false);
        }
        else if (moveY > 0)
        {
            player.SetInputUp(true);
            player.SetInputDown(false);
        }
        else
        {
            player.SetInputUp(false);
            player.SetInputDown(true);
        }

        player.SetMoveX(moveX);
        player.SetMoveY(moveY);
    }


}
