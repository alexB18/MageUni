using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    protected StatScript stats;

    private enum StateEnum
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
    private StateEnum state;

    // Wander Variables
    private const float maxWanderTime = 5f;
    private const float wanderSpeedMultiplier = 0.4f;
    private const float swivelAngleMax = 90f;
    private Coroutine wanderTimer;
    private Quaternion newRotation;
    private float newAngle;

    // Idle variables
    private const float maxIdleTime = 2.5f;
    private const float wanderChance = 0.40f;
    private Coroutine idleTimer;

    // The distance at which this AI will go after the target
    public const float targetDistanceThreshold = 6f;
    public const float targetBehindDistanceThreshold = 3f;
    private const float tdThresholdSq = targetDistanceThreshold * targetDistanceThreshold;
    private const float tdBehindThresholdSq = targetBehindDistanceThreshold * targetBehindDistanceThreshold;
    private const float attackDistanceSquare = 3f;

    // How fast this AI boy rotates and translates
    private const float minRotationSpeed = 180;
    private const float maxRotationSpeed = 250;
    public const float linearSpeed = 3.5f;

    // Angle after which we start to move
    private const float moveAngle = 10f;
    private const float moveAngleDeviation = 7f;
    
    protected float damage = 10f;
    protected bool canAttack = true;
    protected float attackCooldown = 1.25f;

    // Sound variables

    protected GameObject target;
    protected Rigidbody rb;

    protected void OnDeath(Object[] obj)
    {
        Vector3 deadRot = transform.rotation.eulerAngles;
        deadRot.z = 180f;
        transform.rotation = Quaternion.Euler(deadRot);
        state = StateEnum.Dead;
    }

    protected void OnResurrect(Object[] obj)
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        state = StateEnum.Idle;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        stats = GetComponent<StatScript>();
        stats.SubscribeToOnDeath(OnDeath);
        stats.SubscribeToOnResurrect(OnResurrect);
        target = GameObject.FindGameObjectWithTag("Player");
        state = StateEnum.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.AIEnabled && target != null)
        {
            Vector3 ratPos = transform.position;
            Vector3 targetPos = target.transform.position;
            Vector3 dd = targetPos - ratPos;
            float squareDistanceFromTarget = Vector3.SqrMagnitude(dd);
            float angleBetweenTarget = Vector3.Angle(transform.forward, target.transform.position - transform.position);

            switch (state)
            {
                case StateEnum.Idle:
                    // See if we detect the player. If so, make a noise and switch to DetectTarget
                    if (DetectTarget(squareDistanceFromTarget, angleBetweenTarget))
                        state = StateEnum.DetectTarget;
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
                    }
                    break;

                case StateEnum.IdleContinue:
                    // See if we detect the player. If so, make a noise and switch to DetectTarget
                    if (DetectTarget(squareDistanceFromTarget, angleBetweenTarget))
                    {
                        StopCoroutine(idleTimer);
                        state = StateEnum.DetectTarget;
                    }
                    break;

                case StateEnum.Wander:
                    {
                        // See if we detect the player. If so, make a noise and switch to DetectTarget
                        if (DetectTarget(squareDistanceFromTarget, angleBetweenTarget))
                            state = StateEnum.DetectTarget;

                        newAngle = Random.Range(-swivelAngleMax, swivelAngleMax);
                        newRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + newAngle, 0f);

                        state = StateEnum.WanderRotate;
                    }
                    break;

                case StateEnum.WanderRotate:
                    {
                        // See if we detect the player. If so, make a noise and switch to DetectTarget
                        if (DetectTarget(squareDistanceFromTarget, angleBetweenTarget))
                            state = StateEnum.DetectTarget;

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
                        // See if we detect the player. If so, make a noise and switch to DetectTarget
                        if (DetectTarget(squareDistanceFromTarget, angleBetweenTarget))
                        {
                            StopCoroutine(wanderTimer);
                            state = StateEnum.DetectTarget;
                        }

                        Vector3 moveVector = transform.forward;
                        moveVector *= linearSpeed * wanderSpeedMultiplier;
                        moveVector.y = rb.velocity.y;
                        rb.velocity = moveVector;
                    }
                    break;

                case StateEnum.DetectTarget:
                    // Check if we can still detect the target
                    // Check if target is in acceptable sight and switch to MoveTowardTarget
                    // Else rotate toward target
                    if (!DetectTarget(squareDistanceFromTarget, angleBetweenTarget))
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
                    if (!DetectTarget(squareDistanceFromTarget, angleBetweenTarget))
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

                        Quaternion slerpedLook = Quaternion.Slerp(transform.rotation, newRotation, t);
                        //Quaternion slerpedLook = Quaternion.LookRotation(lookV);
                        transform.rotation = Quaternion.Euler(new Vector3(0, slerpedLook.eulerAngles.y, 0));

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
                    if (!DetectTarget(squareDistanceFromTarget, angleBetweenTarget))
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
                        moveVector *= linearSpeed;
                        moveVector.y = rb.velocity.y;
                        rb.velocity = moveVector;
                    }
                    break;

                case StateEnum.Attack:
                    // Check if we can still detect the target
                    // Check if we are still facing the target
                    // Check if we are still within attack range
                    // Attack. Make a noise. Switch to Airborne
                    if (!DetectTarget(squareDistanceFromTarget, angleBetweenTarget))
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


                        // Attack
                        Attack(target);

                        // Make a noise

                        // Switch state
                        state = StateEnum.Airborne;
                    }
                    break;

                case StateEnum.Airborne:
                    // Do nothing. Wait until we hit the ground. Collision detection will change the state
                    break;

                case StateEnum.Dead:
                    // Do nothing, we're dead
                    break;
            }
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (stats.AIEnabled)
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

    protected virtual bool DetectTarget(float squareDistance, float angle)
    {
        return ((squareDistance <= tdThresholdSq && angle < 60) || (squareDistance <= tdBehindThresholdSq));
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
}
