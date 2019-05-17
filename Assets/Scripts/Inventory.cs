using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<PickUp> inventory;
    //public GameObject player;
    private Inventory playerInv;
    // Start is called before the first frame update
    void Start()
    {
        playerInv = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop(int index)
    {
        PickUp item = inventory[index];
        inventory.Remove(item);
        Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));
        item.transform.position = transform.position + randomOffset;
        if (item.thisRigidbody != null)
        {
            item.thisRigidbody.useGravity = true;
        }
    }

    public string LookAt(int index)
    {
        return inventory[index].description;
    }

    public void TakeFrom(int index)
    {
        PickUp item = inventory[index];
        inventory.Remove(item);
        playerInv.inventory.Add(item);
    }

    public void PutInto(PickUp item)
    {
        playerInv.inventory.Remove(item);
        inventory.Add(item);
    }
}
