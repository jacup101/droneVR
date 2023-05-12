using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ColorTemp {
    [SerializeField]
    public float red;
    [SerializeField]
    public float blue;
    [SerializeField]
    public float green;

    public ColorTemp(float red, float blue, float green) {
        // Add error handling
        this.red = red;
        this.blue = blue;
        this.green = green;
    }
}