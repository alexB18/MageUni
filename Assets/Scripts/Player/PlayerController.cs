using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerSceneChange sceneManager;

    // LOCK and CAN ROTATE
    private readonly object _lock = new object();
    private bool canRotate = true;

    private Coroutine coroutine = null;

    /* ------------------------  Player variables   ------------------- */
    public float moveSpeed = 3.5f;
    public float turnSpeed = 10;
    public float cameraRotateSpeed = 45f;
    private float maxPlayerSpeed = 3.5f;
    private float currentPlayerSpeed;
    private Animator anim;
    private Rigidbody playerRb;
    
    private static readonly float walkScale = 0.33f;

    // Follow camera
    public GameObject followCamera;
    public float followCamera2DDistance = 7.5f;
    public float followCameraHeight = 7.5f;
    private Vector3 followCameraDisplacement;
    public Vector3 followCameraRotation = new Vector3(45f, 0f, 0f);

    // Spell variables
    public GameObject emptySpellPrefab;
    public const int spellMaxSpellSlots = 5;
    private int spellSlotsAvailable = 3;
    private int activeSpellSlot = 0;
    private PlayerSpellInventory spellInventory;

    private SpellScript.Spell[] spells = new SpellScript.Spell[spellMaxSpellSlots];
    public SpellScript.Spell[] Spells => spells;

    public int SpellSlotsAvailable { get => spellSlotsAvailable; set => spellSlotsAvailable = value; }
    public void ChangeSpellSlot(int newSlot) => activeSpellSlot = newSlot;
    public void SetSpell(int slot, SpellScript.Spell spell) => spells[slot] = spell;

    public bool campusPracticeArena = false;

    // Health and mana
    private StatScript stats;
    public float manaRechargeRate = 5;

    [HideInInspector] public bool isAlive = true;
    public GameObject deathScreen;
    private const float timeUntilReload = 3f;

    private void OnDeath(Object[] obj)
    {
        isAlive = false;
        ragdoll();
        StartCoroutine("DieAndRestart");
    }

    // Pickup Variables
    private int numKeys;
    private int healthPotionCount = 0;
    private int manaPotionCount = 0;
    private List<Subscriber> onKeyChangeSubscribers = new List<Subscriber>();
    private List<Subscriber> onHealthPotionChangeSubscribers = new List<Subscriber>();
    private List<Subscriber> onManaPotionChangeSubscribers = new List<Subscriber>();

    public void SubscribeToOnKeyChange(Subscriber sub)
    {
        onKeyChangeSubscribers.Add(sub);
    }
    public void UnsubscribeToOnKeyChange(Subscriber sub)
    {
        onKeyChangeSubscribers.Remove(sub);
    }
    public void SubscribeToOnHealthPotionChange(Subscriber sub)
    {
        onHealthPotionChangeSubscribers.Add(sub);
    }
    public void UnsubscribeToOnHealthPotionChange(Subscriber sub)
    {
        onHealthPotionChangeSubscribers.Remove(sub);
    }
    public void SubscribeToOnManaPotionChange(Subscriber sub)
    {
        onManaPotionChangeSubscribers.Add(sub);
    }
    public void UnsubscribeToOnManaPotionChange(Subscriber sub)
    {
        onManaPotionChangeSubscribers.Remove(sub);
    }

    private void OnKeyChange()
    {
        foreach (Subscriber sub in onKeyChangeSubscribers)
            sub(this);
    }
    private void OnHealthPotionChange()
    {
        foreach (Subscriber sub in onHealthPotionChangeSubscribers)
            sub(this);
    }
    private void OnManaPotionChange()
    {
        foreach (Subscriber sub in onManaPotionChangeSubscribers)
            sub(this);
    }

    public int NumKeys { get => numKeys; set { numKeys = value; OnKeyChange(); } }
    public int HealthPotionCount { get => healthPotionCount; set { healthPotionCount = value; OnHealthPotionChange(); } }
    public int ManaPotionCount { get => manaPotionCount; set { manaPotionCount = value; OnManaPotionChange(); } }


    // Interaction trigger
    public GameObject interactionTrigger;
    private Coroutine interactionTimer;
    private const float interactionTime = 0.25f;

    // Called before first frame update regardless of script being enabled or not
    private void Awake()
    {
        followCameraDisplacement = new Vector3(0.0f, followCameraHeight, -followCamera2DDistance);
        anim = GetComponent<Animator>();            //Retrieve animator from player object
        playerRb = GetComponent<Rigidbody>();       //Retrieve rigidbody from player object

        // TODO remove PoC spells
        /*
        SpellScript.Spell fireSpell = new SpellScript.Spell();
        fireSpell.components.Add(new SpellEffectFire());
        fireSpell.shape = new SpellShapeBolt();
        spells[0] = fireSpell;
        //*/
        spellInventory = GetComponent<PlayerSpellInventory>();
        // TODO remove test effects
        spellInventory.AddGlyph(AllSpellsAndGlyphs.boltGlyph);
        spellInventory.AddGlyph(AllSpellsAndGlyphs.fireGlyph);
    }

    // Start is called before the first frame update
    void Start()
    {
        numKeys = 0;
        followCamera = Camera.main.gameObject;
        followCamera.transform.rotation = Quaternion.Euler(followCameraRotation);
        stats = gameObject.GetComponent<StatScript>() as StatScript;
        stats.SubscribeToOnDeath(OnDeath);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Interactable"))
        {
            // Deactivate Object being interacted with
            interactionTrigger.SetActive(false);

            
            if(interactionTimer != null)
                StopCoroutine(interactionTimer);

            other.gameObject.GetComponent<Interactable>().Interact(gameObject);
            
        }
    }

    private void FixedUpdate()
    {
        // If we're alive and 
        if (isAlive && !IsPaused())
        {
            // TODO move this to a coroutine
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            CameraUpdate();
            SimpleMove(h, v);
            MouseTurn();
            Animating(h, v);
        }
    }

    private void Update()
    {
        // If we're alive and 
        if (isAlive && !IsPaused())
        {
            if(!interactionTrigger.activeSelf && Input.GetButtonDown("Interact"))
            {
                interactionTrigger.SetActive(true);
                interactionTimer = StartCoroutine("InteractionTimer");
            }
            stats.ModifyMana(manaRechargeRate * Time.deltaTime);
            /*
            if (healthBar != null)
                healthBar.value = healthScript.currentHealth / healthScript.maximumHealth;
            //*/
            

            // Get spell keydowns to switch our spell
            for (int i = 0; i < SpellSlotsAvailable; ++i)
                if (Input.GetButtonDown("Spell" + (i + 1)))
                    activeSpellSlot = i;

            if(Input.GetButtonDown("Fire") && (!sceneManager.CampusScene() || QuestStage.Quest == QuestStage.Quests.Demon || campusPracticeArena))
                StartCoroutine("StartSpell");

           
            // Health pickups
            if (Input.GetButtonDown("Health Potion") && healthPotionCount > 0)
            {
                --healthPotionCount;
                stats.RestoreHealth(25);
            }

            // Mana Pickups
            if (Input.GetButtonDown("Mana Potion") && manaPotionCount > 0)
            {
                --manaPotionCount;
                stats.RestoreMana(75);
            }


        }
    }

    private bool IsPaused() => Time.timeScale == 0;

    /*
    // Deprecated
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
    */

    private void SimpleMove(float h, float v)
    {
        Vector3 moveVector = followCamera.transform.forward * v + followCamera.transform.right * h;
        //float directionMagnitude = moveVector.magnitude; // Store initial direction magnitude for normalization
        moveVector.y = 0f;   // Ensure lateral movement
        moveVector.Normalize();
        //moveVector = moveVector.normalized * directionMagnitude; // Normalize direction vector

        moveVector *= moveSpeed;
        if (Input.GetButton("Walk")) moveVector *= walkScale;

        moveVector.y = playerRb.velocity.y;
        playerRb.velocity = moveVector;
        anim.SetFloat("MoveSpeed", playerRb.velocity.magnitude / maxPlayerSpeed);
    }

    private void CameraRotateKeys()
    {
        if (canRotate)
        {
            if (Input.GetButtonDown("Camera Horizontal Left"))
            {
                lock (_lock)
                    canRotate = false;
                followCameraRotation.y -= cameraRotateSpeed;
                followCamera.transform.rotation = Quaternion.Euler(followCameraRotation);
                StartCoroutine(KeyWait());
            }
            else if (Input.GetButtonDown("Camera Horizontal Right"))
            {
                lock (_lock)
                    canRotate = false;
                followCameraRotation.y += cameraRotateSpeed;
                followCamera.transform.rotation = Quaternion.Euler(followCameraRotation);
                StartCoroutine(KeyWait());
            }
        }
        
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
        /*
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
        //*/
    }

    private void MouseTurn()
    {
        // We need the direction vector from the screen where the origin is the
        // middle of the screen
        Vector2 screenMiddle = new Vector2(0.5f, 0.5f);
        Vector2 mousePos = Input.mousePosition;
        mousePos.x /= Screen.width;
        mousePos.y /= Screen.height;
        Vector2 direction = mousePos - screenMiddle;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + followCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    void Animating(float h, float v)
    {
        // If moving, walking -> true
        bool moving = h != 0f || v != 0f;
        anim.SetBool("IsMoving", moving);
    }

    IEnumerator StartSpell()
    {
        SpellScript.Spell spell = spells[activeSpellSlot];
        if (spell != null)
        {
            float manaCost = -spell.ManaCost();
            if (stats.ModifyMana(manaCost))
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
                spellScript.spell = spell;
                spellScript.parent = gameObject;
            }
        }
        yield break;
    }

    IEnumerator DieAndRestart()
    {
        yield return new WaitForSeconds(timeUntilReload);
        deathScreen.SetActive(true);
    }

    IEnumerator InteractionTimer()
    {
        yield return new WaitForSeconds(interactionTime);
        interactionTrigger.SetActive(false);
        interactionTimer = null;
    }

    public void ragdoll()
    {
        Animator animator = gameObject.GetComponent<Animator>() as Animator;
        playerRb.freezeRotation = false;
        animator.enabled = false;
    }

    private IEnumerator KeyWait(float f = 0.05f)
    {
        yield return new WaitForSecondsRealtime(f);
        lock (_lock) canRotate = true;
    }
}
