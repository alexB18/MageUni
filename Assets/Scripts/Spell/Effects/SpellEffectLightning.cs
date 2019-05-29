using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectLightning : SpellEffect
{
    private static readonly Color particleColor = new Color(0.94f, 0.86f, 0.39f);
    private static readonly Color emissionColor = new Color(0.47f, 0.43f, 0.19f);
    private const float maxDamage = 25f;
    private const float minDamage = 10f;
    private const float maxTime = 5f;
    private const float minTime = 2f;
    private const float procChance = 0.20f;


    public SpellEffectLightning() => manaCost = 15f;

    public override void Start(SpellScript self)
    {
        self.mm.startColor = particleColor;
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        // Damage enemies
        // First, find the object with the stat script

        StatScript ss = other.GetComponentInParent<StatScript>();
        if (ss != null)
        {
            ss.DamageHealth(Random.Range(minDamage, maxDamage) * self.effectMagnitudeScale, StatScript.DamageType.DTElectric);
            float procRoll = Random.value;
            if (procRoll <= procChance)
            {
                ss.AddStunProc(Random.Range(minTime, maxTime) * self.effectMagnitudeScale);
            }
        }

        // We don't care if we continue or not. That's for a modifier or shape to decide
        return false;
    }
}
