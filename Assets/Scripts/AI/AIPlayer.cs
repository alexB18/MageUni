using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AIPlayer : Agent
{
    private readonly object _lock = new object();
    private const float manaRechargeRate = 5;
    private StatScript stats;
    public int healthPotionCount = 0;
    public int manaPotionCount = 0;
    public int keyCount = 0;
    private bool interact = false;

    public GameObject companion;
    public GameObject enemiesContainer;
    private List<StatScript> enemyStats = new List<StatScript>();
    public GameObject interactablesContainer;
    public GameObject doorsContainer;
    private List<AIDoorScript> doorScripts = new List<AIDoorScript>();

    private GameObject perceived;
    private bool isCompanion = false;
    private bool isEnemy = false;
    private bool isDoor = false;
    private bool isInteractable = false;

    private Rigidbody rb;
    public float moveSpeed = 3f;

    private const int deathScore = -10000;
    private const int maxFrames = 7500;
    private const int timeScoreMult = 1;
    private const int killScore = 500;
    private const int pacifistScore = 15000;

    private int score;
    private int startFrame;
    private bool isDead;
    private int enemyCount;
    private int frozenCount;
    private bool isPacifist;

    public GameObject emptySpellPrefab;
    public static SpellScript.Spell AIFirebolt = new SpellScript.Spell()
    {
        name = "Firebolt",
        shape = AllSpellsAndGlyphs.spellShapeBolt,
        components = new List<SpellComponent>() { AllSpellsAndGlyphs.spellEffectFire }
    };
    public static SpellScript.Spell AIHealSelf = new SpellScript.Spell()
    {
        name = "Heal self",
        shape = AllSpellsAndGlyphs.spellShapeSelf,
        components = new List<SpellComponent>() { AllSpellsAndGlyphs.spellEffectHeal }
    };
    public static SpellScript.Spell AIHealOther = new SpellScript.Spell()
    {
        name = "Heal other",
        shape = AllSpellsAndGlyphs.spellShapeBolt,
        components = new List<SpellComponent>() { AllSpellsAndGlyphs.spellEffectHeal }
    };
    public static SpellScript.Spell AIFreeze = new SpellScript.Spell()
    {
        name = "Freeze",
        shape = AllSpellsAndGlyphs.spellShapeBolt,
        components = new List<SpellComponent>() { AllSpellsAndGlyphs.spellEffectFreeze }
    };
    public static SpellScript.Spell AIStun = new SpellScript.Spell()
    {
        name = "Stun",
        shape = AllSpellsAndGlyphs.spellShapeBolt,
        components = new List<SpellComponent>() { AllSpellsAndGlyphs.spellEffectStun }
    };
    static SpellScript.Spell[] spells = new SpellScript.Spell[5]
        {
            AIFirebolt,
            AIHealSelf,
            AIHealOther,
            AIFreeze,
            AIStun
        };

    private void OnDeath(Object[] obj)
    {
        score += deathScore;
        isDead = true;
        Finish();
    }

    private void OnHealthChange(Object[] obj)
    {
        Debug.Log("Player health at " + stats.currentHealth);
    }

    private void OnEnemyDeath(Object[] obj)
    {
        lock (_lock)
        {
            score += killScore;
            StatScript enemyStats = obj[0] as StatScript;
            if (enemyStats.GetComponent<EnemyAI>().isBoss)
            {
                Finish();
            }
        }
    }

    private void OnEnemyHealthChange(Object[] obj)
    {
        isPacifist = false;
    }

    private void OnEnemyFreeze(Object[] obj)
    {
        lock (_lock)
        {
            frozenCount += 1;
            if (isPacifist && frozenCount == enemyCount)
                Finish();
        }
    }

    private void OnEnemyFreezeEnd(Object[] obj)
    {
        lock (_lock) frozenCount -= 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<StatScript>();
        rb = GetComponent<Rigidbody>();
        AgentReset();
        startFrame = Time.frameCount;
        enemyCount = enemiesContainer.transform.childCount;
        stats.SubscribeToOnDeath(OnDeath);
        stats.SubscribeToOnHealthChange(OnHealthChange);
        foreach (Transform enemyTransform in enemiesContainer.transform)
        {
            StatScript enemyStat = enemyTransform.GetComponent<StatScript>();
            enemyStats.Add(enemyStat);
            enemyStat.SubscribeToOnDeath(OnEnemyDeath);
            enemyStat.SubscribeToOnHealthChange(OnEnemyHealthChange);
            enemyStat.SubscribeToOnFreeze(OnEnemyFreeze);
            enemyStat.SubscribeToOnFreezeEnd(OnEnemyFreezeEnd);
        }
        foreach (Transform doorTransform in doorsContainer.transform)
        {
            doorScripts.Add(doorTransform.GetComponent<AIDoorScript>());
        }
    }

    private void Update()
    {
        stats.ModifyMana(manaRechargeRate * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerStay(Collider other)
    {
        if(perceived == null)
        {
            perceived = other.gameObject;
            isCompanion = perceived == companion;
            isEnemy = !isCompanion && perceived.CompareTag("BadGuy");
            isDoor = !isEnemy && !isCompanion && perceived.GetComponent<AIDoorScript>() != null;
            isInteractable = !isDoor && perceived.CompareTag("Interactable");
        }
        if (interact && other.CompareTag("Interactable"))
            other.gameObject.GetComponent<Interactable>().Interact(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == perceived)
            perceived = null;
    }

    private void Finish()
    {
        Score();
        Done();
    }

    private void Score()
    {
        if (isPacifist)
            score += pacifistScore;
        if (!isDead)
        {
            int framesPassed = maxFrames - (Time.frameCount - startFrame);
            score += framesPassed * timeScoreMult;
        }
        SetReward(score);
    }

    private void Move(float x, float z)
    {
        Vector3 moveVector = transform.right * x + transform.forward * z;
        moveVector = Vector3.ClampMagnitude(moveVector, moveSpeed);

        moveVector *= moveSpeed;
        moveVector.y = rb.velocity.y;
        rb.velocity = moveVector;
    }

    private void Rotate(float a)
    {
        //transform.rotation = Quaternion.Euler(0, a, 0);
        transform.Rotate(0f, a, 0f);
    }

    private void Fire(int index)
    {
        SpellScript.Spell spell = spells[index];
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
    }

    private void UseHealthPotion()
    {
        --healthPotionCount;
        stats.RestoreHealth(25);
    }

    private void UseManaPotion()
    {
        --manaPotionCount;
        stats.RestoreMana(75);
    }

    private void Interact(bool b=false)
    {
        interact = b;
    }

    private bool isTrue(float f)
    {
        return f >= 0.5f;
    }

    public override void CollectObservations()
    {
        // Player
        AddVectorObs(stats.CurrentHealth);
        AddVectorObs(stats.CurrentMana);
        AddVectorObs(transform.localPosition.x);
        AddVectorObs(transform.localPosition.z);
        AddVectorObs(keyCount);
        AddVectorObs(healthPotionCount);
        AddVectorObs(manaPotionCount);
        // Companion
        Vector3 disp;
        disp = companion.transform.localPosition - transform.localPosition;
        AddVectorObs(disp.x);
        AddVectorObs(disp.z);
        float closestDist = 100000f;
        // Nearest enemy
        closestDist = 100000f;
        GameObject enemy = null;
        foreach(StatScript e in enemyStats)
        {
            if (!e.IsDead)
            {
                float dist = (e.transform.localPosition - transform.localPosition).sqrMagnitude;
                if (dist < closestDist)
                {
                    closestDist = dist;
                    enemy = e.gameObject;
                }
            }
        }
        if (enemy)
        {
            disp = enemy.transform.localPosition - transform.localPosition;
            AddVectorObs(disp.x);
            AddVectorObs(disp.z);
            AddVectorObs(enemy.GetComponent<EnemyAI>().isBoss);
        }
        else
        {
            AddVectorObs(1000f);
            AddVectorObs(1000f);
            AddVectorObs(false);
        }

        // Nearest door
        closestDist = 100000f;
        AIDoorScript door = null;
        foreach (AIDoorScript d in doorScripts)
        {
            float dist = (d.transform.localPosition - transform.localPosition).sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                door = d;            }
        }
        if (door)
        {
            disp = door.transform.localPosition - transform.localPosition;
            AddVectorObs(disp.x);
            AddVectorObs(disp.z);
            AddVectorObs(door.isLocked);
        }
        else
        {
            AddVectorObs(1000f);
            AddVectorObs(1000f);
            AddVectorObs(false);
        }
        // Nearest interactable
        closestDist = 100000f;
        GameObject interactable = null;
        foreach (Transform d in doorsContainer.transform)
        {
            float dist = (d.transform.localPosition - transform.localPosition).sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                interactable = d.gameObject;
            }
        }
        if (interactable)
        {
            disp = interactable.transform.localPosition - transform.localPosition;
            AddVectorObs(disp.x);
            AddVectorObs(disp.z);
        }
        else
        {
            AddVectorObs(1000f);
            AddVectorObs(1000f);
        }
        // Front object
        if(perceived)
        {
            disp = perceived.transform.localPosition - transform.localPosition;
            AddVectorObs(disp.x);
            AddVectorObs(disp.z);
            AddVectorObs(isCompanion);
            AddVectorObs(isEnemy);
            AddVectorObs(isDoor);
            AddVectorObs(isInteractable);
        }
        else
        {
            AddVectorObs(1000f);
            AddVectorObs(1000f);
            AddVectorObs(false);
            AddVectorObs(false);
            AddVectorObs(false);
            AddVectorObs(false);
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        base.AgentAction(vectorAction, textAction);
        // Move
        Move(vectorAction[0], vectorAction[1]);
        // Rotate
        Rotate(vectorAction[2]);
        // Fire spell
        for (int i = 0; i < 5; ++i)
        {
            if (isTrue(vectorAction[i + 3]))
            {
                Fire(i);
                break;
            }
        }
        // Health potion
        if (isTrue(vectorAction[8]))
            UseHealthPotion();
        // Mana potion
        if (isTrue(vectorAction[9]))
            UseManaPotion();
        // Interact
        Interact(isTrue(vectorAction[10]));

    }

    public override void AgentReset()
    {
        // If the Agent fell, zero its momentum
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        transform.localPosition = new Vector3(7.5f, 1f, 0f);

        stats.AIReset();
        score = 0;
        startFrame = Time.frameCount;
        isDead = false;
        frozenCount = 0;
        isPacifist = true;

        // Reset enemies
        foreach (Transform t in enemiesContainer.transform)
            t.GetComponent<EnemyAI>().AIReset();
        // Reset Doors
        foreach (Transform t in doorsContainer.transform)
            t.gameObject.SetActive(true);
        // Reset interactables
        foreach (Transform t in interactablesContainer.transform)
            t.gameObject.SetActive(true);
    }

    public override void AgentOnDone()
    {
        Score();
        base.AgentOnDone();
    }
    
}
