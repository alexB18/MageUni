using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectEnrage : SpellEffect
{
    private static readonly Color particleColor = new Color(0.80f, 0.20f, 0.20f);
    private const float maxTime = 30f;
    private const float minTime = 15f;
    private const float procChance = 0.90f;


    public SpellEffectEnrage() => manaCost = 15f;

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
            if (procRoll <= procChance)
            {
                ss.AddEnrageProc(Random.Range(minTime, maxTime) * self.effectMagnitudeScale);
            }
        }

        // We don't care if we continue or not. That's for a modifier or shape to decide
        return false;
    }
}
