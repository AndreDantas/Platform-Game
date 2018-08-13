using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Faction faction;
    [SerializeField]
    private float _maxHealth;
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
    [SerializeField]
    private float _currentHealth;
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

    public virtual void Damage(float damage)
    {
        BeforeDamage(damage); // Before damage is dealt.

        damage = ModifyDamage(damage); // Change damage based on effects.

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath(); // Death of character.
        }

        AfterDamage(damage); // After damage is dealt.
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
