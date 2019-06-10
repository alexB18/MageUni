using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentWardrobe : MonoBehaviour
{
    public Material[] robeMaterials;
    public Renderer hatRenderer;
    public Renderer bodyRenderer;
    
    void Start()
    {
        if(robeMaterials.Length > 0)
        {
            int index = Random.Range(0, robeMaterials.Length - 1);
            hatRenderer.material = robeMaterials[index];
            bodyRenderer.material = robeMaterials[index];
        }
    }
}
