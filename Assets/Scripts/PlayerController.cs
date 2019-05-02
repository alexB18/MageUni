using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player public variables
    public float speed = 6f;

    // Player private variables
    private Vector3 movement;
    private Animator anim;
    private Rigidbody playerRb;

    // Follow camera
    public GameObject followCamera;
    public float followCamera2DDistance = 7.5f;
    public float followCameraHeight = 7.5f;
    private Vector3 followCameraDisplacement;
    public Vector3 followCameraRotation = new Vector3(45f, 0f, 0f);

    // Spell variables
    public GameObject emptySpellPrefab;
    public const int spellMaxSpellSlots = 5;
    private int spellSlotsAvailable = 1;
    private SpellScript.Spell[] spellEffects = new SpellScript.Spell[spellMaxSpellSlots];

    // Raycasting private variables
    private int floorMask;                  // Used to tell if ray cast has hit ground
    private float camRayLength = 100;       // Length of ray cast from camera

    // Called before first frame update regardless of script being enabled or not
    private void Awake()
    {
        followCameraDisplacement = new Vector3(0.0f, followCameraHeight, -followCamera2DDistance);
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();            //Retrieve animator from player object
        playerRb = GetComponent<Rigidbody>();       //Retrieve rigidbody from player object

        // TODO remove PoC spells
        SpellScript.Spell fireSpell = new SpellScript.Spell();
        fireSpell.effects.Add(new SpellEffectFire());
        fireSpell.shape = new SpellShapeBolt();
        spellEffects[0] = fireSpell;
    }

    // Start is called before the first frame update
    void Start()
    {
        followCamera = Camera.main.gameObject;
    }

    private void Update()
    {
        // TODO move this all to a coroutine
        // Get the rotation
        //float rotf = transform.rotation.eulerAngles.y;
        float rotf = 0f;
        followCameraRotation.y = rotf;
        followCamera.transform.rotation = Quaternion.Euler(followCameraRotation);
        // Orbit the camera based on rotation
        rotf *= Mathf.Deg2Rad;
        followCameraDisplacement.x = -followCamera2DDistance * Mathf.Sin(rotf);
        followCameraDisplacement.z = -followCamera2DDistance * Mathf.Cos(rotf);
        followCamera.transform.position = transform.position + followCameraDisplacement;

        // Get spell keydowns
        for (int i = 0; i < spellSlotsAvailable; ++i)
        {
            if (Input.GetButtonDown("Spell" + (i + 1)))
            {
                StartCoroutine("StartSpell", i);
            }
        }
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

    IEnumerator StartSpell(int spellSlot)
    {
        // create spell instance
        Vector3 startPos = transform.position;
        startPos.y += 0.5f;
        // Put the spell a bit in front of us
        startPos += transform.forward * 0.75f;
        Vector3 rotEuler = transform.rotation.eulerAngles;
        rotEuler.x = 90f;
        GameObject spellObject = Instantiate(emptySpellPrefab, startPos, Quaternion.Euler(rotEuler)) as GameObject;

        // Add the spell effects
        SpellScript spellScript = spellObject.GetComponent<SpellScript>();
        spellScript.spell = spellEffects[spellSlot];
        yield break;
    }
}
