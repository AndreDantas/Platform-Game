using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : FightCharacter
{
    [System.Serializable]
    public enum EnemyState
    {

        Combat,
        Normal
    }
    [ReadOnly, PropertyOrder(-1)]
    public EnemyState state = EnemyState.Normal;

    protected BoxCollider2D col;
    public float collisionDamage = 1f;
    [Range(0, 30f)]
    public float collisionKnockbackForce = 8f;
    public FindTargetsInRadius f = new FindTargetsInRadius(5f);
    public Movement movement;
    protected override void Awake()
    {
        state = EnemyState.Normal;
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        faction.value = Faction.Type.Enemy;
        invincibleTime.Time = 0.1f;
        movement = GetComponent<Movement>();

    }

    protected virtual void Update()
    {

        if (rb)
        {
            if (CheckForGround())
                rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

        switch (state)
        {

            case EnemyState.Combat:
                f.findOriginPosition = transform.position;
                f.AcquireTargets();
                break;
            case EnemyState.Normal:
                movement?.Move();
                break;
            default:
                break;
        }

    }

    public override void OnDeath()
    {
        Destroy(gameObject);
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
        c.Damage(attack, new Vector2(Mathf.Sign(c.gameObject.transform.position.x - transform.position.x), 2) * collisionKnockbackForce, 0.5f);
    }

    public virtual void Movement()
    {

    }

    public override IEnumerator Immobilize(float duration = 1)
    {
        yield return base.Immobilize(duration);
        state = EnemyState.Combat;
    }

    public virtual bool CheckForGround()
    {
        if (col == null)
            return grounded = false;
        return grounded = Check2DCollisions.CheckCollisionDown(col).Count > 0;
    }


    public override Collider2D GetCollider()
    {
        return col;
    }
}
