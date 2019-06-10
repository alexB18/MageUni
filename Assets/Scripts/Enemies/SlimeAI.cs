using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : EnemyAI
{
    public GameObject slimeAttack;
    protected override void Attack(GameObject t)
    {
        // Aim
        Vector3 rPos = t.transform.position - transform.position;
        Quaternion aim = Quaternion.LookRotation(rPos);
        float angle = aim.eulerAngles.y;
        // Offset
        Vector3 scale = transform.lossyScale;
        float offset = Mathf.Max(scale.x, scale.z) + .5f;
        // Create
        GameObject attack = Instantiate<GameObject>(slimeAttack, transform.position, aim);
        attack.transform.position += attack.transform.forward * offset;
        attack.GetComponent<ContinueUntilHit>().damage = damage;
        
    }
}
