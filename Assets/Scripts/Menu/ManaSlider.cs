using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaSlider : MonoBehaviour
{
    public PlayerHolder menuHolder;
    public Slider mySlider;
    StatScript stat;

    private void OnManaChange(Object[] obj)
    {
        StatScript hs = obj[0] as StatScript;
        mySlider.value = hs.CurrentMana / hs.MaximumMana;
    }

    // Start is called before the first frame update
    void Start()
    {
        stat = menuHolder.Player.GetComponent<StatScript>();
        stat.SubscribeToOnManaChange(OnManaChange);
    }

    private void OnEnable()
    {
        stat = menuHolder.Player.GetComponent<StatScript>();
        stat.RestoreMana(0);
    }
}
