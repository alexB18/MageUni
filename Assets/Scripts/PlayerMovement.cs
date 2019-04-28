using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Player public variables
    public float speed = 6f;

    // Player private variables
    private Vector3 movement;
    private Animator anim;
    private Rigidbody playerRb;

    // Raycasting private variables
    private int floorMask;                  // Used to tell if ray cast has hit ground
    private float camRayLength = 100;       // Length of ray cast from camera

    // Called before first frame update regardless of script being enabled or not
    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();            //Retrieve animator from player object
        playerRb = GetComponent<Rigidbody>();       //Retrieve rigidbody from player object

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Used primarily for physics
    private void FixedUpdate()
    {
        // Raw -> means player will "snap" to full speed with no acceleration
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);

    }

    private void Move(float h, float v)
    {
        //Lateral movement only
        movement.Set(h, 0f, v);

        /*Because of how vectors work, moving in diagonal results in speed higher 
          than h or v so we need to normalize */
        
        //                                       Time between each update call
        movement = movement.normalized * speed * Time.deltaTime;
        //Move player to current position + movement
        playerRb.MovePosition(transform.position + movement);
    }

    private void Turning()
    {
        // Create Camera Ray
        // Takes point underneath mouse and casts a ray from that point to the screen 
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Store Raycast data
        RaycastHit floorHit;

        // If hit
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            // Set player forward vector to mouse position
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRb.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        // If moving, walking -> true
        bool running = h != 0f || v != 0f;
        anim.SetBool("IsRunning", running);
    }
}
