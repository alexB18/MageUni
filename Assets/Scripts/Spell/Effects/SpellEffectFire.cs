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
        
        // Create Light Object
        Light boltLight = self.gameObject.AddComponent<Light>() as Light;
        boltLight.color = Color.yellow;
        boltLight.intensity = 1.0f;
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        // Damage enemies
        // First, find the object with the stat script
        
        StatScript ss = other.GetComponentInParent<StatScript>();
        if (ss != null)
            ss.DamageHealth(Random.Range(minDamage, maxDamage) * self.effectMagnitudeScale);

        // We don't care if we continue or not. That's for a modifier or shape to decide
        return false;
    }
}
