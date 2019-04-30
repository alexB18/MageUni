using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellShape
{
    /**
     * Run at the start of the spell. This can be used to initialise a new collider
     * or trigger. It can also be used to set the locomotion
     */
    public abstract void Start(GameObject self);

    /**
     * Used to locomote if needed
     * others can be used for tracking effects
     */
    public abstract void Run(GameObject self, List<GameObject> others = null);
    /**
     * When there is a trigger, a spell shape might change or do something else
     * For example, when we hit someone with a fireball, the spell should burst
     * first.
     * For a wave, the spell should affect the target and keep going
     * 
     * Return value is a boolean for whether or not we destroy ourself
     */
    public abstract bool Trigger(GameObject self, GameObject other, List<SpellEffect> effects);
}
