using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class NoteDefinition {
    // Limits:
    // 0 - x min
    // 1 - x max
    // 2 - y min
    // 3 - y max

    [SerializeField]
    public List<int> limits;

    [SerializeField]
    public List<NoteType> types;

    [SerializeField]
    public int z;
    [SerializeField]
    public int distance;
    [SerializeField]
    public int beats;
    [SerializeField]
    public List<string> animTypes;
    public Dictionary<string, NoteType> noteTypeDict;

    public NoteDefinition(int z, int distance, List<int> limits, List<NoteType> types) {
        this.z = z;
        this.distance = distance;
        this.limits = limits;
        this.types = types;
    }
    public void ConstructDict() {
        this.noteTypeDict = new Dictionary<string, NoteType>();
        foreach(NoteType noteType in this.types) {
            Debug.LogFormat("dronesaber: Adding def for {0}", noteType.type);
            noteTypeDict.Add(noteType.type, noteType);
        }
    }
    public int GetXMin() {
        return limits[0];
    }
    public int GetXMax() {
        return limits[1];
    }
    public int GetYMin() {
        return limits[2];
    }
    public int GetYMax() {
        return limits[3];
    }
}