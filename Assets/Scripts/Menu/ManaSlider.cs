using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaSlider : MonoBehaviour
{
    public PlayerHolder menuHolder;
    public Slider mySlider;

    private void OnManaChange(Object[] obj)
    {
        StatScript hs = obj[0] as StatScript;
        mySlider.value = hs.CurrentMana / hs.MaximumMana;
    }

    // Start is called before the first frame update
    void Start()
    {
        menuHolder.Player.GetComponent<StatScript>().SubscribeToOnManaChange(OnManaChange);
    }
}
