using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTransformOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition.Set(0.5f, 0.5f);
    }
}
