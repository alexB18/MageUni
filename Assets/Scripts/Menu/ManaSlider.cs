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
        HealthScript hs = obj[0] as HealthScript;
        mySlider.value = hs.currentHealth / hs.maximumHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        menuHolder.Player.GetComponent<HealthScript>().SubscribeToOnHealthChange(OnManaChange);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
