using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlockDetect : MonoBehaviour
{
    private bool inPlace = false;
    Rigidbody otherRb;

    private void OnCollisionEnter(Collision other)
    {
        otherRb = other.gameObject.GetComponent<Rigidbody>();

        if (other.gameObject.CompareTag("MovingBlock_Detect"))
        {
            gameObject.GetComponentInParent<hidden_key>().setTriggerActive();
            otherRb.velocity = Vector3.zero;
            otherRb.detectCollisions = false;
        }
    }
}
