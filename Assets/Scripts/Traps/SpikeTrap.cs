using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class SpikeTrap : Trap
{
    [Range(0, 30f)]
    public float collisionKnockbackForce = 8f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!active)
            return;
        var c = collision.gameObject.GetComponent<FightCharacter>();

        if (c)
        {
            c.Damage(damage, new Vector2(Mathf.Sign(c.gameObject.transform.position.x - transform.position.x), 2) * collisionKnockbackForce, 0.5f);
        }
    }
}
