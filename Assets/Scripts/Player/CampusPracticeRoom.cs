using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampusPracticeRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().campusPracticeArena = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().campusPracticeArena = false;
        }
    }
}
