using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatScript : MonoBehaviour
{
    // LOCK
    private readonly object _lock = new object();

    private void Awake()
    {
        _DTArmourDict = new Dictionary<DamageType, float> {
            { DamageType.DTPuncture, punctureArmour },
            { DamageType.DTSlash, slashArmour },
            { DamageType.DTBludgeon, bludgeonArmour },
            { DamageType.DTFire, fireArmour },
            { DamageType.DTElectric, electricArmour },
            { DamageType.DTDivine, divineArmour }
        };
    }

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
    private bool canResurrect = true;

    private List<Subscriber> onDeathSubscribers = new List<Subscriber>();
    private List<Subscriber> onResurrectSubscribers = new List<Subscriber>();
    private List<Subscriber> onHealthChangeSubscribers = new List<Subscriber>();

    /*---- MANA VARIABLES ----*/
    // Only access these directly with the inspector!
    public float maximumMana = 100f;
    public float currentMana = 100f;

    private List<Subscriber> onManaChangeSubscribers = new List<Subscriber>();

    /*---- STUN VARIABLES ----*/

    // Lock before editing
    private float stunTime = 0f;
    public float stunResistance = 0f;

    // What to multiply the armour with when stunned
    public float stunArmourMultiplier = 0.1f;

    /*---- ARMOUR VARIABLES ----*/
    // Armour stat: damage = max(0, (damage - armour / 5.) * (100. - armour) / (200.)
    // Lock before editing
    public float punctureArmour = 0f;
    public float slashArmour = 0f;
    public float bludgeonArmour = 0f;
    public float fireArmour = 0f;
    public float electricArmour = 0f;
    public float divineArmour = 0f;

    public enum DamageType
    {
        DTPuncture,
        DTSlash,
        DTBludgeon,
        DTFire,
        DTElectric,
        DTDivine
    }

    private Dictionary<DamageType, float> _DTArmourDict;

    /*---- GENERAL METHODS ----*/
    public bool AIEnabled => !(IsStunned || IsDead);

    /*---- HEALTH METHODS ----*/

    public float MaximumHealth => maximumHealth;
    public float CurrentHealth => currentHealth;

    public void DamageHealth(float f, DamageType dt = DamageType.DTBludgeon)
    {
        if (!IsDead)
        {
            float armour = _DTArmourDict[dt];
            if (IsStunned) armour *= stunArmourMultiplier;
            float damage = Mathf.Max(0f, (f - armour / 5f) * (100f - armour) / 200f);
            currentHealth -= f;
            OnHealthChange();
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath();
            }
            Debug.Log(string.Format("Health at {0}", currentHealth));
        }
    }

    public void Resurrect()
    {
        if (IsDead && canResurrect)
        {
            canResurrect = false;
            currentHealth = maximumHealth / 2f;
            OnResurrect();
            OnHealthChange();
        }
    }

    public void RestoreHealth(float f)
    {
        if (!IsDead)
        {
            currentHealth += f;
            if (currentHealth > maximumHealth)
                currentHealth = maximumHealth;
            OnHealthChange();
        }
    }

    public void SubscribeToOnDeath(Subscriber sub) => onDeathSubscribers.Add(sub);

    public bool UnsubscribeFromOnDeath(Subscriber sub) => onDeathSubscribers.Remove(sub);

    public void SubscribeToOnResurrect(Subscriber sub) => onResurrectSubscribers.Add(sub);

    public bool UnsubscribeFromOnResurrect(Subscriber sub) => onResurrectSubscribers.Remove(sub);

    public void SubscribeToOnHealthChange(Subscriber sub) => onHealthChangeSubscribers.Add(sub);

    public bool UnsubscribeFromOnHealthChange(Subscriber sub) => onHealthChangeSubscribers.Remove(sub);

    public void OnDeath()
    {
        //Debug.Log("Dead");
        foreach (var sub in onDeathSubscribers)
            sub(gameObject);
    }

    public void OnResurrect()
    {
        foreach (var sub in onResurrectSubscribers)
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

    public bool IsDead => currentHealth <= 0;

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

    /*---- STUN METHODS ----*/
    public void AddStunProc(float time)
    {
        // request lock
        lock (_lock)
            stunTime += time;
        StartCoroutine("StunProcTimer", time);
    }
    
    private IEnumerator StunProcTimer(float time)
    {
        Debug.Log(gameObject.name + " stunned for " + time + " seconds");
        yield return new WaitForSeconds(time);
        lock (_lock)
            stunTime -= time;
        Debug.Log("Stun proc ended");
    }

    public bool IsStunned => stunTime > 0;
}
