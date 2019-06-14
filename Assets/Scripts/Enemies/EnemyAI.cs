using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    [HideInInspector] public bool isBoss = false;
    protected StatScript stats;
    protected Rigidbody rb;

    protected enum StateEnum
    {
        Idle,
        IdleContinue,
        Wander,
        WanderRotate,
        WanderContinue,
        DetectTarget,
        RotateTowardTarget,
        MoveTowardTarget,
        Attack,
        Airborne,
        Dead
    };
    protected StateEnum state;

    // Wander Variables
    public float maxWanderTime = 5f;
    private const float wanderSpeedMultiplier = 0.5f;
    private const float swivelAngleMax = 90f;
    private Coroutine wanderTimer;
    private Quaternion newRotation;
    private float newAngle;

    // Idle variables
    public float maxIdleTime = 2.5f;
    public float wanderChance = 0.40f;
    private Coroutine idleTimer;

    // The distance at which this AI will go after the target
    public const float targetDistanceThreshold = 6f;
    public const float targetBehindDistanceThreshold = 3f;
    private const float tdThresholdSq = targetDistanceThreshold * targetDistanceThreshold;
    private const float tdBehindThresholdSq = targetBehindDistanceThreshold * targetBehindDistanceThreshold;
    public float attackDistanceSquare = 3f;

    // How fast this AI boy rotates and translates
    private const float minRotationSpeed = 180;
    private const float maxRotationSpeed = 250;
    public float linearSpeed = 3f;

    // Angle after which we start to move
    private const float moveAngle = 10f;
    private const float moveAngleDeviation = 7f;
    
    // Attack variables
    public float damage = 10f;
    protected bool canAttack = true;
    public float attackCooldown = 1.25f;

    // Sound variables
    public AudioClip idleNoise1;
    public AudioClip idleNoise2;
    public AudioClip idleNoise3;
    public AudioClip detectNoise;
    public AudioClip attackNoise;
    public AudioClip deathNoise;

    protected AudioSource noiseSource;

    // Target variables
    protected GameObject target;
    public GameObject Target => target;
    protected StatScript targetStatScript;

    protected virtual void OnDeath(Object[] obj)
    {
        Vector3 deadRot = transform.rotation.eulerAngles;
        deadRot.z = 180f;
        transform.rotation = Quaternion.Euler(deadRot);
        state = StateEnum.Dead;
        if (deathNoise != null)
        {
            noiseSource.clip = deathNoise;
            noiseSource.Play();
        }
    }

    protected virtual void OnResurrect(Object[] obj)
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        state = StateEnum.Idle;
    }

    protected virtual void OnTargetDeath(Object[] obj)
    {
        StatScript enemyStatScript = obj[0] as StatScript;
        ResetTarget();
    }

    protected virtual void OnEnrage(Object[] obj)
    {
        ResetTarget();
    }


    public Vector2 xRange;
    public Vector2 yRange;
    private Quaternion startRot;
    public virtual void AIReset()
    {
        if (!stats)
            stats = GetComponent<StatScript>();
        stats?.AIReset();
        if (!rb)
            rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        Vector3 pos = new Vector3(
            Random.Range(xRange.x, xRange.y),
            0,
            Random.Range(yRange.x, yRange.y));
        transform.localPosition = pos;
        transform.localRotation = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0);

        state = StateEnum.Idle;
        ResetTarget();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        stats = GetComponent<StatScript>();
        stats.SubscribeToOnDeath(OnDeath);
        stats.SubscribeToOnResurrect(OnResurrect);
        stats.SubscribeToOnEnrage(OnEnrage);
        state = StateEnum.Idle;
        noiseSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.AIEnabled)
        {
            Vector3 ratPos = transform.position;
            Vector3 targetPos, dd;
            float squareDistanceFromTarget = 0f, angleBetweenTarget = 0f;
            if (target != null)
            {
                targetPos = target.transform.position;
                dd = targetPos - ratPos;
                squareDistanceFromTarget = Vector3.SqrMagnitude(dd);
                angleBetweenTarget = Vector3.Angle(transform.forward, target.transform.position - transform.position);
            }

            switch (state)
            {
                case StateEnum.Idle:
                    // See if we have a target. If so, make a noise and switch to DetectTarget
                    if (target != null)
                    {
                        state = StateEnum.DetectTarget;
                        if (detectNoise != null && playSound(0.5f))
                        {
                            noiseSource.clip = detectNoise;
                            noiseSource.Play();
                        }
                    }
                    // Try and switch to wander
                    if (Random.Range(0f, 1f) < wanderChance)
                    {
                        state = StateEnum.Wander;
                    }
                    else
                    {
                        idleTimer = StartCoroutine("IdleTimer");
                        state = StateEnum.IdleContinue;
                        // Do cute animations and squeaks, randomly switch to Wander
                        if (playSound(0.4f))
                        {
                            int idleNoise = Random.Range(0, 2);
                            switch (idleNoise)
                            {
                                case 0:
                                    if (idleNoise1 != null)
                                    {
                                        noiseSource.clip = idleNoise1;
                                        noiseSource.Play();
                                    }
                                    break;
                                case 1:
                                    if (idleNoise2 != null)
                                    {
                                        noiseSource.clip = idleNoise2;
                                        noiseSource.Play();
                                    }
                                    break;
                                case 2:
                                    if (idleNoise3 != null)
                                    {
                                        noiseSource.clip = idleNoise3;
                                        noiseSource.Play();
                                    }
                                    break;
                            }
                        }
                    }
                    break;

                case StateEnum.IdleContinue:
                    // See if have a target. If we do make a noise and, switch to DetectTarget
                    if (target != null)
                    {
                        StopCoroutine(idleTimer);
                        state = StateEnum.DetectTarget;
                        if (detectNoise != null && playSound(0.5f))
                        {
                            noiseSource.clip = detectNoise;
                            noiseSource.Play();
                        }
                    }
                    break;

                case StateEnum.Wander:
                    {
                        // See if we have a target. If so, make a noise and switch to DetectTarget
                        if (target != null)
                        {
                            state = StateEnum.DetectTarget;
                            if (detectNoise != null && playSound(0.5f))
                            {
                                noiseSource.clip = detectNoise;
                                noiseSource.Play();
                            }
                        }
                        newAngle = Random.Range(-swivelAngleMax, swivelAngleMax);
                        newRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + newAngle, 0f);

                        state = StateEnum.WanderRotate;
                    }
                    break;

                case StateEnum.WanderRotate:
                    {
                        // See if we have a target. If so, make a noise and switch to DetectTarget
                        if (target != null)
                        {
                            state = StateEnum.DetectTarget;
                            if (detectNoise != null && playSound(0.5f))
                            {
                                noiseSource.clip = detectNoise;
                                noiseSource.Play();
                            }
                        }

                        float t = Mathf.Abs(minRotationSpeed * Time.deltaTime / newAngle);

                        Quaternion slerpedLook = Quaternion.Slerp(transform.rotation, newRotation, t);
                        //Quaternion slerpedLook = Quaternion.LookRotation(lookV);
                        transform.rotation = Quaternion.Euler(new Vector3(0, slerpedLook.eulerAngles.y, 0));

                        if (Quaternion.Angle(transform.rotation, newRotation) <= 2)
                        {
                            state = StateEnum.WanderContinue;
                            wanderTimer = StartCoroutine("WanderTimer");
                        }
                    }
                    break;

                case StateEnum.WanderContinue:
                    {
                        // See if we have a target. If so, make a noise and switch to DetectTarget
                        if (target != null)
                        {
                            StopCoroutine(wanderTimer);
                            state = StateEnum.DetectTarget;
                            if (detectNoise != null && playSound(0.5f))
                            {
                                noiseSource.clip = detectNoise;
                                noiseSource.Play();
                            }
                        }

                        Vector3 moveVector = transform.forward;
                        moveVector *= linearSpeed * wanderSpeedMultiplier * stats.SpeedModifier;
                        moveVector.y = rb.velocity.y;
                        rb.velocity = moveVector;
                    }
                    break;

                case StateEnum.DetectTarget:
                    // Check if we can still detect the target
                    // Check if target is in acceptable sight and switch to MoveTowardTarget
                    // Else rotate toward target
                    if (target == null)
                        state = StateEnum.Idle;
                    else if (FacingTarget(angleBetweenTarget))
                        state = StateEnum.MoveTowardTarget;
                    else
                    {
                        state = StateEnum.RotateTowardTarget;
                    }
                    break;

                case StateEnum.RotateTowardTarget:
                    // Check if we can still detect the target
                    // Check if target is in acceptable sight and switch to MoveTowardTarget
                    // Else rotate toward target
                    if (target == null)
                        state = StateEnum.Idle;
                    else if (FacingTarget(angleBetweenTarget))
                        state = StateEnum.MoveTowardTarget;
                    else
                    {
                        newRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                        // Calculate rotation speed based on the velocity
                        float v2 = Vector3.SqrMagnitude(rb.velocity);
                        float rotationSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, v2 / linearSpeed);
                        float t = Mathf.Abs(rotationSpeed * Time.deltaTime / Quaternion.Angle(transform.rotation, newRotation));

                        Quaternion slerpedLook = Quaternion.Slerp(transform.rotation, newRotation, t).normalized;
                        //Quaternion slerpedLook = Quaternion.LookRotation(lookV);
                        transform.rotation = Quaternion.Euler(new Vector3(0, newRotation.eulerAngles.y, 0)).normalized;

                        if (Quaternion.Angle(transform.rotation, newRotation) <= 2)
                        {
                            state = StateEnum.MoveTowardTarget;
                        }
                    }
                    break;

                case StateEnum.MoveTowardTarget:
                    // Check if we can still detect the target
                    // Check if we are still facing the target
                    // Move toward the target. If within attacking range and can bite, switch to attack
                    if (target == null)
                        state = StateEnum.Idle;
                    else if (!FacingTarget(angleBetweenTarget))
                        state = StateEnum.DetectTarget;
                    else if (WithinAttackRange(squareDistanceFromTarget))
                        state = StateEnum.Attack;
                    else
                    {
                        // Calculate the most we can move
                        if (Vector3.Cross(transform.forward, target.transform.position - transform.position).y < 0)
                            angleBetweenTarget = -angleBetweenTarget;
                        float moveToAngle = transform.rotation.eulerAngles.y + angleBetweenTarget;
                        moveToAngle *= Mathf.Deg2Rad;
                        //Vector3 moveVector = new Vector3(Mathf.Cos(moveAngle), 0f, Mathf.Sin(moveAngle));

                        Vector3 moveVector = transform.forward;
                        moveVector *= linearSpeed * stats.SpeedModifier;
                        moveVector.y = rb.velocity.y;
                        rb.velocity = moveVector;
                    }
                    break;

                case StateEnum.Attack:
                    // Check if we can still detect the target
                    // Check if we are still facing the target
                    // Check if we are still within attack range
                    // Attack. Make a noise. Switch to Airborne
                    if (target == null)
                        state = StateEnum.Idle;
                    else if (!FacingTarget(angleBetweenTarget))
                        state = StateEnum.DetectTarget;
                    else if (!WithinAttackRange(squareDistanceFromTarget))
                        state = StateEnum.MoveTowardTarget;
                    else if (canAttack)
                    {
                        // Calculate the most we can move
                        if (Vector3.Cross(transform.forward, target.transform.position - transform.position).y < 0)
                            angleBetweenTarget = -angleBetweenTarget;
                        float moveToAngle = transform.rotation.eulerAngles.y + angleBetweenTarget;
                        moveToAngle *= Mathf.Deg2Rad;

                        // Make a noise
                        if (attackNoise != null && playSound(0.3f))
                        {
                            noiseSource.clip = attackNoise;
                            noiseSource.Play();
                        }

                        // Attack
                        StartCoroutine(AttackCooldown());
                        Attack(target);
                    }
                    break;

                case StateEnum.Airborne:
                    // Do nothing. Wait until we hit the ground. Collision detection will change the state
                    if (transform.localPosition.y <= 0.25f)
                        state = StateEnum.MoveTowardTarget;
                    break;

                case StateEnum.Dead:
                    // Do nothing, we're dead
                    break;
            }
        }
    }

    private bool playSound(float chance)
    {
        float roll = Random.Range(0f, 1f);
        return chance != 0 && roll <= chance;
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (stats && stats.AIEnabled)
        {
            if (state == StateEnum.WanderContinue)
            {
                // We've hit a wall. Go back to idle and stop the wander timer
                state = StateEnum.Idle;
                StopCoroutine(wanderTimer);
                wanderTimer = null;
            }

            if (state == StateEnum.Airborne && collision.gameObject.CompareTag("Ground"))
                state = StateEnum.MoveTowardTarget;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (target == null)
            PickTarget(other.gameObject);
    }

    public void OnTriggerStay(Collider other)
    {
        if (target == null)
            PickTarget(other.gameObject);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
            ResetTarget();
    }

    protected virtual bool DetectTarget(float squareDistance, float angle)
    {
        return false; //((squareDistance <= tdThresholdSq && angle < 60) || (squareDistance <= tdBehindThresholdSq));
    }

    protected virtual bool FacingTarget(float angle)
    {
        return angle < moveAngle;
    }

    protected virtual bool WithinAttackRange(float squareDistance)
    {
        return squareDistance < attackDistanceSquare;
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator WanderTimer()
    {
        yield return new WaitForSeconds(Random.Range(0, maxWanderTime));
        state = StateEnum.Idle;
        wanderTimer = null;
    }

    private IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(Random.Range(0, maxIdleTime));
        state = StateEnum.Idle;
        idleTimer = null;
    }

    protected abstract void Attack(GameObject t);

    private void PickTarget(GameObject go)
    {
        //go = go.transform.root.gameObject;
        bool pickTarget = false;

        if (stats && stats.IsEnraged)
            pickTarget = pickTarget || go.CompareTag("BadGuy");
        pickTarget = pickTarget || go.CompareTag("GoodGuy") || go.CompareTag("Player");
        pickTarget = pickTarget && !go.GetComponent<StatScript>().IsDead; // Will short circuit if there is no stats script because I'm cool

        if (pickTarget)
        {
            target = go;
            targetStatScript = target.GetComponent<StatScript>();
            targetStatScript.SubscribeToOnDeath(OnTargetDeath);
        }
    }

    private void ResetTarget()
    {
        targetStatScript?.UnsubscribeFromOnDeath(OnTargetDeath);
        target = null;
    }
}
