using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButtons : MonoBehaviour
{
    public Button inventoryButton;
    public PlayerHolder playerHolder;

    private Inventory playerInv;
    private InventoryButtons listHolder;

    // Start is called before the first frame update
    void Start()
    {
        playerInv = playerHolder.Player.GetComponent<Inventory>();
        listHolder = inventoryButton.GetComponent<InventoryButtons>();
    }

    public void Select(int index)
    {
        listHolder.infoItemName.text = playerInv.inventory[index].itemName;
        listHolder.infoDescription.text = playerInv.inventory[index].description;

        foreach (Button b in listHolder.invButtons)
        {
            b.image.color = Color.white;
        }
        listHolder.invButtons[index].image.color = new Color(0.3f, 0.6f, 0.8f);
        ActionButtons.selected = index;
    }
}
