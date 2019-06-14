using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AICompanion : Agent
{
    private readonly object _lock = new object();
    // Need to be locked


    public float manaRechargeRate = 7;
    private StatScript stats;
    private bool canFire = true;
    private int numEpisodes = 0;
    private float cumulativeScore = 0f;
    public bool isDead = false;

    public AIPlayer2 player;
    public StatScript playerStats;
    public bool didFire = false;
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

    public TMPro.TextMeshProUGUI[] selfFields;
    public TMPro.TextMeshProUGUI scoreField;
    public TMPro.TextMeshProUGUI avgScoreField;
    public TMPro.TextMeshProUGUI[] playerFields;
    public TMPro.TextMeshProUGUI[] Enemy1Fields;
    public TMPro.TextMeshProUGUI[] Enemy2Fields;
    public TMPro.TextMeshProUGUI[] Enemy3Fields;
    public TMPro.TextMeshProUGUI[] Enemy4Fields;
    private TMPro.TextMeshProUGUI[][] enemyFields = new TMPro.TextMeshProUGUI[4][];

    public GameObject emptySpellPrefab;

    private SpellModifierAIScoreTracker spellModifierAIScoreTracker = new SpellModifierAIScoreTracker();
    private SpellShapeTarget spellShapeSelf = new SpellShapeTarget();
    private SpellShapeTarget spellShapePlayer = new SpellShapeTarget();
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
        isDead = true;
        AddReward(deathScore);
        AddCumulativeReward(deathScore);
        Finish();
    }
    private void OnHealthChange(Object[] obj)
    {
        Float score = obj[1] as Float;
        AddReward(score.val);
        AddCumulativeReward(score.val);
    }
    private void OnPlayerDeath(Object[] obj)
    {
        AddReward(deathScore * 3f);
        AddCumulativeReward(deathScore * 3f);
        isDead = true;
        Finish();
    }
    private void OnPlayerHealthChange(Object[] obj)
    {
        float score = (obj[1] as Float).val;
        if (score < 0f)
            score *= 6f;
        else
            score *= 3f;
        AddReward(score);
        AddCumulativeReward(score);
    }
    /*
    private void OnEnemyDeath(Object[] obj)
    {
        lock (_lock)
        {
            AddReward(killScore / 2f);
            GameObject enemy = obj[0] as GameObject;
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI.isBoss)
            {
                player.Finish();
            }
        }
    }
    private void OnEnemyHealthChange(Object[] obj)
    {
        Float score = obj[1] as Float;
        AddReward(-score.val / 2f);
    }
    //*/

    private void OnEnemyDeath(Object[] obj)
    {
        GameObject source = obj[1] as GameObject;
        if (source != player.gameObject)
        {
            lock (_lock)
            {
                AddReward(killScore / 2f);
                AddCumulativeReward(killScore / 2f);
            }

            GameObject enemy = obj[0] as GameObject;
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI.isBoss)
            {
                player.Finish();
            }
        }
    }
    private void OnEnemyHealthChange(Object[] obj)
    {
        GameObject source = obj[2] as GameObject;
        if (source != player.gameObject)
        {
            Float score = obj[1] as Float;
            AddReward(-score.val / 2f);
            AddCumulativeReward(-score.val / 2f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Init spells
        spellModifierAIScoreTracker = new SpellModifierAIScoreTracker();
        spellModifierAIScoreTracker.agent = this;
        spellShapeSelf.target = gameObject;
        spellShapePlayer.target = player.gameObject;
        AIFirebolt = new SpellScript.Spell()
        {
            name = "Firebolt",
            shape = AllSpellsAndGlyphs.spellShapeBolt,
            components = new List<SpellComponent>() { AllSpellsAndGlyphs.spellEffectFire, spellModifierAIScoreTracker }
        };
        AIHealSelf = new SpellScript.Spell()
        {
            name = "Heal self",
            shape = spellShapeSelf,
            components = new List<SpellComponent>() { AllSpellsAndGlyphs.spellEffectHeal, spellModifierAIScoreTracker }
        };
        AIHealOther = new SpellScript.Spell()
        {
            name = "Heal other",
            shape = spellShapePlayer,
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
        spells = new SpellScript.Spell[4]
        {
            AIFirebolt,
            AIHealSelf,
            AIHealOther,
            AIStun
        };

        startPos = transform.localPosition;
        stats = GetComponent<StatScript>();
        rb = GetComponent<Rigidbody>();

        //AgentReset();
        stats.SubscribeToOnDeath(OnDeath);
        stats.SubscribeToOnHealthChange(OnHealthChange);
        playerStats = player.GetComponent<StatScript>();
        playerStats.SubscribeToOnDeath(OnPlayerDeath);
        playerStats.SubscribeToOnHealthChange(OnPlayerHealthChange);
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

    private void Finish()
    {
        player.isDead = player.isDead | isDead;
        player.Finish();
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
                StartCoroutine(FireCooldown());
                // create spell instance
                Vector3 startPos = transform.position;
                startPos.y += 0.5f;
                // Put the spell a bit in front of us
                startPos += transform.forward * 1f;
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
        // Self
        AddVectorObs(stats.CurrentHealth);
        AddVectorObs(stats.CurrentMana);
        if (selfFields != null)
        {
            selfFields[0].text = stats.CurrentHealth.ToString();
            selfFields[1].text = stats.CurrentMana.ToString();
        }
        if(scoreField != null)
        {
            scoreField.text = GetCumulativeReward().ToString();
        }
        // Player
        Vector3 disp;

        {
            disp = player.transform.localPosition - transform.localPosition;
            float d = disp.magnitude;
            float a = player.transform.rotation.eulerAngles.y - Quaternion.LookRotation(disp).eulerAngles.y;
            float b = Quaternion.LookRotation(disp).eulerAngles.y - transform.rotation.eulerAngles.y;
            float h = playerStats.CurrentHealth;
            float m = playerStats.CurrentMana;
            float s = player.GetScore();
            
            if (a > 180f) a -= 360f; else if (a < -180f) a += 360f;
            if (b > 180f) b -= 360f;
            AddVectorObs(d);
            AddVectorObs(a);
            AddVectorObs(b);
            AddVectorObs(h);
            AddVectorObs(m);
            AddVectorObs(s);
            AddVectorObs(didFire);
            if(playerFields != null)
            {
                playerFields[0].text = d.ToString();
                playerFields[1].text = a.ToString();
                playerFields[2].text = b.ToString();
                playerFields[3].text = h.ToString();
                playerFields[4].text = m.ToString();
                playerFields[5].text = s.ToString();
                playerFields[6].text = didFire.ToString();
            }
            didFire = false;
        }
        // Enemies
        for (int i = 0; i < enemies.Length; ++i)
        {
            GameObject enemy = enemies[i];
            disp = enemy.transform.localPosition - transform.localPosition;
            float d = disp.magnitude;
            float a = enemy.transform.rotation.eulerAngles.y - Quaternion.LookRotation(disp).eulerAngles.y;
            float b = Quaternion.LookRotation(disp).eulerAngles.y - transform.rotation.eulerAngles.y;
            if (a > 180f) a -= 360f; else if (a < -180f) a += 360f;
            if (b > 180f) b -= 360f;
            float h = enemyStats[i].CurrentHealth;
            bool isStunned = enemyStats[i].IsStunned;
            bool isBoss = enemyAIs[i].isBoss;
            bool isPlayerTarget = enemyAIs[i].Target == player.gameObject;
            bool isSelfTarget = enemyAIs[i].Target == gameObject;
            AddVectorObs(d);
            AddVectorObs(a);
            AddVectorObs(b);
            AddVectorObs(h);
            AddVectorObs(isStunned);
            AddVectorObs(isBoss);
            AddVectorObs(isPlayerTarget);
            AddVectorObs(isSelfTarget);
            if(enemyFields != null && enemyFields[i] != null && enemyFields[i].Length > 0)
            {
                enemyFields[i][0].text = d.ToString();
                enemyFields[i][1].text = a.ToString();
                enemyFields[i][2].text = b.ToString();
                enemyFields[i][3].text = h.ToString();
                enemyFields[i][4].text = isStunned.ToString();
                enemyFields[i][5].text = isBoss.ToString();
                enemyFields[i][6].text = isPlayerTarget.ToString();
                enemyFields[i][7].text = isSelfTarget.ToString();
            }
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //AddReward(-1 / 10f);
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
        else if (spellIndex != -1)
            Fire(spellIndex);
    }

    public override void AgentReset()
    {
        ++numEpisodes;

        if (avgScoreField != null)
        {
            avgScoreField.text = (cumulativeScore / numEpisodes).ToString();
        }
        foreach (GameObject spell in castSpells)
        {
            if (spell != null)
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
        isDead = false;
        stats.AIReset();
        Shuffle();
    }

    public void AddCumulativeReward(float val)
    {
        cumulativeScore += val;
    }

    public override void AgentOnDone()
    {
        ++numEpisodes;
        //cumulativeScore += GetCumulativeReward();
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