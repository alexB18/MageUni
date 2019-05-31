using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBiteScript : MonoBehaviour
{
    public float damage = 10f;
    private bool canBite = true;
    private const float biteCooldown = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        EnemyTestRatAI ai = gameObject.GetComponentInParent<EnemyTestRatAI>() as EnemyTestRatAI;
        if (collision.gameObject == ai.target && canBite)
        {
            canBite = false;
            ai.Bite(collision.gameObject);
        }
    }

    private IEnumerator BiteCooldown()
    {
        yield return new WaitForSeconds(biteCooldown);
        canBite = true;
    }
}
