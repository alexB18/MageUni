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
    private int[] indices;

    // Image children
    private string imageDirectory = "";
    public GameObject childPrefab;
    private Image[] images;

    private Vector2 startPosition = new Vector2();
    private Vector3 startScale = new Vector3(1f, 1f, 1f);

    public GameObject highlightChild;

    private GameObject[] childImages;
    public GameObject[] ChildImages => childImages;

    public Glyph Glyph {
        get => glyph;
        set
        {
            glyph = value;
            imageDirectory = glyph.GlyphSprite;

            connectedEnds = new GlyphUIElement[glyph.Connections.Count];

            int imageDim = (glyph.IsShape) ? 1 : glyph.Connections.Count;
            images = new Image[imageDim];
            childImages = new GameObject[imageDim];
            for(int i = 0; i < imageDim; ++i)
            {
                GameObject child = Instantiate(childPrefab);
                child.name = i.ToString();
                child.transform.SetParent(transform);
                child.GetComponent<RectTransform>().anchoredPosition.Set(0, 0);
                childImages[i] = child;
                images[i] = child.GetComponent<Image>();
                images[i].sprite = Resources.Load<Sprite>(imageDirectory + "/" + i);
            }
        }
    }
    public GlyphUIElement ParentShape { get => parentShape; set => parentShape = value; }

    public AllSpellsAndGlyphs.SpellComponentEnum GlyphType { get => glyphType; set => glyphType = value; }

    public void Connect(int[] indices, GlyphUIElement child)
    {
        Vector2 origin = new Vector2(0.5f, 0.5f);
        Vector2 zero = new Vector2(0f, 1f);
        GameObject[] children = child.ChildImages;
        /*
        int i = 0;
        float a = 0f, b = 0f;
        foreach (int index in indices)
        {
            connectedEnds[index] = child;
            // Rotate the child image to fit our constraint
            Vector2 v = child.Glyph.Connections[i].Coordinates - origin;
            Vector2 g = glyph.Connections[index].Coordinates - origin;
            // Minimise distance with rotation with respect to t:
            a += g.x * v.x + g.y * v.y;
            b += g.x * v.y - g.y * v.x;
            //float t = Mathf.Rad2Deg * (- Mathf.Atan((g.x * v.y - g.y * v.x) / (g.x * v.x - g.y * v.y)));
            //Debug.Log("Connection " + i + " rotate z by " + t + " degrees");
            ++i;
        }
        float t = Mathf.Rad2Deg * (Mathf.Atan(b / a));
        //*/

        // There may be a case where the rotation is flipped. Check for that
        float t;
        Vector2 gd = new Vector2();
        Vector2 vd = new Vector2();
        for (int i = 0; i < indices.Length; ++i)
        {
            connectedEnds[indices[i]] = child;
            Vector2 g = glyph.Connections[indices[i]].Coordinates - origin;
            Vector2 v = origin - child.Glyph.Connections[i].Coordinates;
            //v.Set(v.x * Mathf.Cos(t1) - v.y * Mathf.Sin(t1), v.x * Mathf.Sin(t1) + v.y * Mathf.Cos(t1));
            gd += g;
            vd += v;
        }
        t = Vector2.Angle(gd, vd);
        Vector3 cross = Vector3.Cross(new Vector3(vd.x, vd.y), new Vector3(gd.x, gd.y));
        Vector3 normal = new Vector3(0, 0, 1);
        t = (Vector3.Dot(cross, normal) >= 0) ? -t : t;
        Debug.Log("Rotate by " + t + " degrees");
        child.transform.rotation = Quaternion.Euler(0, 0, t);
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

    public int ImageLength => glyph.IsShape ? 1 : connectedEnds.Length;

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
        if (ret >= length)
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
    public bool Resolve(int startIndex = 0, bool clockwise = true)
    {
        // If we don't have our images yet, let's get them
        // Release connections on parent
        ParentShape.Disconnect(this);

        int offset = (clockwise) ? 1 : -1;

        int parentLength = parentShape.ConnectionLength;
        int myLength = (Glyph.IsShape) ? (1) : glyph.Connections.Count;
        int iterations = nCr(parentLength, myLength);
        indices = new int[myLength];


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
            //int currIteration = i*offset + indexOffset;
            int currIteration = i*offset + indexOffset;
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
                //int newIndex = (clockwise) ? startIndex + indices[j] : startIndex - indices[j];
                int newIndex = startIndex + indices[j];
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
                else
                {
                    indices[j] = newIndex;
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
    public void FindChildrenImages()
    {
        // We only have the highlight image and the children
        childImages = new GameObject[ImageLength];
        // Note, unity magically makes it so that transform can be iterated 
        // over to get its children. Whack.
        foreach (Transform c in transform)
        {
            if (int.TryParse(c.name, out int index))
                childImages[index] = transform.gameObject;
        }
    }
    public int StartIndex(bool clockwise = true)
    {
        //int index = clockwise ? indices[0] : indices[indices.Length - 1];

        return indices[0];
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
