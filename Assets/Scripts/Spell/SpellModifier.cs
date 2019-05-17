using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellModifier : SpellComponent
{
    public override AllSpellsAndGlyphs.SpellComponentEnum ComponentType => AllSpellsAndGlyphs.SpellComponentEnum.SCModifier;
}
