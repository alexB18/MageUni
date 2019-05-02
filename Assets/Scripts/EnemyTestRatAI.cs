using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestRatAI : MonoBehaviour
{
    // The distance at which this rat will go after the player
    public const float playerDistanceThreshold = 10f;
    public const float playerBehindDistanceThreshold = 2f;
    private const float pdThresholdSq = playerDistanceThreshold * playerDistanceThreshold;
    private const float pdBehindThresholdSq = playerBehindDistanceThreshold * playerBehindDistanceThreshold;

    // How fast rat boy rotates and translates
    private const float minRotationSpeed = 2f;
    private const float maxRotationSpeed = 15f;
    public const float linearSpeed = 2f;
    public const float maxLinearSpeed = 3f;
    public const float maxLinearSpeedSq = maxLinearSpeed * maxLinearSpeed;

    // Angle after which we start to move
    private const float moveAngle = 20f;
    private const float moveAngleDeviation = 10;

    // Pounce attack consts
    public const float pounceForce = 50f;
    private static Vector3 pounceUps = new Vector3(0, 40f, 0);
    private const float pounceDistanceSq = 7f;
    private bool grounded = true; // If this is true, rat boy can pounce
    private bool hitGround = false;

    private GameObject target;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get player sq distance from rat boy
        if (target != null)
        {

            if (grounded)
            {
                Vector3 ratPos = transform.position;
                Vector3 targetPos = target.transform.position;
                Vector3 dd = targetPos - ratPos;
                float sqd = Vector3.SqrMagnitude(dd); // Dot product but optimised since we pass one reference

                // If the player is within range of the rat and in the FOV, let's follow
                float angle = Vector3.Angle(transform.forward, target.transform.position - transform.position);
                if ((sqd <= pdThresholdSq && angle < 60) || (sqd <= pdBehindThresholdSq))
                {
                    // Calculate rotation speed based on the velocity
                    float v2 = Vector3.SqrMagnitude(rb.velocity);
                    float rotationSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, v2 / maxLinearSpeed);
                    float t = Mathf.Abs(rotationSpeed / angle);

                    Quaternion slerpedLook = Quaternion.Slerp(transform.rotation,
                                                                Quaternion.LookRotation(target.transform.position - transform.position),
                                                                t);
                    //Quaternion slerpedLook = Quaternion.LookRotation(lookV);
                    transform.rotation = Quaternion.Euler(new Vector3(0, slerpedLook.eulerAngles.y, 0));
                    //*/

                    angle = Vector3.Angle(transform.forward, target.transform.position - transform.position);
                    // If we are now within the angle threshold, lets start moving
                    if (angle < moveAngle)
                    {
                        // We can only move forward with some certain angle deviation
                        if (angle > moveAngleDeviation)
                            angle = moveAngleDeviation;

                        // Get the Vector3 movement vector direction
                        if (Vector3.Cross(transform.forward, target.transform.position - transform.position).y < 0)
                            angle = -angle;
                        float moveToAngle = transform.rotation.eulerAngles.y + angle;
                        moveToAngle *= Mathf.Deg2Rad;
                        //Vector3 moveVector = new Vector3(Mathf.Cos(moveAngle), 0f, Mathf.Sin(moveAngle));
                        Vector3 moveVector = transform.forward;


                        // If within pouncing range, pounce. Otherwise move towards
                        if (sqd < pounceDistanceSq)
                        {
                            grounded = false;
                            hitGround = false;
                            rb.AddForce(moveVector * pounceForce);
                            rb.AddForce(pounceUps);
                        }
                        else
                        {
                            rb.AddForce(moveVector * linearSpeed * 5);
                            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxLinearSpeed);
                        }
                    }
                }
            }
        }


    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!grounded && !hitGround && collision.gameObject.CompareTag("Ground"))
        {
            hitGround = true;
            StartCoroutine("JumpCooldown");
        }
    }

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        grounded = true;
    }
}
