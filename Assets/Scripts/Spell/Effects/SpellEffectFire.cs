using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectFire : SpellEffect
{
    private const float fColorR = 247f / 255f;
    private const float fColorG = 147 / 255f;
    private const float fColorB = 52 / 255f;
    private const float maxDamage = 25f;
    private const float minDamage = 10f;

    public SpellEffectFire() => manaCost = 10f;

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
        // First, find the object with the health script
        
        StatScript hs = other.GetComponentInParent<StatScript>();
        if (hs != null)
            hs.DamageHealth(Random.Range(minDamage, maxDamage));

        // We don't care if we continue or not. That's for a modifier or shape to decide
        return false;
    }
}
