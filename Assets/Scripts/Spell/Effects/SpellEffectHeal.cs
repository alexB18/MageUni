using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectHeal : SpellEffect
{
    private const float fColorR = 102 / 255f;
    private const float fColorG = 204 / 255f;
    private const float fColorB = 255 / 255f;
    private const float maxHeal = 35f;
    private const float minHeal = 20f;

    public SpellEffectHeal() => manaCost = 20f;

    // Start is called before the first frame update
    public override void Start(SpellScript self)
    {
        ParticleSystem ps = self.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule mm = ps.main;
        mm.startColor = new Color(fColorR, fColorG, fColorB);
    }
    
    public override bool Trigger(SpellScript self, GameObject other)
    {
        StatScript ss = other.GetComponentInParent<StatScript>();
        if (ss != null)
            ss.RestoreHealth(Random.Range(minHeal, maxHeal) * self.effectMagnitudeScale);

        return false;
    }
}
