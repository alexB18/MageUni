using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBiteScript : MonoBehaviour
{
    private bool canBite = true;
    private const float biteCooldown = 2f;
    public EnemyAI myAI;

    private void Start()
    {
        if(!myAI)
        {
            myAI = gameObject.GetComponentInParent<EnemyAI>() as EnemyAI;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject == myAI.Target || collision.gameObject.CompareTag("GoodGuy")) && canBite)
        {
            canBite = false;
            myAI.Target.GetComponent<StatScript>().DamageHealth(myAI.damage);
            StartCoroutine(BiteCooldown());
        }
    }

    private IEnumerator BiteCooldown()
    {
        yield return new WaitForSeconds(biteCooldown);
        canBite = true;
    }
}
