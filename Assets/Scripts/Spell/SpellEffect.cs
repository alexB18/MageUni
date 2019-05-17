using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellEffect : SpellComponent
{
    public override AllSpellsAndGlyphs.SpellComponentEnum ComponentType => AllSpellsAndGlyphs.SpellComponentEnum.SCEffect;
}
