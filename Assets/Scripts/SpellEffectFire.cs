using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectFire : SpellEffect
{
    public override void Start(GameObject self)
    {
        Debug.Log("I am in the Fire spell effect start");
        // Create particle effect
        ParticleSystem ps = self.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule mm = ps.main;
        ParticleSystem.MinMaxGradient mmg = mm.startColor;
        Color fireColor = mmg.color;
        fireColor.r = 255;
        fireColor.g = 147;
        fireColor.b = 52;
        mmg.color = fireColor;
        mm.startColor = mmg;
    }

    public override void Trigger(GameObject self, GameObject other)
    {
        // Damage enemies
    }
}
