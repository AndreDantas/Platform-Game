using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Player : Character
{
    [InlineEditor(InlineEditorModes.LargePreview)]
    public GameObject bulletPrefab;
    public PlayerMovements movements;
    public float bulletForce = 16f;
    Vector2 movementDirection = Vector2.right;
    // Use this for initialization
    void Awake()
    {
        movements = GetComponent<PlayerMovements>();
        faction.value = Faction.Type.Player;
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerDirection();
        if (Input.GetKeyDown(KeyCode.X))
        {
            Shoot();
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
                if (movements.InputRight() && !movements.InputLeft())
                {
                    movementDirection = Vector2.right;
                }
                else if (!movements.InputRight() && movements.InputLeft())
                {
                    movementDirection = Vector2.left;
                }
            }
        }

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
}
