using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AIPlayer : Agent
{
    private readonly object _lock = new object();
    private StatScript stats;

    public GameObject EnemiesContainer;
    public GameObject InteractablesContainer;
    public GameObject DoorsContainer;

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

    private void OnDeath(Object[] obj)
    {
        score += deathScore;
        isDead = true;
        Finish();
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
        AgentReset();
        startFrame = Time.frameCount;
        enemyCount = EnemiesContainer.transform.childCount;
        foreach (Transform enemyTransform in EnemiesContainer.transform)
        {
            StatScript enemyStats = enemyTransform.GetComponent<StatScript>();
            enemyStats.SubscribeToOnDeath(OnEnemyDeath);
            enemyStats.SubscribeToOnHealthChange(OnEnemyHealthChange);
            enemyStats.SubscribeToOnFreeze(OnEnemyFreeze);
            enemyStats.SubscribeToOnFreezeEnd(OnEnemyFreezeEnd
);
        }
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

    private void SimpleMove(float x, float z)
    {
        Vector3 moveVector = new Vector3(x, 0f, z);

        moveVector *= moveSpeed;
        moveVector.y = rb.velocity.y;
        rb.velocity = moveVector;
    }

    public override void CollectObservations()
    {
        RayPerception3D ray = new RayPerception3D();
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        base.AgentAction(vectorAction, textAction);
        // Move
        // Rotate
        // Health potion
        // Mana potion
        // Interact
        // Fire spell
    }

    public override void AgentReset()
    {
        stats.Reset();
        score = 0;
        startFrame = Time.frameCount;
        isDead = false;
        frozenCount = 0;
        isPacifist = true;

        // Reset enemies
        foreach (Transform t in EnemiesContainer.transform)
            t.GetComponent<EnemyAI>().Reset();
    }

    public override void AgentOnDone()
    {
        Score();
        base.AgentOnDone();
    }
}
