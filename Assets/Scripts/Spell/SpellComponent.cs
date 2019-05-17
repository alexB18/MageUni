using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellComponent
{
    public float manaCost = 5;
    public float manaMultiplier = 1.0f;

    public virtual bool IsShape => false;

    public abstract AllSpellsAndGlyphs.SpellComponentEnum ComponentType { get; }

    /**
     * Run at the start of the spell. This can be used to initialise a new collider
     * or trigger. It can also be used to set the locomotion
     */
    public virtual void Start(SpellScript self) { }

    /**
     * Used to locomote if needed
     * others can be used for tracking effects
     */
    public virtual void Update(SpellScript self, List<GameObject> others = null) { }
    /**
     * When there is a trigger, a spell shape might change or do something else
     * For example, when we hit someone with a fireball, the spell should burst
     * first.
     * For a wave, the spell should affect the target and keep going
     * 
     * Return value is a boolean for whether or not we destroy ourself
     */
    public virtual bool Trigger(SpellScript self, GameObject other) { return false; }
}
