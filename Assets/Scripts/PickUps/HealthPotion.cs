using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : PickUp
{
    public float healAmount;

    public override void Use()
    {
        playerInv.inventory.Remove(this);
        HealthScript playerHealth = player.GetComponent<HealthScript>();
        playerHealth.RestoreHealth(healAmount);
    }
}
