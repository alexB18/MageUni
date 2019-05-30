using System.Collections.Generic;
using UnityEngine;

public class DynamicColorObject : MonoBehaviour
{
    public static List<GameObject> list = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        list.Add(gameObject);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        list.Remove(gameObject);
    }

}
