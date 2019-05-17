using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellModifierSpeedIncrease : SpellModifier
{
    public override void Start(SpellScript hostSpell)
    {
        hostSpell.GetComponent<Rigidbody>().velocity *= 2.0f;
    }
}
