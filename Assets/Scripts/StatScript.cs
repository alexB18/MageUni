﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatScript : MonoBehaviour
{
    // LOCK
    private readonly object _lock = new object();

    // Material for tints
    Material material;
    Color clearTint = new Color(1.0f, 1.0f, 1.0f);

    private void Awake()
    {
        _DTResistanceDict = new Dictionary<DamageType, float> {
            { DamageType.DTPuncture, punctureResistance },
            { DamageType.DTSlash, slashResistance },
            { DamageType.DTBludgeon, bludgeonResistance },
            { DamageType.DTFire, fireResistance },
            { DamageType.DTElectric, electricResistance },
            { DamageType.DTDivine, divineResistance }
        };
    }

    private void Start()
    {
        material = GetComponentInChildren<Renderer>()?.material;
        // Activate the change listeners for health and mana
        OnHealthChange(0f);
        OnManaChange(0f);
    }

    private void OnDestroy()
    {
        Destroy(material);
    }
    /*---- HEALTH VARIABLES ----*/
    // Only access these directly with the inspector!
    public float maximumHealth = 100f;
    public float currentHealth = 100f;
    private bool canResurrect = true;
    private float dotTime = 0f;
    private const float dotTimeStep = 1.6f;

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
    // What to multiply the resistance with when stunned
    public float stunResistanceMultiplier = 0.1f;

    private Color stunTint = new Color(0.91f, 0.75f, 0.19f);

    /*---- FREEZE VARIABLES ----*/
    // Lock before editing
    private bool isFrozen = false;
    public float freezeResistance = 0f;
    // What to multiply the damage with when unfrozen
    public float freezeDamageMultiplier = 1f;
    private Color freezeTint = new Color(0.46f, 0.81f, 0.99f);

    private List<Subscriber> onFreezeSubscribers = new List<Subscriber>();
    private List<Subscriber> onFreezeEndSubscribers = new List<Subscriber>();

    /*---- ENRAGE VARIABLES ----*/
    public float enrageResistance = 0f;
    // Lock before changing
    private float enrageTime = 0f;
    private const float enrageTimeStep = 5f;
    private List<Subscriber> onEnrageSubscribers = new List<Subscriber>();

    /*---- SPEED VARIABLES ----*/
    //Lock before changing
    private float speedModifier = 1.0f;
    public float slowResistance = 0f;

    /*---- RESISTANCE VARIABLES ----*/
    // Resistance stat: damage = max(0, (damage - Resistance / 5.) * (100. - Resistance) / (200.)
    // Lock before editing
    public float punctureResistance = 0f;
    public float slashResistance = 0f;
    public float bludgeonResistance = 0f;
    public float fireResistance = 0f;
    public float electricResistance = 0f;
    public float divineResistance = 0f;

    public enum DamageType
    {
        DTPuncture,
        DTSlash,
        DTBludgeon,
        DTFire,
        DTElectric,
        DTDivine
    }

    private Dictionary<DamageType, float> _DTResistanceDict;

    /*---- GENERAL METHODS ----*/
    public bool AIEnabled => !(IsStunned || IsDead || IsFrozen);
    public void AIReset()
    {
        StopAllCoroutines();
        currentHealth = maximumHealth;
        currentMana = maximumMana;
        stunTime = 0f;
        speedModifier = 1f;
        enrageTime = 0f;
        isFrozen = false;
    }

    /*---- HEALTH METHODS ----*/

    public float MaximumHealth => maximumHealth;
    public float CurrentHealth => currentHealth;

    public void DamageHealth(float f, DamageType dt = DamageType.DTBludgeon)
    {
        DamageHealth(f, null, dt);
    }

    public void DamageHealth(float f, GameObject source, DamageType dt = DamageType.DTBludgeon)
    {
        if (!IsDead)
        {
            float old = currentHealth;
            float resistance = _DTResistanceDict[dt];
            if (IsStunned) resistance *= stunResistanceMultiplier;

            float damage = Mathf.Max(0f, (f - resistance / 5f) * (100f - resistance / 2f) / 100f);
            if (IsFrozen)
            {
                damage *= freezeDamageMultiplier;
                RemoveFreezeProc();
            }

            lock (_lock)
            {
                currentHealth -= damage;
                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    OnDeath(source);
                }
            }
            OnHealthChange(currentHealth - old, source);
            Debug.Log(string.Format("Health at {0}", currentHealth));
        }
    }

    public void AddDamageOverTimeProc(float time, float damage, DamageType dt = DamageType.DTBludgeon)
    {
        lock(_lock)
        {
            dotTime += time;
        }
        StartCoroutine(DoTLoop(time, damage, dt));
    }

    public void Resurrect()
    {
        if (IsDead && canResurrect)
        {
            canResurrect = false;
            currentHealth = maximumHealth / 2f;
            OnResurrect();
            OnHealthChange(maximumHealth / 2f);
        }
    }

    public void RestoreHealth(float f)
    {
        if (!IsDead)
        {
            float old = currentHealth;
            currentHealth += f;
            if (currentHealth > maximumHealth)
                currentHealth = maximumHealth;
            OnHealthChange(currentHealth - old);
        }
    }

    public void RestoreMana(float f)
    {
        if (!IsDead)
        {
            float old = currentMana;
            currentMana += f;
            if (currentMana > maximumMana)
                currentMana = maximumMana;
            OnManaChange(currentMana - old);
        }
    }

    public void SubscribeToOnDeath(Subscriber sub)
    {
        if (!onDeathSubscribers.Contains(sub)) onDeathSubscribers.Add(sub);
    }

    public bool UnsubscribeFromOnDeath(Subscriber sub) => onDeathSubscribers.Remove(sub);

    public void SubscribeToOnResurrect(Subscriber sub) => onResurrectSubscribers.Add(sub);

    public bool UnsubscribeFromOnResurrect(Subscriber sub) => onResurrectSubscribers.Remove(sub);

    public void SubscribeToOnHealthChange(Subscriber sub) => onHealthChangeSubscribers.Add(sub);

    public bool UnsubscribeFromOnHealthChange(Subscriber sub) => onHealthChangeSubscribers.Remove(sub);

    public void OnDeath()
    {
        //Debug.Log("Dead");
        List<Subscriber> subscribers = new List<Subscriber>(onDeathSubscribers);
        foreach (var sub in subscribers)
            sub(gameObject, null);
    }

    public void OnDeath(GameObject source)
    {
        //Debug.Log("Dead");
        List<Subscriber> subscribers = new List<Subscriber>(onDeathSubscribers);
        foreach (var sub in subscribers)
            sub(gameObject, source);
    }

    public void OnResurrect()
    {
        foreach (var sub in onResurrectSubscribers)
            sub(gameObject);
    }

    private IEnumerator DoTLoop(float time, float damage, DamageType dt)
    {
        float timeAcc = time;
        do
        {
            DamageHealth(damage, dt);
            yield return new WaitForSeconds(dotTimeStep);
            timeAcc -= dotTimeStep;
        } while (timeAcc > 0);
        lock (_lock)
            dotTime -= time;
    }

    /**
     * Notifies with this script as the object. We can't pass floats as objects
     */
    public void OnHealthChange(float f)
    {
        foreach (var sub in onHealthChangeSubscribers)
            sub(this, new Float(f), null);
    }
    /**
     * Notifies with this script as the object. We can't pass floats as objects
     */
    public void OnHealthChange(float f,GameObject source)
    {
        foreach (var sub in onHealthChangeSubscribers)
            sub(this, new Float(f), source);
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
        float old = currentMana;
        float mana = currentMana + f;

        if(mana < 0)
            return false;

        if(mana > maximumMana)
            mana = maximumMana;

        currentMana = mana;
        OnManaChange(currentMana - old);

        return true;
    }

    public void OnManaChange(float f)
    {
        foreach (Subscriber sub in onManaChangeSubscribers)
            sub.Invoke(this, new Float(f));
    }

    /*---- STUN METHODS ----*/
    public void AddStunProc(float time)
    {
        // Check if we resist
        float procRoll = Random.value;
        if (procRoll > stunResistance)
        {
            // request lock
            lock (_lock)
            {
                stunTime += time;
                material.SetColor("_Color", stunTint);
            }
            StartCoroutine("StunProcTimer", time);
        }
        else
            Debug.Log(gameObject.name + " resisted stun");
    }
    
    private IEnumerator StunProcTimer(float time)
    {
        Debug.Log(gameObject.name + " stunned for " + time + " seconds");
        yield return new WaitForSeconds(time);
        lock (_lock)
        {
            stunTime -= time;
            if(stunTime <= 0f)
                material.SetColor("_Color", clearTint);
        }
        Debug.Log("Stun proc ended");
    }

    public bool IsStunned => stunTime > 0;

    /*---- FREEZE METHODS ----*/
    public void AddFreezeProc()
    {
        // Check if we resist
        float procRoll = Random.value;
        if (!isFrozen && procRoll > freezeResistance)
        {
            lock (_lock)
            {
                Debug.Log(gameObject.name + " frozen");
                isFrozen = true;
                OnFreeze();
                material.SetColor("_Color", freezeTint);
            }
        }
        else
            Debug.Log(gameObject.name + " resisted freeze");
    }

    private void RemoveFreezeProc()
    {
        lock (_lock)
        {
            isFrozen = false;
            OnFreezeEnd();
            material.SetColor("_Color", clearTint);
        }
        Debug.Log(gameObject.name + " unfrozen");
    }

    public bool IsFrozen => isFrozen;

    public void SubscribeToOnFreeze(Subscriber sub)
    {
        onFreezeSubscribers.Add(sub);
    }

    public void UnsubscribeToOnFreeze(Subscriber sub)
    {
        onFreezeSubscribers.Remove(sub);
    }

    public void SubscribeToOnFreezeEnd(Subscriber sub)
    {
        onFreezeEndSubscribers.Add(sub);
    }

    public void UnsubscribeToOnFreezeEnd(Subscriber sub)
    {
        onFreezeEndSubscribers.Remove(sub);
    }

    public void OnFreeze()
    {
        foreach (Subscriber sub in onFreezeSubscribers)
            sub(this);
    }

    public void OnFreezeEnd()
    {
        foreach (Subscriber sub in onFreezeEndSubscribers)
            sub(this);
    }

    /*---- ENRAGE METHODS ----*/
    public void AddEnrageProc(float time)
    {
        // Roll resist
        float procRoll = Random.value;
        if (procRoll > enrageResistance)
        {
            // I would like to rage
            lock(_lock) enrageTime += time;
            StartCoroutine(EnrageProcTimer(time));
        }
    }

    public void SubscribeToOnEnrage(Subscriber sub)
    {
        onEnrageSubscribers.Add(sub);
    }

    public void UnsubscribeToOnEnrage(Subscriber sub)
    {
        onEnrageSubscribers.Remove(sub);
    }

    private void OnEnrage()
    {
        foreach (var sub in onEnrageSubscribers)
            sub(this);
    }

    private IEnumerator EnrageProcTimer(float time)
    {
        Debug.Log(gameObject.name + " enraged for " + time + " seconds");
        float timeAcc = time;
        do
        {
            OnEnrage();
            yield return new WaitForSeconds(enrageTimeStep);
            timeAcc -= enrageTimeStep;
        } while (timeAcc > 0);
        lock (_lock) enrageTime -= time;
        // Reset the target selection again
        OnEnrage();
        Debug.Log(gameObject.name + " enrage proc ended");
    }

    public bool IsEnraged => enrageTime > 0;

    /*---- SPEED METHODS ----*/
    public void AddSpeedProc(float time, float magnitude)
    {
        // Resist negative (slow), don't resist positive.
        bool success = true;
        if(magnitude < 0)
        {
            float procRoll = Random.value;
            success = procRoll > slowResistance;
        }
        if(success)
        {
            lock (_lock) speedModifier += magnitude;
            StartCoroutine(SpeedProcTimer(time, magnitude));
        }
    }

    private IEnumerator SpeedProcTimer(float time, float magnitude)
    {
        Debug.Log(gameObject.name + " speed proc with magnitude " + magnitude);
        yield return new WaitForSeconds(time);
        lock (_lock) speedModifier -= magnitude;
        Debug.Log(gameObject.name + " speed proc finished");
    }

    public float SpeedModifier => Mathf.Max(0f, speedModifier);
}
