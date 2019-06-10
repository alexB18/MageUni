using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Subscriber(params Object[] args);

public class Float : Object
{
    public float val = 0f;
    public Float(float f)
    {
        val = f;
    }
}
