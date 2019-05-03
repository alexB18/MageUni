using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    public float spellTime = 10f;
    public class Spell
    {
        public List<SpellEffect> effects = new List<SpellEffect>();
        public SpellShape shape;
        public float ManaCost()
        {
            float cost = 0f;
            foreach (SpellEffect e in effects)
                cost += e.manaCost;
            cost = shape.manaCost + shape.manaMultiplier * cost;
            return cost;
        }
    }
    [HideInInspector] public Spell spell;

    // Start is called before the first frame update
    void Start()
    {
        foreach (SpellEffect effect in spell.effects)
            effect.Start(gameObject);
        spell.shape.Start(gameObject);
        StartCoroutine("Decay");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure we don't prematurely break
        if (other.CompareTag("Player") || other.CompareTag("Ground"))
            return;
        // If this returns true, destroy ourself
        if (spell.shape.Trigger(gameObject, other.gameObject, spell.effects))
            Destroy(gameObject);
    }

    IEnumerator Decay()
    {
        yield return new WaitForSeconds(spellTime);
        Destroy(gameObject);
    }
}
