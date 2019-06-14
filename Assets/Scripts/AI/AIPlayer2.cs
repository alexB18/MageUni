using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AIPlayer2 : Agent
{
    private readonly object _lock = new object();


    public float manaRechargeRate = 7;
    private StatScript stats;
    private bool canFire = true;

    public AICompanion companion;
    public GameObject[] enemies = new GameObject[4];
    private StatScript[] enemyStats = new StatScript[4];
    private EnemyAI[] enemyAIs = new EnemyAI[4];
    private Vector3 startPos;

    private Rigidbody rb;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;

    private const float deathScore = -5000;
    private const float winScore = 5000;
    private const float timeScoreMult = 1;
    private const float killScore = 500;
    private const float pacifistScore = 3500;
    private const float misfireScore = -5;
    
    private int startFrame;
    public bool isDead;

    public TMPro.TextMeshProUGUI[] playerFields;
    public TMPro.TextMeshProUGUI[] companionFields;
    public TMPro.TextMeshProUGUI[] Enemy1Fields;
    public TMPro.TextMeshProUGUI[] Enemy2Fields;
    public TMPro.TextMeshProUGUI[] Enemy3Fields;
    public TMPro.TextMeshProUGUI[] Enemy4Fields;
    private TMPro.TextMeshProUGUI[][] enemyFields = new TMPro.TextMeshProUGUI[4][];

    public GameObject emptySpellPrefab;

    private SpellModifierAIScoreTracker spellModifierAIScoreTracker = new SpellModifierAIScoreTracker();
    public SpellScript.Spell AIFirebolt;
    public SpellScript.Spell AIHealSelf;
    public SpellScript.Spell AIHealOther;
    public SpellScript.Spell AIFreeze;
    public SpellScript.Spell AIStun;
    SpellScript.Spell[] spells;
    private List<GameObject> castSpells = new List<GameObject>();

    private void OnDeath(Object[] obj)
    {
        //score += deathScore;
        AddReward(deathScore);
        isDead = true;
        Finish();
    }
    private void OnHealthChange(Object[] obj)
    {
        Float score = obj[1] as Float;
        AddReward(score.val);
    }
    private void OnEnemyDeath(Object[] obj)
    {
        GameObject source = obj[1] as GameObject;
        if (source != companion.gameObject)
        {
            lock (_lock)
            {
                AddReward(killScore);
            }

            GameObject enemy = obj[0] as GameObject;
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI.isBoss)
            {
                Finish();
            }
        }
    }
    private void OnEnemyHealthChange(Object[] obj)
    {
        GameObject source = obj[2] as GameObject;
        if (source != companion.gameObject)
        {
            Float score = obj[1] as Float;
            AddReward(-score.val);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Init spells
        spellModifierAIScoreTracker = new SpellModifierAIScoreTracker();
        spellModifierAIScoreTracker.agent = this;
        AIFirebolt = new SpellScript.Spell()
        {
            name = "Firebolt",
            shape = AllSpellsAndGlyphs.spellShapeBolt,
            components = new List<SpellComponent>() { AllSpellsAndGlyphs.spellEffectFire, spellModifierAIScoreTracker }
        };
        AIHealSelf = new SpellScript.Spell()
        {
            name = "Heal self",
            shape = AllSpellsAndGlyphs.spellShapeSelf,
            components = new List<SpellComponent>() { AllSpellsAndGlyphs.spellEffectHeal, spellModifierAIScoreTracker }
        };
        AIHealOther = new SpellScript.Spell()
        {
            name = "Heal other",
            shape = AllSpellsAndGlyphs.spellShapeBolt,
            components = new List<SpellComponent>() { AllSpellsAndGlyphs.spellEffectHeal, spellModifierAIScoreTracker }
        };
        AIFreeze = new SpellScript.Spell()
        {
            name = "Freeze",
            shape = AllSpellsAndGlyphs.spellShapeBolt,
            components = new List<SpellComponent>() { AllSpellsAndGlyphs.spellEffectFreeze, spellModifierAIScoreTracker }
        };
        AIStun = new SpellScript.Spell()
        {
            name = "Stun",
            shape = AllSpellsAndGlyphs.spellShapeBolt,
            components = new List<SpellComponent>() { AllSpellsAndGlyphs.spellEffectStun, spellModifierAIScoreTracker }
        };
        spells = new SpellScript.Spell[2]
        {
            AIFirebolt,
            AIFreeze
        };

        startPos = transform.localPosition;
        stats = GetComponent<StatScript>();
        rb = GetComponent<Rigidbody>();
        //AgentReset();
        startFrame = Time.frameCount;
        stats.SubscribeToOnDeath(OnDeath);
        stats.SubscribeToOnHealthChange(OnHealthChange);
        for (int i = 0; i < enemies.Length; ++i)
        {
            GameObject enemy = enemies[i];
            StatScript enemyStat = enemy.GetComponent<StatScript>();
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            enemyStats[i] = enemyStat;
            enemyAIs[i] = enemyAI;
            enemyStat.SubscribeToOnDeath(OnEnemyDeath);
            enemyStat.SubscribeToOnHealthChange(OnEnemyHealthChange);
        }

        enemyFields[0] = Enemy1Fields;
        enemyFields[1] = Enemy2Fields;
        enemyFields[2] = Enemy3Fields;
        enemyFields[3] = Enemy4Fields;
    }

    private void Update()
    {
        stats.ModifyMana(manaRechargeRate * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;
    }

    public void Finish()
    {
        Score();
        companion.Done();
        Done();
    }

    private void Score()
    {
        float score = 0f;
        if (!isDead)
        {
            score += winScore;
        }
        AddReward(score);
    }

    public float GetScore()
    {
        return GetCumulativeReward();
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
        transform.Rotate(0f, a * rotationSpeed, 0f);
    }

    private void Fire(int index)
    {
        SpellScript.Spell spell = spells[index];
        if (spell != null)
        {
            float manaCost = -spell.ManaCost();
            if (stats.ModifyMana(-10))
            {
                lock (_lock) canFire = false;
                companion.didFire = true;
                StartCoroutine(FireCooldown());
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
                castSpells.Add(spellObject);
            }
        }
    }

    private bool isTrue(float f)
    {
        return f >= 0.5f;
    }

    public void AddScore(float f)
    {
        AddReward(f);
    }

    public override void CollectObservations()
    {
        // Player
        AddVectorObs(stats.CurrentHealth);
        AddVectorObs(stats.CurrentMana);
        if(playerFields != null && playerFields.Length > 0)
        {
            playerFields[0].text = stats.CurrentHealth.ToString();
            playerFields[1].text = stats.CurrentMana.ToString();
        }
        // Companion
        Vector3 disp;
        {
            disp = companion.transform.localPosition - transform.localPosition;
            float d = disp.magnitude;
            float a = companion.transform.rotation.eulerAngles.y - Quaternion.LookRotation(disp).eulerAngles.y;
            float b = Quaternion.LookRotation(disp).eulerAngles.y - transform.rotation.eulerAngles.y;
            if (a > 180f) a -= 360f; else if (a < -180f) a += 360f ;
            if (b > 180f) b -= 360f;
            AddVectorObs(d);
            AddVectorObs(a);
            AddVectorObs(b);
            if(companionFields != null && companionFields.Length > 0)
            {
                companionFields[0].text = d.ToString();
                companionFields[1].text = a.ToString();
                companionFields[2].text = b.ToString();
            }
        }
        // Enemies
        for (int i = 0; i < enemies.Length; ++i)
        {
            GameObject enemy = enemies[i];
            disp = enemy.transform.localPosition - transform.localPosition;
            float d = disp.magnitude;
            float a = enemy.transform.rotation.eulerAngles.y - Quaternion.LookRotation(disp).eulerAngles.y;
            float b = Quaternion.LookRotation(disp).eulerAngles.y - transform.rotation.eulerAngles.y;
            if (a > 180f) a = 180f - a; else if (a < -180f) a = -180f - a;
            if (b > 180f) b -= 360f;
            float h = enemyStats[i].CurrentHealth;
            bool isBoss = enemyAIs[i].isBoss;
            AddVectorObs(d);
            AddVectorObs(a);
            AddVectorObs(b);
            AddVectorObs(isBoss);
            AddVectorObs(h);

            if(enemyFields != null && enemyFields[i].Length > 0)
            {
                enemyFields[i][0].text = d.ToString();
                enemyFields[i][1].text = a.ToString();
                enemyFields[i][2].text = b.ToString();
                enemyFields[i][3].text = h.ToString();
                enemyFields[i][4].text = isBoss ? "T" : "F";
            }
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Move
        Move(vectorAction[0], vectorAction[1]);
        // Rotate
        Rotate(vectorAction[2]);
        // Fire spell
        bool firedTwice = false;
        int spellIndex = -1;
        if (canFire)
            for (int i = 0; i < spells.Length; ++i)
            {
                if (isTrue(vectorAction[i + 3]))
                {
                    if (spellIndex != -1)
                        firedTwice = true;
                    spellIndex = i;
                }
            }

        if (firedTwice)
            AddReward(misfireScore);
        else if(spellIndex != -1)
            Fire(spellIndex);

    }

    public override void AddReward(float increment)
    {
        base.AddReward(increment);
        companion.AddReward(increment);
        companion.AddCumulativeReward(increment);
    }

    public override void AgentReset()
    {
        foreach(GameObject spell in castSpells)
        {
            if(spell != null)
            {
                Destroy(spell);
            }
        }
        castSpells.Clear();
        // If the Agent fell, zero its momentum
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        transform.localPosition = startPos;
        transform.localRotation = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);
        

        stats.AIReset();
        isDead = false;

        // Reset enemies
        for (int i = 0; i < enemies.Length; ++i)
            enemyAIs[i].AIReset();

        Shuffle();
    }

    public override void AgentOnDone()
    {
        base.AgentOnDone();
    }

    private IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(0.25f);
        lock (_lock)
            canFire = true;
    }


    public void Shuffle()
    {
        int n = enemies.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            // Enemies
            var e = enemies[k];
            enemies[k] = enemies[n];
            enemies[n] = e;
            // Enemies Stats
            var s = enemyStats[k];
            enemyStats[k] = enemyStats[n];
            enemyStats[n] = s;
            // Enemies AIs
            var a = enemyAIs[k];
            enemyAIs[k] = enemyAIs[n];
            enemyAIs[n] = a;
        }
    }
}
