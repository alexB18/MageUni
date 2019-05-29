using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectStun : SpellEffect
{
    private static readonly Color particleColor = new Color(0.94f, 0.94f, 0.20f);
    private static readonly Color emissionColor = new Color(0.57f, 0.57f, 0.10f);
    private const float maxTime = 10f;
    private const float minTime = 3f;
    private const float procChance = 0.90f;


    public SpellEffectStun() => manaCost = 15f;

    public override void Start(SpellScript self)
    {
        self.mm.startColor = particleColor;
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        // stun enemies
        // First, find the object with the stat script

        StatScript ss = other.GetComponentInParent<StatScript>();
        if (ss != null)
        {
            float procRoll = Random.value;
            if(procRoll <= procChance)
            {
                ss.AddStunProc(Random.Range(minTime, maxTime) * self.effectMagnitudeScale);
            }
        }

        // We don't care if we continue or not. That's for a modifier or shape to decide
        return false;
    }
}
