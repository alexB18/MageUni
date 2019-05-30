using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictEnemyInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Don't allow bad guys to move blocks
        if (!other.CompareTag("Player"))
        {
            Physics.IgnoreCollision(other.GetComponent<Collider>(), this.GetComponent<Collider>());
        }
    }
}
