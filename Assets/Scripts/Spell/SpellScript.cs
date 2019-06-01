using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    public float effectMagnitudeScale = 1.0f;
    public float spellTime = 10f;
    public ParticleSystem.MainModule mm;
    public GameObject parent;
    public MeshFilter mf;

    public class Spell
    {
        public string name = "Spell";
        public List<SpellComponent> components = new List<SpellComponent>();
        public SpellShape shape;
        public float ManaCost()
        {
            float cost = 0f;
            foreach (var e in components)
                cost += e.manaCost;
            cost = shape.manaCost + shape.manaMultiplier * cost;
            return cost;
        }
    }
    [HideInInspector] public Spell spell;

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        mm = ps.main;

        gameObject.name = spell.name;
        spell.shape.Start(this);
        foreach (var spellComponent in spell.components)
            spellComponent.Start(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure we don't prematurely break
        if (other.CompareTag("Ground"))
            return;

        // If this returns true, we keep going. Otherwise, split off and destroy ourself
        bool continueSpell = spell.shape.Trigger(this, other.gameObject);
        foreach (var spellComponent in spell.components)
            continueSpell |= spellComponent.Trigger(this, other.gameObject);

        if (!continueSpell)
            spell.shape.DestroyAndStartChildren(gameObject);
    }

    IEnumerator Decay(float time)
    {
        yield return new WaitForSeconds(time);

        spell.shape.DestroyAndStartChildren(gameObject);
    }
}
