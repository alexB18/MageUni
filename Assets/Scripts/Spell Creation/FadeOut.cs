using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    private const float stayLength = 2f;
    private const float decayStepLength = 1f / 15f;
    private const float decayLength = 1.5f;
    public TMPro.TextMeshProUGUI text;
    private void OnEnable()
    {
        // Start a coroutine to do our coolness
        StartCoroutine("Fade");
    }

    IEnumerator Fade()
    {
        Color color = text.color;
        color.a = 1f;
        text.color = color;
        yield return new WaitForSecondsRealtime(stayLength);
        float decayStep = decayStepLength / decayLength;
        while(color.a > 0f)
        {
            color.a -= decayStep;
            text.color = color;
            yield return new WaitForSecondsRealtime(decayStepLength);
        }
        text.gameObject.SetActive(false);
    }
}
