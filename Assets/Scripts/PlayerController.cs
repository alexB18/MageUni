using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    /* ------------------------  Player variables   ------------------- */
    public float moveSpeed = 15f;
    public float turnSpeed = 10;
    public float cameraRotateSpeed = 3f;
    private float maxPlayerSpeed = 3f;
    private float currentPlayerSpeed;
    private Animator anim;
    private Rigidbody playerRb;

    // Track player's current vertical/horizontal movement
    private float currentVertical = 0f;
    private float currentHorizontal = 0f;

    private static readonly float interpConst = 10;
    private static readonly float walkScale = 0.33f;


    private Vector3 currentDirection = Vector3.zero;    // Track players current direction

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

    // Health and mana
    private HealthScript healthScript;
    public float maximumMana = 100;
    public float manaRechargeRate = 5f;
    private float currentMana;
    public Slider healthBar;
    public Slider manaBar;
    [HideInInspector] public bool isAlive = true;
    private float timeUntilReload = 5f;
    public GameObject deathScreen;

    private readonly Subscriber deathListener = delegate (Object[] obj)
    {
        GameObject gameObject = obj[0] as GameObject;
        PlayerController pc = gameObject.GetComponent<PlayerController>();
        pc.isAlive = false;
        pc.ragdoll();
        pc.StartCoroutine("DieAndRestart");
    };
    private readonly Subscriber healthChangeListener = delegate (Object[] obj)
    {
        HealthScript hs = obj[0] as HealthScript;
        PlayerController pc = hs.gameObject.GetComponent<PlayerController>();
        pc.healthBar.value = hs.currentHealth / hs.maximumHealth;
    };

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

        currentMana = maximumMana;
    }

    // Start is called before the first frame update
    void Start()
    {
        followCamera = Camera.main.gameObject;
        healthScript = gameObject.GetComponent<HealthScript>() as HealthScript;
        healthScript.SubscribeToOnDeath(deathListener);
        healthScript.SubscribeToOnHealthChange(healthChangeListener);
        healthScript.OnHealthChange();
    }

    private void Update()
    {
        if (isAlive)
        {
            currentMana += manaRechargeRate * Time.deltaTime;
            if (manaBar != null)
                manaBar.value = currentMana / maximumMana;
            /*
            if (healthBar != null)
                healthBar.value = healthScript.currentHealth / healthScript.maximumHealth;
            //*/
            // TODO move this to a coroutine
            CameraUpdate();

            // Get spell keydowns
            for (int i = 0; i < spellSlotsAvailable; ++i)
            {
                if (Input.GetButtonDown("Spell" + (i + 1)))
                {
                    SpellScript.Spell spell = spellEffects[i];
                    float manaCost = spell.ManaCost();
                    if (currentMana > manaCost)
                    {
                        currentMana -= manaCost;
                        StartCoroutine("StartSpell", i);
                    }
                }
            }
        }
    }

    // Used primarily for physics
    private void FixedUpdate()
    {
        if (isAlive)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            //Move(h, v);
            SimpleMove(h, v);
            //CameraRotate();
            //Turning();
            Animating(h, v);
        }
    }

    private void Move(float h, float v)
    {
     
        // Track camera location
        Transform camera = followCamera.transform;

        // If leftshift, slow the heck down
        if (Input.GetButton("Walk"))
        {
            v *= walkScale;
            h *= walkScale;
        }

        // Recalculate current horizontal/vertical movement from input/ use interpConst to accelerate
        currentVertical = Mathf.Lerp(currentVertical, v, Time.deltaTime * interpConst);
        currentHorizontal = Mathf.Lerp(currentHorizontal, h, Time.deltaTime * interpConst);

        // Vector describing player direction
        Vector3 direction = camera.forward * currentVertical + camera.right * currentHorizontal;

        float directionMagnitude = direction.magnitude; // Store initial direction magnitude for normalization
        direction.y = 0f;   // Ensure lateral movement
        direction = direction.normalized * directionMagnitude; // Normalize direction vector


        // If player direction != 0 update player position and animator
        if(direction != Vector3.zero)
        {
            anim.SetFloat("MoveSpeed", direction.magnitude);
            currentDirection = Vector3.Slerp(currentDirection, direction, Time.deltaTime * interpConst);

            transform.rotation = Quaternion.LookRotation(currentDirection);
            transform.position += currentDirection * moveSpeed * Time.deltaTime;
        }
    }

    private void SimpleMove(float h, float v)
    {
        Vector3 moveVector = followCamera.transform.forward * v + followCamera.transform.right * h;
        float directionMagnitude = moveVector.magnitude; // Store initial direction magnitude for normalization
        moveVector.y = 0f;   // Ensure lateral movement
        moveVector = moveVector.normalized * directionMagnitude; // Normalize direction vector
        moveVector *= moveSpeed;
        //currentPlayerSpeed = moveVector.magnitude;  //Retrieive speed of current player for global use

        if (moveVector != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVector);
            float adjMaxSpeed = Input.GetButton("Walk") ? maxPlayerSpeed * walkScale : maxPlayerSpeed;
            playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, adjMaxSpeed);
            playerRb.AddForce(moveVector);

            anim.SetFloat("MoveSpeed", playerRb.velocity.magnitude / maxPlayerSpeed);
        }
    }

    private void CameraRotateKeys()
    {
        float degree = Input.GetAxis("Camera Horizontal");
        followCameraRotation.y += degree * cameraRotateSpeed;
        followCamera.transform.rotation = Quaternion.Euler(followCameraRotation);
        
        /*
        if( currentPlayerSpeed == 0)
        {
            transform.rotation = Quaternion.Euler(followCameraRotation);
        }
        //*/
    }

    private void CameraRotateMovement()
    {
        // Get the rotation
        float rotf = transform.rotation.eulerAngles.y;
        followCameraRotation.y = rotf;
        followCamera.transform.rotation = Quaternion.Euler(followCameraRotation);
    }

    private void CameraUpdate()
    {
        CameraRotateKeys();
        // Orbit the camera based on rotation
        float rotf = followCameraRotation.y * Mathf.Deg2Rad;
        followCameraDisplacement.x = -followCamera2DDistance * Mathf.Sin(rotf);
        followCameraDisplacement.z = -followCamera2DDistance * Mathf.Cos(rotf);
        followCamera.transform.position = transform.position + followCameraDisplacement;
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
        bool moving = h != 0f || v != 0f;
        anim.SetBool("IsMoving", moving);
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

    IEnumerator DieAndRestart()
    {
        yield return new WaitForSeconds(timeUntilReload);
        deathScreen.SetActive(true);
    }

    public void ragdoll()
    {
        Animator animator = gameObject.GetComponent<Animator>() as Animator;
        playerRb.freezeRotation = false;
        animator.enabled = false;
    }
}
