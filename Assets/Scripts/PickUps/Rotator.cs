using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private static readonly Vector3 axis = new Vector3(0, 4, 1);
    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
        transform.Rotate(axis, 60 * Time.deltaTime);
    }
}
