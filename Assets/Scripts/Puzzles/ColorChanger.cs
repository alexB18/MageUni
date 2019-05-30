using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void changeColor(Color color)
    {
        foreach (GameObject dynamicColorObject in DynamicColorObject.list)
        {
            dynamicColorObject.GetComponent<Renderer>().sharedMaterial.color = color;
        }
    }
}
