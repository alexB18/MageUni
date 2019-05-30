using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock_Physics : MonoBehaviour
{
    public float pushPower = 2.0f;
    public int keyHidden = 0;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        
        // Account for case where rb doesn't exist or is kinematic
        if(rb == null || rb.isKinematic)
        {
            return;          
        }

        // Account for any movement in the y direction
        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        // 
        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, 0);
        rb.velocity = pushDirection * pushPower * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the block reaches where it needs to be, 
        if (other.CompareTag("MovingBlock_Detect"))
        {
            // Completely restrict any new movement
            this.pushPower = 0.0f;

            // TODO:
            // Then trigger part 1 (or 2) of the key buisiness...
            
            if(keyHidden < 2)
            {
                this.keyHidden += 1;

            }
        }
    }
}
