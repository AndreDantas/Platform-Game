using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public abstract class FindTargets
{
    public float findDistance = 5f;
    public Vector2 findOriginPosition;
    protected List<FightCharacter> targets = new List<FightCharacter>();

    [ShowInInspector]
    public bool HasTargets
    {
        get { return targets?.Count > 0; }
    }

    public FindTargets()
    {

    }
    public FindTargets(float findDistance, Vector2 position)
    {
        this.findDistance = findDistance;
        this.findOriginPosition = position;
    }

    protected FindTargets(float findDistance)
    {
        this.findDistance = findDistance;
    }

    public abstract List<FightCharacter> AcquireTargets();

    public List<FightCharacter> GetTargets()
    {
        return targets;
    }


}
