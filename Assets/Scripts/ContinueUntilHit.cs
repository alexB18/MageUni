using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueUntilHit : MonoBehaviour
{
    public float damage = 5f;
    public Rigidbody rb;
    public float timer = 10f;

    private void Start()
    {
        rb.velocity = transform.forward * 5f;
        StartCoroutine(DestroyAfterTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<StatScript>()?.DamageHealth(damage);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
