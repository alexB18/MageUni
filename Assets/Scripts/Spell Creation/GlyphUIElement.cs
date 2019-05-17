using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GlyphUIElement : MonoBehaviour
{
    // This is the object that stores data connections
    private Glyph glyph;
    private GlyphUIElement parentShape;
    private AllSpellsAndGlyphs.SpellComponentEnum glyphType;
    private GlyphUIElement[] connectedEnds;

    // Image children
    private string imageDirectory = "";
    public GameObject childPrefab;
    private Image[] images;

    private Vector2 startPosition = new Vector2();
    private Vector3 startScale = new Vector3(1f, 1f, 1f);

    public GameObject highlightChild;

    public Glyph Glyph {
        get => glyph;
        set
        {
            glyph = value;
            imageDirectory = glyph.GlyphSprite;

            connectedEnds = new GlyphUIElement[glyph.Connections.Count];

            int imageDim = (glyph.IsShape) ? 1 : glyph.Connections.Count;
            images = new Image[imageDim];
            for(int i = 0; i < imageDim; ++i)
            {
                GameObject child = Instantiate(childPrefab);
                child.transform.SetParent(transform);
                child.GetComponent<RectTransform>().anchoredPosition.Set(0, 0);
                images[i] = child.GetComponent<Image>();
                images[i].sprite = Resources.Load<Sprite>(imageDirectory + "/" + i);
            }
        }
    }
    public GlyphUIElement ParentShape { get => parentShape; set => parentShape = value; }

    public AllSpellsAndGlyphs.SpellComponentEnum GlyphType { get => glyphType; set => glyphType = value; }

    public void Connect(int[] indices, GlyphUIElement child)
    {
        foreach(int index in indices)
            connectedEnds[index] = child;
    }

    public void Disconnect(GlyphUIElement child)
    {
        for(int i = 0; i < connectedEnds.Length; ++i)
            if(connectedEnds[i] == child)
                connectedEnds[i] = null;
    }

    public bool IsConnected(int index)
    {
        bool ret = connectedEnds[index] != null;
        return ret;
    }

    public int ConnectionLength => connectedEnds.Length;

    public Vector2 StartPosition { get => startPosition; set => startPosition = value; }
    public Vector3 StartScale { get => startScale; set => startScale = value; }

    private void Start()
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.anchoredPosition = StartPosition;
        transform.localScale = StartScale;
    }
    
    public void SetSelected(bool b)
    {
        highlightChild.SetActive(b);
    }

    private int CircularIndex(int value, int length)
    {
        int ret = value;
        if (ret > length)
            ret -= length;
        else if (ret < 0)
            ret += length;
        return ret;
    }

    private int nCr(int n, int r)
    {
        int denominator = fact(n);
        int numerator = fact(r) * fact(n - r);
        return denominator / numerator;
    }

    private int fact(int n)
    {
        int acc = 1;
        for (int i = 1; i < n + 1; ++i)
            acc *= i;
        return acc;
    }

    /**
     * Resolves a connection between this object (the focused object) and the parent
     * (the container object). Best connection will be the connection which starts
     * closest to the startIndex with its offset in the direction of clockwise
     * Returns whether or not a connection could be resolved
     */
    public bool Resolve(int startIndex, bool clockwise)
    {
        // Release connections on parent
        ParentShape.Disconnect(this);

        int offset = (clockwise) ? 1 : -1;

        int parentLength = parentShape.ConnectionLength;
        int myLength = (Glyph.IsShape) ? (1) : glyph.Connections.Count;
        int iterations = nCr(parentLength, myLength);
        int[] indices = new int[myLength];


        int indexOffset = 0;
        for(int j = myLength - 1, mod = 1; j > 0; --j)
        {
            mod *= parentLength;
            indexOffset += j * mod;
        }
        Debug.Log("IndexOffset: " + indexOffset);

        for (int i = 0; i < iterations; ++i)
        {
            // Label is for the inner loop mechanic since it's whack
            doContinue:

            // Calculate the relative indices
            int mod = 1;
            int prevMod = 1;
            int prev = 0;
            int currIteration = i + indexOffset;
            for (int j = 0; j < myLength; ++j)
            {
                mod *= parentLength;
                indices[j] = ((currIteration % mod) - prev) / prevMod;
                prev = indices[j];
                prevMod = mod;
            }

            // Convert the absolute indices
            for(int j = 0; j < myLength; ++j)
            {
                int newIndex = (clockwise) ? startIndex + indices[j] : startIndex - indices[j];
                newIndex = CircularIndex(newIndex, parentLength);

                // If this index is already connected, move the index forward
                if(ParentShape.IsConnected(newIndex))
                {
                    i += (int) Mathf.Pow(parentLength, j);
                    if (i < iterations)
                        goto doContinue;
                    else
                        goto failure;
                }
            }

            // Check if each pair of connections at the indices are adjacent
            bool adjacent = true;
            prev = indices[0];

            for(int j = 1; j < myLength; ++j)
            {
                adjacent &= parentShape.Glyph.Connected(prev, indices[j]);
                prev = indices[j];
                if (!adjacent)
                    break;
            }

            if (adjacent)
            {
                // We have a match. Transform the object to meet the nodes
                string debugString = "Found match with connections";
                foreach (int j in indices)
                {
                    debugString += " " + j;
                }
                Debug.Log(debugString);
                ParentShape.Connect(indices, this);
                return true;
            }
            
            failure:
            Debug.Log("Could not resolve glyph");
        }

        // We couldn't find a suitable connection
        return false;
    }
    public SpellScript.Spell Compile()
    {
        SpellScript.Spell ret = new SpellScript.Spell();
        ret.shape = Glyph.Component as SpellShape;


        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            if (obj.CompareTag("GlyphUI"))
            {
                GlyphUIElement glyphUI = obj.GetComponent<GlyphUIElement>();
                // Recurse if it's a shape! Otherwise, add it to our components
                if (glyphUI.Glyph.IsShape)
                    ret.shape.AddChildSpell(glyphUI.Compile());
                else
                    ret.components.Add(glyphUI.Glyph.Component);
            }
        }

        return ret;
    }
}
