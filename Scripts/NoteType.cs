using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class NoteType {
    // Defines a note, including which gameobject it is, where it can spawn, and animation type
    // Type of note (corresponds to gameobect, i.e. centerhole)
    [SerializeField]
    public string type;
    // Index in the definitions list
    [SerializeField]
    public int index;
    // Where it can spawn in both x and y
    // -1 - Spawn at left (down) limit
    // 0 - Spawn randomly between limits
    // 1 - Spawn at right (up) limit
    // 2 - Spawn at the centerpoint between both limits
    [SerializeField]
    public int x;
    [SerializeField]
    public int y;
    // Animation type
    [SerializeField]
    public string anim;

    public NoteType(string type, int index, int x, int y) {
        this.type = type;
        this.index = index;
        this.x = x;
        this.y = y;
    }

    public NoteType(string type, int index, int x, int y, string anim) {
        this.type = type;
        this.index = index;
        this.x = x;
        this.y = y;
        this.anim = anim;
    }
}