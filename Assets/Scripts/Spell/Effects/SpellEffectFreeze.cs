using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectFreeze : SpellEffect
{
    private static readonly Color particleColor = new Color(0f, 0.54f, 1.0f);
    private static readonly Color emissionColor = new Color(0f, 0.26f, 0.5f);
    private const float procChance = 0.90f;


    public SpellEffectFreeze() => manaCost = 15f;

    public override void Start(SpellScript self)
    {
        self.mm.startColor = particleColor;
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        // stun enemies
        // First, find the object with the stat script

        StatScript ss = other.GetComponentInParent<StatScript>();
        if (ss != null)
        {
            float procRoll = Random.value;
            if (procRoll <= procChance)
            {
                ss.AddFreezeProc();
            }
        }

        // We don't care if we continue or not. That's for a modifier or shape to decide
        return false;
    }
}
