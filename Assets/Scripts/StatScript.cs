using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatScript : MonoBehaviour
{
    private void Start()
    {
        // Activate the change listeners for health and mana
        OnHealthChange();
        OnManaChange();
    }
    /*---- HEALTH VARIABLES ----*/
    // Only access these directly with the inspector!
    public float maximumHealth = 100f;
    public float currentHealth = 100f;

    private List<Subscriber> onDeathSubscribers = new List<Subscriber>();
    private List<Subscriber> onHealthChangeSubscribers = new List<Subscriber>();

    /*---- MANA VARIABLES ----*/
    // Only access these directly with the inspector!
    public float maximumMana = 100f;
    public float currentMana = 100f;

    private List<Subscriber> onManaChangeSubscribers = new List<Subscriber>();

    /*---- HEALTH METHODS ----*/

    public float MaximumHealth => maximumHealth;
    public float CurrentHealth => currentHealth;

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

    public void SubscribeToOnDeath(Subscriber sub) => onDeathSubscribers.Add(sub);

    public bool UnsubscribeFromOnDeath(Subscriber sub) => onDeathSubscribers.Remove(sub);

    public void SubscribeToOnHealthChange(Subscriber sub) => onHealthChangeSubscribers.Add(sub);

    public bool UnsubscribeFromOnHealthChange(Subscriber sub) => onHealthChangeSubscribers.Remove(sub);

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


    /*---- MANA METHODS ----*/

    public float MaximumMana => maximumMana;
    public float CurrentMana => currentMana;

    public void SubscribeToOnManaChange(Subscriber sub) => onManaChangeSubscribers.Add(sub);
    public bool UnsubscribeFromOnManaChange(Subscriber sub) => onManaChangeSubscribers.Remove(sub);

    // Returns whether or not there was enough mana left
    public bool ModifyMana(float f)
    {
        float mana = currentMana + f;

        if(mana < 0)
            return false;

        if(mana > maximumMana)
            mana = maximumMana;

        currentMana = mana;
        OnManaChange();

        return true;
    }

    public void OnManaChange()
    {
        foreach (Subscriber sub in onManaChangeSubscribers)
            sub.Invoke(this);
    }
}
