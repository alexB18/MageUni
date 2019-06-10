using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueUntilHit : MonoBehaviour
{
    public float damage = 5f;
    public Rigidbody rb;
    public Renderer rend;
    public Collider coll;
    public float timer = 10f;
    private float audioClipTime = 1f;
    public AudioClip hitSound;
    public AudioClip startSound;
    public AudioSource noiseSource;

    private bool playSound(float chance)
    {
        float roll = Random.Range(0f, 1f);
        return chance != 0 && roll <= chance;
    }

    private void Start()
    {
        rb.velocity = transform.forward * 5f;
        StartCoroutine(DestroyAfterTime());
        noiseSource.clip = startSound;
        noiseSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<StatScript>()?.DamageHealth(damage);
        StopAllCoroutines();

        noiseSource.clip = hitSound;
        noiseSource.Play();
        rend.enabled = false;
        coll.enabled = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        StartCoroutine(HitDestroy());
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

    IEnumerator HitDestroy()
    {
        yield return new WaitForSeconds(audioClipTime);
        Destroy(gameObject);
    }
}
