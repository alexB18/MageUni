using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectFire : SpellEffect
{
    private const float fColorR = 247f / 255f;
    private const float fColorG = 147 / 255f;
    private const float fColorB = 52 / 255f;
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
    }
}
