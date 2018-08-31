using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public abstract class FightCharacter : Character
{
    public Faction faction;
    public Hitbox hitbox;
    protected override void Awake()
    {
        base.Awake();
        if (!hitbox)
            hitbox = GetComponentInChildren<Hitbox>();
        if (hitbox)
            hitbox.parent = this;
    }

    public override IEnumerator Invincible(float time)
    {
        if (hitbox)
            hitbox.hitArea.enabled = false;
        yield return base.Invincible(time);
        if (hitbox)
            hitbox.hitArea.enabled = true;
    }
}
