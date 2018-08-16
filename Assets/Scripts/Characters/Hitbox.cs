using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour
{

    public Character parent;
    public Collider2D hitArea;

    protected virtual void OnEnable()
    {
        if (!hitArea)
            hitArea = GetComponent<Collider2D>();
        if (hitArea)
            hitArea.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Hitboxes");
        if (!parent)
            parent = GetComponentInParent<Character>();
        if (parent)
            name = parent.name + "'s hitbox";
    }

}
