using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Character : MonoBehaviour
{
    public Faction faction;
    public Hitbox hitbox;
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
    [SerializeField, ReadOnly]
    protected bool stunned = false;
    [SerializeField, ReadOnly]
    protected bool invincible = false;
    public float invincibleTime = 2f;

    protected virtual void Awake()
    {
        if (!rb)
            rb = GetComponent<Rigidbody2D>();
        if (!hitbox)
            hitbox = GetComponentInChildren<Hitbox>();
        if (hitbox)
            hitbox.parent = this;
    }

    public virtual void Damage(float damage)
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
        StartCoroutine(Invincible());
    }

    public virtual void Knockback(Vector2 force)
    {
        if (!rb)
            return;

        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);


    }

    public virtual void Stun(float duration)
    {
        StartCoroutine(Immobilize(duration));
    }

    public virtual IEnumerator Invincible()
    {
        if (invincible)
            yield break;
        invincible = true;
        if (hitbox)
            hitbox.hitArea.enabled = false;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;
        if (hitbox)
            hitbox.hitArea.enabled = true;
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
        yield return new WaitForSeconds(duration);
        SetCanMove(true);
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

}
