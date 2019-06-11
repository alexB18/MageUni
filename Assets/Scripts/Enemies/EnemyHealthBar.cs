using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private readonly object _lock = new object();
    private Coroutine coroutine = null;
    public Image healthbarBg;
    public Image healthbar;
    public StatScript stats;
    private float stayTime = 2.0f;
    private float fadeTime = 0.75f;
    private float timeStep = 0.10f;

    private float bgStartAlpha;
    private float hbStartAlpha;

    void Start()
    {
        stats.SubscribeToOnHealthChange(OnHealthChange);
        bgStartAlpha = healthbarBg.color.a;
        hbStartAlpha = healthbar.color.a;
        
        Color bg = healthbarBg.color;
        Color hb = healthbar.color;
        bg.a = 0f;
        hb.a = 0f;
        healthbarBg.color = bg;
        healthbar.color = hb;
    }

    private void Update()
    {
        if (Camera.main)
        {
            Vector3 look = Camera.main.transform.forward;
            look.y = 0f;
            look = -look;
            transform.parent.rotation = Quaternion.LookRotation(look, transform.up);
        }
    }

    private void OnHealthChange(Object[] obj)
    {
        if (!stats.IsDead)
        {
            healthbar.fillAmount = stats.CurrentHealth / stats.maximumHealth;
            lock (_lock) if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        Color bg = healthbarBg.color;
        Color hb = healthbar.color;
        bg.a = bgStartAlpha;
        hb.a = hbStartAlpha;
        healthbarBg.color = bg;
        healthbar.color = hb;

        yield return new WaitForSeconds(stayTime);

        float steps = Mathf.Ceil(fadeTime / timeStep);
        float bgColourStep = bgStartAlpha / steps;
        float hbColourStep = hbStartAlpha / steps;
        int l = (int)steps;
        
        for(int i = 0; i < l; ++i)
        {
            bg.a -= bgColourStep;
            hb.a -= hbColourStep;
            healthbarBg.color = bg;
            healthbar.color = hb;
            yield return new WaitForSeconds(timeStep);
        }
        lock (_lock) coroutine = null;
    }
}
