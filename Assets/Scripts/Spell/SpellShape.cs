using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellShape : SpellComponent
{
    public override bool IsShape => true;
    public override AllSpellsAndGlyphs.SpellComponentEnum ComponentType => AllSpellsAndGlyphs.SpellComponentEnum.SCShape;

    private List<SpellScript.Spell> childSpells = new List<SpellScript.Spell>();
    public void AddChildSpell(SpellScript.Spell spell) => childSpells.Add(spell);

    public void DestroyAndStartChildren(GameObject self)
    {
        // If we are triggered and are finished with our spell, we should start our child effects
        foreach(SpellScript.Spell spell in childSpells)
        {
            GameObject newSpell = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Spell"));
            SpellScript spellScript = newSpell.GetComponent<SpellScript>();
            spellScript.spell = spell;
        }
        GameObject.Destroy(self);
    }
}
