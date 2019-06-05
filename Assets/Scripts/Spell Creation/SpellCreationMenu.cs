using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SpellCreationMenu : MonoBehaviour
{
    private GameObject player;
    private const float fScaleIncrement = 0.1f;
    private const float fMaxScale = 1.7f;
    private const float fMinScale = 0.5f;
    private readonly Vector3 childScale = new Vector3(0.5f, 0.5f, 0.5f);

    private readonly Vector3 scaleIncrement = new Vector3(fScaleIncrement, fScaleIncrement, fScaleIncrement);
    private const float maxScaleSqr = fMaxScale * fMaxScale;
    private const float minScaleSqr = fMinScale * fMinScale;


    private GlyphUIElement selected;
    private RectTransform selectedTransform;
    private GlyphUIElement baseShape;

    public GameObject glyphListElementPrefab;
    public GameObject glyphUIPrefab;

    public GameObject componentView;
    public GameObject shapeSelectorContent;
    public GameObject effectSelectorContent;
    public GameObject modifierSelectorContent;

    private List<GameObject> _glyphs = new List<GameObject>();

    private Subscriber clickHandler;
    public TMPro.TextMeshProUGUI componentName;

    // Saving the Spell
    public Dropdown spellSlotSelector;
    public TMPro.TMP_InputField spellNameField;

    private void HandleClick(Object[] obj)
    {
        ClickListener clickListener = obj[0] as ClickListener;
        
        SetSelected(clickListener.GetGlyphUIElement());
    }

    public void Awake()
    {
        clickHandler = HandleClick;
    }

    public void Start()
    {
        ClickListener componentViewClickListener = componentView.GetComponent<ClickListener>() as ClickListener;
        componentViewClickListener.SubscribeToClickEvent(clickHandler);
    }

    public GlyphUIElement GetSelected()
    {
        return selected;
    }

    public void SetSelected(GlyphUIElement obj)
    {
        if (selected != null)
            selected.SetSelected(false);
        componentName.text = "";
        selected = obj;

        if (selected != null)
        {
            selectedTransform = selected.GetComponent<RectTransform>();
            selected.SetSelected(true);
            componentName.text = selected.Glyph.name;
        }
    }

    private void OnEnable()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Spell Creation Menu is enabled: loading glyphs");

        //*
        CreateGlyphListElement(AllSpellsAndGlyphs.boltGlyph);
        //CreateGlyphListElement(AllSpellsAndGlyphs.ballGlyph);
        CreateGlyphListElement(AllSpellsAndGlyphs.selfGlyph);

        CreateGlyphListElement(AllSpellsAndGlyphs.fireGlyph);
        CreateGlyphListElement(AllSpellsAndGlyphs.healGlyph);
        CreateGlyphListElement(AllSpellsAndGlyphs.stunGlyph);
        CreateGlyphListElement(AllSpellsAndGlyphs.lightningGlyph);
        CreateGlyphListElement(AllSpellsAndGlyphs.freezeGlyph);
        CreateGlyphListElement(AllSpellsAndGlyphs.enrageGlyph);
        CreateGlyphListElement(AllSpellsAndGlyphs.hasteGlyph);
        CreateGlyphListElement(AllSpellsAndGlyphs.slowGlyph);

        CreateGlyphListElement(AllSpellsAndGlyphs.speedIncreaseGlyph);
        //*/
        // Instantiate
        /*
        PlayerSpellInventory playerSpellInventory = player.GetComponent<PlayerSpellInventory>();
        foreach(Glyph g in playerSpellInventory.KnownGlyphs)
            CreateGlyphListElement(g);
        //*/
    }

    public void UpdateSpellSlots()
    {
        spellNameField.text = "";
        PlayerController pc = player.GetComponent<PlayerController>();
        spellSlotSelector.ClearOptions();
        for (int i = 0; i < pc.SpellSlotsAvailable; ++i)
            spellSlotSelector.options.Add(new Dropdown.OptionData("Slot " + (i + 1) + ": " + ((pc.Spells[i] != null) ? pc.Spells[i].name : "Empty")));
        spellSlotSelector.value = 1; // Force dirty
        spellSlotSelector.value = 0;
    }

    private void CreateGlyphListElement(Glyph glyph)
    {
        GameObject obj = Instantiate(glyphListElementPrefab);
        _glyphs.Add(obj);
        GlyphListToken token = obj.GetComponent<GlyphListToken>();
        token.Glyph = glyph;
        token.SpellMenu = this;

        // Add to the correct selector
        switch (token.Glyph.Component.ComponentType)
        {
            case AllSpellsAndGlyphs.SpellComponentEnum.SCShape:
                obj.transform.SetParent(shapeSelectorContent.transform);
                break;

            case AllSpellsAndGlyphs.SpellComponentEnum.SCEffect:
                obj.transform.SetParent(effectSelectorContent.transform);
                break;

            case AllSpellsAndGlyphs.SpellComponentEnum.SCModifier:
                obj.transform.SetParent(modifierSelectorContent.transform);
                break;
        }
    }

    private void OnDisable()
    {
        // Delete our currently built spell
        DeleteSelected();
        SetSelected(baseShape);
        DeleteSelected();

        // Remove our glyphs
        foreach(var token in _glyphs)
            Destroy(token);
    }

    public void AddGlyph(Glyph glyph)
    {
        Transform parentTransform = null;
        bool canAdd = false;
        bool focus = false;

        if (selected == null)
        {
            if (glyph.IsShape && baseShape == null)
            {
                canAdd = true;
                parentTransform = componentView.transform;
                focus = true;
            }
        }
        else if(selected.Glyph.IsShape)
        {
            canAdd = true;
            parentTransform = selected.transform;
            focus = false;
        }

        if(canAdd)
        {
            // Create the glyph
            GameObject glyphUIObject = Instantiate(glyphUIPrefab) as GameObject;
            GlyphUIElement glyphUIElement = glyphUIObject.GetComponent<GlyphUIElement>() as GlyphUIElement;
            glyphUIElement.Glyph = glyph;

            // Make the glyph a child of the correct transform as determined above
            glyphUIElement.transform.SetParent(parentTransform);
            glyphUIElement.StartPosition = new Vector2(0.0f, 0.0f);

            // If this is the first shape, let's set it as the root of our tree
            if (baseShape == null && glyph.IsShape)
                baseShape = glyphUIElement;
            else // Otherwise we should scale this down and resolve it
            {
                glyphUIElement.StartScale = childScale;
                glyphUIElement.ParentShape = selected;
                if(!glyphUIElement.Resolve(0, true))
                {
                    // We couldn't resolve the glyph, remove it
                    SetSelected(glyphUIElement);
                    DeleteSelected();
                    //
                    return;
                }
            }

            // Listen for it being clicked so we know to focus on it
            ClickListener clickListener = glyphUIElement.GetComponent<ClickListener>() as ClickListener;
            clickListener.SubscribeToClickEvent(clickHandler);

            if(focus)
                SetSelected(glyphUIElement);
        }
    }

    public void DeleteSelected()
    {
        if (selected != null)
        {
            if (selected.Equals(baseShape))
                baseShape = null;
            else
                selected.ParentShape.Disconnect(selected);
            Destroy(selected.gameObject);
            selected = null;
        }
    }

    public void ScaleDown()
    {
        bool selectedExists = selected != null;
        bool isBaseShape = selected == baseShape;
        bool withinConstrains = (selectedExists) ? selectedTransform.localScale.x > fMinScale : false;
        if (selectedExists && isBaseShape && withinConstrains)
            selectedTransform.localScale -= scaleIncrement;
    }

    public void ScaleUp()
    {
        bool selectedExists = selected != null;
        bool isBaseShape = selected == baseShape;
        bool withinConstrains = (selectedExists) ? selectedTransform.localScale.x < fMaxScale : false;
        if (selectedExists && isBaseShape && withinConstrains)
            selectedTransform.localScale += scaleIncrement;
    }

    public void RotateSelectedRight()
    {
        Debug.Log("Rotate right");
        if (selected != null && selected != baseShape)
        {
            int index = selected.StartIndex(true) + 1;
            selected.Resolve(index, true);
        }
    }

    public void RotateSelectedLeft()
    {
        Debug.Log("Rotate left");
        if (selected != null && selected != baseShape)
        {
            int index = selected.StartIndex(false) - 1;
            selected.Resolve(index, false);
        }
    }

    public void SaveSpell()
    {
        Debug.Log("Save spell button has been clicked");
    }

    // Traverse our DAG and generate our spell hierarchy which will be ready to launch
    public SpellScript.Spell Compile()
    {
        SpellScript.Spell ret = baseShape.Compile();
        return ret;
    }

    public void ConfirmSave()
    {
        if (baseShape)
        {
            SpellScript.Spell spell = Compile();
            spell.name = spellNameField.text;
            if (spell.name == null || spell.name == "")
                spell.name = "Spell";
            PlayerController controller = player.GetComponent<PlayerController>();
            controller.ChangeSpellSlot(spellSlotSelector.value);
            controller.SetSpell(spellSlotSelector.value, spell);
        }
        else
        {
            // Error message
        }
    }
}
