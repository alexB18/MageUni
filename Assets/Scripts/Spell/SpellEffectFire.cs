using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectFire : SpellEffect
{
    private const float fColorR = 247f / 255f;
    private const float fColorG = 147 / 255f;
    private const float fColorB = 52 / 255f;
    private float maxDamage = 25f;
    private float minDamage = 10f;
    private float spread = 2f;
    public float manaCost = 10f;
    public override void Start(GameObject self)
    {
        //Debug.Log("I am in the Fire spell effect start");
        // Create particle effect
        ParticleSystem ps = self.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule mm = ps.main;
        mm.startColor = new Color(fColorR, fColorG, fColorB);
    }

    public override void Trigger(GameObject self, GameObject other)
    {
        // Damage enemies
        // First, find the object with the health script
        
        HealthScript hs = other.GetComponentInParent<HealthScript>();
        if (hs != null)
            hs.DamageHealth(maxDamage);
    }
}
