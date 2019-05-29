using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectSlow : SpellEffect
{
    private static readonly Color particleColor = new Color(0.94f, 0.94f, 0.20f);
    private static readonly Color emissionColor = new Color(0.57f, 0.57f, 0.10f);
    private const float maxMagnitude = -0.75f;
    private const float minMagnitude =-0.25f;
    private const float maxTime = 15f;
    private const float minTime = 7f;
    private const float procChance = 0.90f;


    public SpellEffectSlow() => manaCost = 15f;

    public override void Start(SpellScript self)
    {
        self.mm.startColor = particleColor;
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        StatScript ss = other.GetComponentInParent<StatScript>();
        if (ss != null)
        {
            float procRoll = Random.value;
            if (procRoll <= procChance)
            {
                ss.AddSpeedProc(Random.Range(minTime, maxTime) * self.effectMagnitudeScale,
                                Random.Range(minMagnitude, maxMagnitude) * self.effectMagnitudeScale);
            }
        }

        // We don't care if we continue or not. That's for a modifier or shape to decide
        return false;
    }
}
