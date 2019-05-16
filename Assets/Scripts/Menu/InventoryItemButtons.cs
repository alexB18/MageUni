using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButtons : MonoBehaviour
{
    public Button inventoryButton;
    public GameObject player;

    private Inventory playerInv;
    private InventoryButtons listHolder;

    // Start is called before the first frame update
    void Start()
    {
        playerInv = player.GetComponent<Inventory>();
        listHolder = inventoryButton.GetComponent<InventoryButtons>();
    }

    public void Select(int index)
    {
        listHolder.infoItemName.text = playerInv.inventory[index].itemName;
        listHolder.infoDescription.text = playerInv.inventory[index].description;

        foreach (Image image in listHolder.invImages)
        {
            image.color = Color.white;
        }
        listHolder.invImages[index].color = new Color(0.3f, 0.6f, 0.8f);
        ActionButtons.selected = index;
    }
}
