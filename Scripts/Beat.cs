using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Beat {
    [SerializeField]
    public float beat_num;
    [SerializeField]
    public string type;

    public Beat(float bnum, string type) {
        this.beat_num = bnum;
        this.type = type;
    }
}