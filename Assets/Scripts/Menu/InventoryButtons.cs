using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtons : MonoBehaviour
{
    public PlayerHolder playerHolder;
    public List<Button> invButtons;
    public Text infoItemName;
    public Text infoDescription;

    private Inventory playerInv;

    // Start is called before the first frame update
    void Start()
    {
        playerInv = playerHolder.Player.GetComponent<Inventory>();
    }
    
    public void Inventory()
    {
        int numItems = 0;
        foreach (PickUp item in playerInv.inventory)
        {
            // here is where we would set the sprite for the image if we had one
            if (numItems <= 15)
            {
                invButtons[numItems].interactable = true;
                numItems++;
            }
        }
        while (numItems <= 15)
        {
            invButtons[numItems].interactable = false;
            numItems++;
        }
        infoDescription.text = "";
        infoItemName.text = "";

        foreach (Button b in invButtons)
        {
            b.image.color = Color.white;
        }
    }
}
