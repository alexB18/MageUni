using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float maximumHealth = 100f;
    public float currentHealth = 100f;
    public List<Subscriber> onDeathSubscribers = new List<Subscriber>();
    public List<Subscriber> onHealthChangeSubscribers = new List<Subscriber>();

    public void Awake()
    {
        //currentHealth = maximumHealth;
    }

    public void DamageHealth(float f)
    {
        currentHealth -= f;
        OnHealthChange();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath();
        }
        Debug.Log(string.Format("Health at {0}", currentHealth));
    }

    public void RestoreHealth(float f)
    {
        currentHealth += f;
        if (currentHealth > maximumHealth)
            currentHealth = maximumHealth;
        OnHealthChange();
    }

    public void SubscribeToOnDeath(Subscriber sub)
    {
        onDeathSubscribers.Add(sub);
    }

    public bool UnsubscribeToOnDeath(Subscriber sub)
    {
        return onDeathSubscribers.Remove(sub);
    }

    public void SubscribeToOnHealthChange(Subscriber sub)
    {
        onHealthChangeSubscribers.Add(sub);
    }

    public bool UnsubscribeToOnHealthChange(Subscriber sub)
    {
        return onHealthChangeSubscribers.Remove(sub);
    }

    public void OnDeath()
    {
        //Debug.Log("Dead");
        foreach (var sub in onDeathSubscribers)
            sub(gameObject);
    }

    /**
     * Notifies with this script as the object. We can't pass floats as objects
     */
    public void OnHealthChange()
    {
        foreach (var sub in onHealthChangeSubscribers)
            sub(this);
    }
}
