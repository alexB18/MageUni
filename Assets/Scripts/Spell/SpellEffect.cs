using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellEffect
{
    public float manaCost = 10f;
    public abstract void Start(GameObject self);
    public abstract void Trigger(GameObject self, GameObject other);
}
