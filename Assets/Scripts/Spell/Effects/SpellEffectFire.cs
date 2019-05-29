using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectFire : SpellEffect
{
    private static readonly Color particleColor = new Color(0.97f, 0.58f, 0.20f);
    private static readonly Color emissionColor = new Color(0.48f, 0.29f, 0.10f);
    private const float maxDamage = 25f;
    private const float minDamage = 10f;


    public SpellEffectFire() => manaCost = 10f;

    public override void Start(SpellScript self)
    {
        self.mm.startColor = particleColor;

        // Create Light Object
        //Light boltLight = self.gameObject.AddComponent<Light>() as Light;
        //boltLight.color = Color.yellow;
        //boltLight.intensity = 1.0f;
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        // Damage enemies
        // First, find the object with the stat script
        
        StatScript ss = other.GetComponentInParent<StatScript>();
        if (ss != null)
            ss.DamageHealth(Random.Range(minDamage, maxDamage) * self.effectMagnitudeScale, StatScript.DamageType.DTFire);

        // We don't care if we continue or not. That's for a modifier or shape to decide
        return false;
    }
}
