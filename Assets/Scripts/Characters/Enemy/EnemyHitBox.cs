using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[RequireComponent(typeof(Collider2D))]
[DisallowMultipleComponent]
public class EnemyHitBox : Hitbox
{

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (!parent ? true : !(parent is Enemy))
            return;

        var hitbox = collision.gameObject.GetComponent<Hitbox>();
        if (hitbox)
        {
            if (hitbox.parent != null ? hitbox.parent.faction.value != parent.faction.value : false)
            {
                ((Enemy)parent).CollisionDamage(hitbox.parent);
            }
        }
    }


}
