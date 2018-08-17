using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : Character
{
    protected BoxCollider2D col;
    public float collisionDamage = 1f;
    public float collisionKnockbackForce = 20f;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        faction.value = Faction.Type.Enemy;
    }

    protected virtual void Update()
    {

        if (rb)
        {
            if (CheckForGround())
                rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

        Movement();
    }

    public override void OnDeath()
    {

    }
    public override void BeforeDamage(float damage)
    {

    }
    public override float ModifyDamage(float damage)
    {
        return damage;
    }
    public override void AfterDamage(float damage)
    {

    }

    public virtual void CollisionDamage(Character c)
    {
        c.Damage(attack);
        c.Knockback(new Vector2(Mathf.Sign(c.gameObject.transform.position.x - transform.position.x), 2) * collisionKnockbackForce);
        c.Stun(0.5f);
    }

    public virtual void Movement()
    {

    }

    public virtual bool CheckForGround()
    {
        if (col == null)
            return false;
        return Check2DCollisions.CheckCollisionDown(col).Count > 0;
    }

}
