using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAI : EnemyAI
{
    // Pounce attack consts
    public const float pounceForce = 25f;
    private static Vector3 pounceUps = new Vector3(0, 30f, 0);

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (canAttack && collision.gameObject.CompareTag("Player"))
        {
            // Check if we hit with the head collider
            foreach (var c in collision.contacts)
            {
                if (c.thisCollider.name.Equals("_head"))
                {
                    Bite(collision.gameObject);
                    canAttack = false;
                    StartCoroutine("AttackCooldown");
                    break;
                }
            }
        }
    }

    public void Bite(GameObject enemy)
    {
        StatScript eHS = enemy.GetComponent<StatScript>();
        eHS.DamageHealth(damage);
    }

    protected override void Attack(GameObject t)
    {
        //Add force
        rb.AddForce(transform.forward * pounceForce);
        rb.AddForce(pounceUps);
    }
}
