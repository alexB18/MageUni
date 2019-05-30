using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictDetectBlockInteraction : MonoBehaviour
{
    Collider coll;
    private void Start()
    {
        coll = GetComponent<Collider>();
    }
    private void OnCollisionEnter(Collision other)
    {
        // Don't allow any one to interact with block except other block
        if (!(other.gameObject.tag == "MovingBlock_Detect"))
        {
            Physics.IgnoreCollision(other.gameObject.GetComponentInChildren<Collider>(), coll);
        }
    }
}
