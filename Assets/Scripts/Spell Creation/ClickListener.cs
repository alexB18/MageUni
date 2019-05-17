using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickListener : MonoBehaviour, IPointerClickHandler
{
    private List<Subscriber> clickSubscribers;
    public GlyphUIElement glyphUIElement;

    public void Awake()
    {
        clickSubscribers = new List<Subscriber>();
    }

    public void SubscribeToClickEvent(Subscriber sub) => clickSubscribers.Add(sub);

    public bool UnsubscribeFromClickEvent(Subscriber sub) => clickSubscribers.Remove(sub);

    public void OnPointerClick(PointerEventData eventData)
    {
        Object[] args = { this };
        foreach (Subscriber sub in clickSubscribers)
        {
            sub.Invoke(args);
        }
    }

    public bool Selectable()
    {
        return glyphUIElement != null;
    }

    public GlyphUIElement GetGlyphUIElement()
    {
        return glyphUIElement;
    }
}
