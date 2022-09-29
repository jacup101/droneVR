using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class NoteType {
    [SerializeField]
    public string type;
    [SerializeField]
    public int index;
    [SerializeField]
    public int x;
    [SerializeField]
    public int y;

    public NoteType(string type, int index, int x, int y) {
        this.type = type;
        this.index = index;
        this.x = x;
        this.y = y;
    }
}