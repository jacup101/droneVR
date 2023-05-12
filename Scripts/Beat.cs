using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Beat : IComparable {
    [SerializeField]
    public float beat_num;
    [SerializeField]
    public string type;
    [SerializeField]
    public float beat_length = -1;

    public Beat(float bnum, string type) {
        this.beat_num = bnum;
        this.type = type;
    }
    public Beat(float bnum, string type, float blen) {
        this.beat_num = bnum;
        this.type = type;
        this.beat_length = blen;
    }

    public int CompareTo(object obj)
    {
        var a = this;
        var b = obj as Beat;
     
        if (a.beat_num < b.beat_num)
            return -1;
     
        if (a.beat_num > b.beat_num)
            return 1;
 
        return 0;
    }
}