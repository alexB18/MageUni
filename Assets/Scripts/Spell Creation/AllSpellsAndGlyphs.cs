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
    public static readonly SpellShape spellShapeBall = new SpellShapeBall();
    public static readonly SpellShape spellShapeSelf = new SpellShapeSelf();
    public static readonly SpellEffect spellEffectFire = new SpellEffectFire();
    public static readonly SpellEffect spellEffectHeal = new SpellEffectHeal();
    public static readonly SpellEffect spellEffectStun = new SpellEffectStun();
    public static readonly SpellEffect spellEffectLightning = new SpellEffectLightning();
    public static readonly SpellEffect spellEffectFreeze = new SpellEffectFreeze();
    public static readonly SpellEffect spellEffectEnrage = new SpellEffectEnrage();
    public static readonly SpellEffect spellEffectHaste = new SpellEffectHaste();
    public static readonly SpellEffect spellEffectSlow = new SpellEffectSlow();
    public static readonly SpellModifier spellModifierSpeedIncrease = new SpellModifierSpeedIncrease();

    public static readonly Glyph boltGlyph = new Glyph(
        spellShapeBolt,
        "ShapeBolt",
        new List<Glyph.Connection> {
            new Glyph.Connection(64f/128f, 110f/128f),
            new Glyph.Connection(35/128f, 55/128f),
            new Glyph.Connection(93/128f, 55/128f) },
        new Glyph.ConnectionMap(new bool[][] {
            new bool[] { true, true, true },
            new bool[] { true, true, true },
            new bool[] { true, true, true } })
        );
    // Todo change connections
    public static readonly Glyph ballGlyph = new Glyph(
         spellShapeBall,
         "ShapeBall",
         new List<Glyph.Connection> {
            new Glyph.Connection(64f/128f, 118f/128f),
            new Glyph.Connection(32/128f, 64/128f),
            new Glyph.Connection(96/128f, 64/128f) },
         new Glyph.ConnectionMap(new bool[][] {
            new bool[] { true, true, true },
            new bool[] { true, true, true },
            new bool[] { true, true, true } })
         );
    // Todo change connections
    public static readonly Glyph selfGlyph = new Glyph(
         spellShapeSelf,
         "ShapeSelf",
         new List<Glyph.Connection> {
            new Glyph.Connection(0f/128f, 64f/128f),
            new Glyph.Connection(64/128f, 0/128f),
            new Glyph.Connection(128/128f, 64/128f),
            new Glyph.Connection(64/128f, 128/128f) },
         new Glyph.ConnectionMap(new bool[][] {
            new bool[] { true, true, false, true },
            new bool[] { true, true, true, false },
            new bool[] { false, true, true, true },
            new bool[] { true, false, true, true },
         })
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
    public static readonly Glyph healGlyph = new Glyph(
        spellEffectHeal,
        "EffectHeal",
        new List<Glyph.Connection>
        {
            new Glyph.Connection(0f/128f, 86f/128f),
            new Glyph.Connection(128f/128f, 86f/128f)
        },
        null
        );
    public static readonly Glyph stunGlyph = new Glyph(
        spellEffectStun,
        "EffectStun",
        new List<Glyph.Connection>
        {
            new Glyph.Connection(0f/128f, 37f/128f),
            new Glyph.Connection(128f/128f, 37f/128f)
        },
        null
        );
    public static readonly Glyph lightningGlyph = new Glyph(
        spellEffectLightning,
        "EffectLightning",
        new List<Glyph.Connection>
        {
            new Glyph.Connection(0f/128f, 72f/128f),
            new Glyph.Connection(128f/128f, 72f/128f),
            new Glyph.Connection(64f/128f, 95f/128f)
        },
        null
        );
    public static readonly Glyph freezeGlyph = new Glyph(
        spellEffectFreeze,
        "EffectFreeze",
        new List<Glyph.Connection>
        {
            new Glyph.Connection(0f/128f, 28f/128f),
            new Glyph.Connection(128f/128f, 28f/128f)
        },
        null
        );
    public static readonly Glyph enrageGlyph = new Glyph(
        spellEffectEnrage,
        "EffectEnrage",
        new List<Glyph.Connection>
        {
            new Glyph.Connection(0f/128f, 52f/128f),
            new Glyph.Connection(128f/128f, 52f/128f),
            new Glyph.Connection(64f/128f, 128f/128f)
        },
        null
        );
    public static readonly Glyph hasteGlyph = new Glyph(
        spellEffectHaste,
        "EffectHaste",
        new List<Glyph.Connection>
        {
            new Glyph.Connection(64f/128f, 0f/128f)
        },
        null
        );
    public static readonly Glyph slowGlyph = new Glyph(
        spellEffectSlow,
        "EffectSlow",
        new List<Glyph.Connection>
        {
            new Glyph.Connection(64f/128f, 84f/128f)
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
