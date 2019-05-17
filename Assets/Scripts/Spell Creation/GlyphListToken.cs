using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

public class GlyphListToken : MonoBehaviour, IPointerClickHandler
{
    private Glyph glyph;
    private SpellCreationMenu spellMenu;
    public Image image;
    
    public Glyph Glyph {
        get => glyph;
        set {
            glyph = value;
            image.sprite = Resources.Load<Sprite>(glyph.GlyphSprite + "/icon");
        }
    }

    public SpellCreationMenu SpellMenu { get => spellMenu; set => spellMenu = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount == 2)
        {
            // create GlyphUIElement
            SpellMenu.AddGlyph(Glyph);
        }
    }
}
