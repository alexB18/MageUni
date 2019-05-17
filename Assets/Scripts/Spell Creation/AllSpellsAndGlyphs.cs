using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllSpellsAndGlyphs
{
    public enum SpellComponentEnum
    {
        SCEffect,
        SCModifier,
        SCShape
    }

    public static readonly SpellShape spellShapeBolt = new SpellShapeBolt();
    public static readonly SpellEffect spellEffectFire = new SpellEffectFire();
    public static readonly SpellModifier spellModifierSpeedIncrease = new SpellModifierSpeedIncrease();

    public static readonly Glyph boltGlyph = new Glyph(
        spellShapeBolt,
        "ShapeBolt",
        new List<Glyph.Connection> {
            new Glyph.Connection(64f/128f, 118f/128f),
            new Glyph.Connection(32/128f, 64/128f),
            new Glyph.Connection(96/128f, 64/128f) },
        new Glyph.ConnectionMap(new bool[][] {
            new bool[] { true, true, true },
            new bool[] { true, true, true },
            new bool[] { true, true, true } })
        );
    public static readonly Glyph fireGlyph = new Glyph(
        spellEffectFire,
        "EffectFire",
        new List<Glyph.Connection>
        {
            new Glyph.Connection(0f/128f, 96f/128f),
            new Glyph.Connection(128f/128f, 96/128f)
        },
        null
        );
    public static readonly Glyph speedIncreaseGlyph = new Glyph(
        spellModifierSpeedIncrease,
        "ModifierSpeedIncrease",
        new List<Glyph.Connection> { new Glyph.Connection(64f/128f, 128f/128f) },
        null
        );
}
