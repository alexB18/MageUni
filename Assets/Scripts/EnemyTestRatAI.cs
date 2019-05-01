using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestRatAI : MonoBehaviour
{
    // The distance at which this rat will go after the player
    public float playerDistanceThreshold = 10f;
    // How fast rat boy rotates
    public float rotationSpeed = 0.4f;
    public float linearSpeed = 2f;
    public float maxLinearSpeed = 5f;
    public float pounceForce = 50f;
    private static Vector3 pounceUps = new Vector3(0, 25, 0);
    private float pdThresholdSq;
    private const float pounceDistanceSq = 0f;
    // Angle after which we start to move
    private const float moveAngle = 30f;
    private const float moveAngleDeviation = 15f;
    private const float MAX_ANGLE = 180f;
    private GameObject target;
    private bool grounded = true;

    private void Awake()
    {
        pdThresholdSq = playerDistanceThreshold * playerDistanceThreshold;
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Get player sq distance from rat boy
        if (target != null)
        {
            Vector3 ratPos = transform.position;
            Vector3 targetPos = target.transform.position;
            Vector3 dd = targetPos - ratPos;
            float sqd = Vector3.SqrMagnitude(dd); // Dot product but optimised since we pass one reference

            // If the player is within range of the rat and in the FOV, let's follow
            if (sqd <= pdThresholdSq)
            {
                // First, we should rotate towards the player
                //*
                Quaternion toLook = new Quaternion();
                toLook.SetFromToRotation(ratPos, targetPos);
                float t = rotationSpeed / MAX_ANGLE;
                Quaternion slerpedLook = Quaternion.Slerp(transform.rotation, toLook, t);
                transform.rotation = Quaternion.Euler(new Vector3(0, slerpedLook.eulerAngles.y, 0));
                //*/

                float _angle = slerpedLook.eulerAngles.y - toLook.eulerAngles.y;
                /*
                Vector3 rot = transform.rotation.eulerAngles;
                float da = _angle - rot.y;
                float daSign = Mathf.Sign(da);
                if (daSign * da > rotationSpeed)
                    rot.y += da * rotationSpeed;
                else
                    rot.y += da;

                transform.rotation = Quaternion.Euler(rot);
                //*/

                // If we are now within the angle threshold, lets start moving
                if (_angle < moveAngle)
                {
                    // We can only move forward with some certain angle deviation
                    float sign = Mathf.Sign(_angle);
                    float angle = _angle;
                    if (sign * angle > moveAngleDeviation)
                        angle = sign * moveAngleDeviation;

                    // Get the Vector3 angle
                    Vector3 moveAngle = (angle == 0) ? transform.rotation.eulerAngles : Vector3.Lerp(ratPos, targetPos, angle / _angle).normalized;

                    // Get rigidbody for movement
                    Rigidbody rb = gameObject.GetComponent<Rigidbody>();

                    // If within pouncing range, pounce. Otherwise move towards
                    if (grounded)
                        if (sqd < pounceDistanceSq)
                        {
                            grounded = false;
                            //rb.AddForce(moveAngle * pounceForce + pounceUps);
                        }
                        else
                        {
                            rb.AddForce(moveAngle * linearSpeed);
                            //rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxLinearSpeed);
                        }
                }
            }
        }


    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }
}
