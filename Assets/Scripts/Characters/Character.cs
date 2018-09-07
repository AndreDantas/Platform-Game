using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Character : MonoBehaviour
{
    public static string[] IgnoreLayers = { "Characters", "Hitboxes", "Projectiles", "Ignore" };

    protected Rigidbody2D rb;
    [SerializeField, HideInInspector]
    protected float _maxHealth = 10f;
    [ShowInInspector]
    public float maxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            if (value < 0)
                _maxHealth = 0;
            else
                _maxHealth = value;
        }
    }
    [SerializeField, HideInInspector]
    protected float _currentHealth = 10f;
    [ShowInInspector]
    public float currentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            if (value < 0)
                _currentHealth = 0;
            else if (value > maxHealth)
                _currentHealth = maxHealth;
            else
                _currentHealth = value;
        }
    }

    public float attack = 1f;
    [SerializeField]
    protected bool canMove = true;
    [SerializeField]
    protected bool grounded = false;

    public Timer invincibleTime = new Timer(2f);
    [ShowInInspector, ReadOnly]
    public bool invincible { get; private set; }
    protected bool stunned;
    protected virtual void Awake()
    {
        if (!rb)
            rb = GetComponent<Rigidbody2D>();

    }

    public virtual void Damage(float damage)
    {
        Damage(damage, Vector2.zero, 0f);

    }

    public virtual void Damage(float damage, Vector2 force, float stunTime)
    {
        if (invincible)
            return;
        BeforeDamage(damage); // Before damage is dealt.

        damage = ModifyDamage(damage); // Change damage based on effects.

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath(); // Death of character.
        }

        AfterDamage(damage); // After damage is dealt.
        StartCoroutine(Invincible(invincibleTime.Time + stunTime));
        Knockback(force);
        Stun(stunTime);
    }

    public virtual void Knockback(Vector2 force, bool invincibleBlock = false)
    {
        if (!rb || (invincibleBlock && invincible))
            return;

        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);


    }

    public virtual void Stun(float duration)
    {
        if (!stunned)
            StartCoroutine(Immobilize(duration));
    }

    public virtual IEnumerator Invincible(float time)
    {
        if (invincible)
            yield break;
        invincible = true;

        while (!invincibleTime.isFinished)
        {
            invincibleTime.Update();
            yield return null;
        }
        invincible = false;

        invincibleTime.Reset();
    }

    /// <summary>
    /// Makes character unable to move for X seconds.
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public virtual IEnumerator Immobilize(float duration = 1f)
    {

        if (duration < 0)
            duration = 0;
        SetCanMove(false);
        stunned = true;
        yield return new WaitForSeconds(duration);
        SetCanMove(true);
        stunned = false;
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public bool CanMove()
    {
        return canMove;
    }

    public abstract void OnDeath();
    public virtual void BeforeDamage(float damage)
    {

    }

    public virtual float ModifyDamage(float damage)
    {
        return damage;
    }

    public virtual void AfterDamage(float damage)
    {

    }

    public bool IsGrounded()
    {
        return grounded;
    }

    public abstract Collider2D GetCollider();
    public Rigidbody2D GetRigidbody2D()
    {
        return rb;
    }
}
