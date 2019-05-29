using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectHaste : SpellEffect
{
    private static readonly Color particleColor = new Color(0.94f, 0.94f, 0.20f);
    private static readonly Color emissionColor = new Color(0.57f, 0.57f, 0.10f);
    private const float maxMagnitude = 1.0f;
    private const float minMagnitude = 0.5f;
    private const float maxTime = 20f;
    private const float minTime = 10f;


    public SpellEffectHaste() => manaCost = 15f;

    public override void Start(SpellScript self)
    {
        self.mm.startColor = particleColor;
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        StatScript ss = other.GetComponentInParent<StatScript>();
        if (ss != null)
        {
            ss.AddSpeedProc(Random.Range(minTime, maxTime) * self.effectMagnitudeScale,
                            Random.Range(minMagnitude, maxMagnitude) * self.effectMagnitudeScale);
        }

        // We don't care if we continue or not. That's for a modifier or shape to decide
        return false;
    }
}
