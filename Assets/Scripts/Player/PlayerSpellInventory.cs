using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellInventory : MonoBehaviour
{
    private List<Glyph> knownGlyphs = new List<Glyph>();

    public List<Glyph> KnownGlyphs { get => knownGlyphs; set => knownGlyphs = value; }
    public void AddGlyph(Glyph g) => knownGlyphs.Add(g);
}
