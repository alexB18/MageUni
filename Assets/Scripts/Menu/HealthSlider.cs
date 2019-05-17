using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public PlayerHolder menuHolder;
    public Slider mySlider;
    
    private void OnHealthChange(Object[] obj) 
    {
        StatScript hs = obj[0] as StatScript;
        mySlider.value = hs.CurrentHealth / hs.MaximumHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        menuHolder.Player.GetComponent<StatScript>().SubscribeToOnHealthChange(OnHealthChange);
    }
}
