using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class FindTargetsInRadius : FindTargets
{

    public LayerMask ignore;

    public FindTargetsInRadius()
    {

    }

    public FindTargetsInRadius(float distance, Vector2 position, LayerMask ignore) : base(distance, position)
    {
        this.ignore = ignore;
    }
    public FindTargetsInRadius(float distance, Vector2 position) : base(distance, position)
    {

    }
    public FindTargetsInRadius(float distance) : base(distance)
    {

    }

    public override List<FightCharacter> AcquireTargets()
    {
        targets = new List<FightCharacter>();

        var hits = Physics2D.CircleCastAll(findOriginPosition, findDistance, Vector2.up, 0f, ~ignore);
        if (hits != null)
            for (int i = 0; i < hits.Length; i++)
            {
                var character = hits[i].collider.gameObject.GetComponent<FightCharacter>();
                if (character)
                    targets.Add(character);
            }

        return targets;
    }

}
