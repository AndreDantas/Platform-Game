using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
public class JoystickController : MonoBehaviour
{

    public SimpleJoystick joystick;
    public PlayerMovements playerMovements;
    [Range(0f, 1f)]
    public float deadzoneMinX = 0.25f;
    [Range(0f, 1f)]
    public float deadzoneMaxX = 0.8f;
    [Range(0f, 1f)]
    public float deadzoneMinY = 0.25f;
    [Range(0f, 1f)]
    public float deadzoneMaxY = 0.8f;

    private void Awake()
    {
        if (!playerMovements)
            playerMovements = GameObject.FindObjectOfType<PlayerMovements>();
    }

    private void Update()
    {
        if (playerMovements != null)
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
                playerMovements.NoInput();
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
        if (!playerMovements)
            return;

        if (!playerMovements.canMove)
        {
            playerMovements.NoInput();
            return;
        }
        if (moveX == 0)
        {
            playerMovements.SetInputLeft(false);
            playerMovements.SetInputRight(false);
        }
        else if (moveX > 0)
        {
            playerMovements.SetInputLeft(false);
            playerMovements.SetInputRight(true);
        }
        else
        {
            playerMovements.SetInputLeft(true);
            playerMovements.SetInputRight(false);
        }

        if (moveY == 0)
        {
            playerMovements.SetInputUp(false);
            playerMovements.SetInputDown(false);
        }
        else if (moveY > 0)
        {
            playerMovements.SetInputUp(true);
            playerMovements.SetInputDown(false);
        }
        else
        {
            playerMovements.SetInputUp(false);
            playerMovements.SetInputDown(true);
        }

        playerMovements.SetMoveX(moveX);
        playerMovements.SetMoveY(moveY);
    }


}
