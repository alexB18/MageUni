using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBiteScript : MonoBehaviour
{
    private bool canBite = true;
    private const float biteCooldown = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        EnemyAI ai = gameObject.GetComponentInParent<EnemyAI>() as EnemyAI;
        if (collision.gameObject == ai.Target && canBite)
        {
            canBite = false;
            ai.Target.GetComponent<StatScript>().DamageHealth(ai.damage);
            StartCoroutine(BiteCooldown());
        }
    }

    private IEnumerator BiteCooldown()
    {
        yield return new WaitForSeconds(biteCooldown);
        canBite = true;
    }
}
