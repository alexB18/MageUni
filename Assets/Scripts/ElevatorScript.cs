using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    public float top;
    public float bottom;
    public float speed;

    private Vector3 moveDirection = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= top)
        {
            moveDirection = Vector3.down;
        } else if (transform.position.y <= bottom)
        {
            moveDirection = Vector3.up;
        }

        transform.Translate(moveDirection * Time.deltaTime * speed);
    }
}
