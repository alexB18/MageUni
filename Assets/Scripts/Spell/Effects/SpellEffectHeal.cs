using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectHeal : SpellEffect
{
    private static readonly Color particleColor = new Color(0.40f, 0.80f, 1.0f);
    private static readonly Color emissionColor = new Color(0.20f, 0.40f, 0.5f);
    private const float maxHeal = 35f;
    private const float minHeal = 20f;

    public SpellEffectHeal() => manaCost = 20f;

    // Start is called before the first frame update
    public override void Start(SpellScript self)
    {
        self.mm.startColor = particleColor;
    }
    
    public override bool Trigger(SpellScript self, GameObject other)
    {
        StatScript ss = other.GetComponentInParent<StatScript>();
        if (ss != null)
            ss.RestoreHealth(Random.Range(minHeal, maxHeal) * self.effectMagnitudeScale);

        return false;
    }
}
