using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectLightning : SpellEffect
{
    private const float fColorR = 240f / 255f;
    private const float fColorG = 220f / 255f;
    private const float fColorB = 100f / 255f;
    private const float maxDamage = 25f;
    private const float minDamage = 10f;
    private const float maxTime = 5f;
    private const float minTime = 2f;
    private const float procChance = 0.20f;


    public SpellEffectLightning() => manaCost = 15f;

    public override void Start(SpellScript self)
    {
        //Debug.Log("I am in the Fire spell effect start");
        // Create particle effect
        ParticleSystem ps = self.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule mm = ps.main;
        mm.startColor = new Color(fColorR, fColorG, fColorB);
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        // Damage enemies
        // First, find the object with the stat script

        StatScript ss = other.GetComponentInParent<StatScript>();
        if (ss != null)
        {
            ss.DamageHealth(Random.Range(minDamage, maxDamage) * self.effectMagnitudeScale, StatScript.DamageType.DTElectric);
            float procRoll = Random.value - procChance;
            if (procRoll <= 0f)
            {
                ss.AddStunProc(Random.Range(minTime, maxTime) * self.effectMagnitudeScale);
            }
        }

        // We don't care if we continue or not. That's for a modifier or shape to decide
        return false;
    }
}
