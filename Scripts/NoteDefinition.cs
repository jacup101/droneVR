using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class NoteDefinition {
    // Defines the definitions of a level (not related to music)
    // (i.e. which notes spawn, how far back to spawn, how far the note should travel, etc.)
    // Limits for player movement:
    // 0 - x min
    // 1 - x max
    // 2 - y min
    // 3 - y max
    [SerializeField]
    public List<int> limits;
    // Types of notes available in the level
    [SerializeField]
    public List<NoteType> types;
    // z to spawn notes at
    [SerializeField]
    public int z;
    // how far the notes move to arrive at player's position
    [SerializeField]
    public int distance;
    // Number of beats to spawn note before it actually is played (i.e. 2, so that if a note plays at beat num 4, it spawns at beat num 2 and moves in time until arriving at 4)
    [SerializeField]
    public int beats;
    // Available animation types for notes
    [SerializeField]
    public List<string> animTypes;
    // Dictionary which contains the definitions of what notes can spawn
    public Dictionary<string, NoteType> noteTypeDict;
    // Constructor
    public NoteDefinition(int z, int distance, List<int> limits, List<NoteType> types) {
        this.z = z;
        this.distance = distance;
        this.limits = limits;
        this.types = types;
    }
    // Constructs a dictionary of note types, which can be used to spawn new ones
    public void ConstructDict() {
        this.noteTypeDict = new Dictionary<string, NoteType>();
        foreach(NoteType noteType in this.types) {
            Debug.LogFormat("dronesaber: Adding def for {0}", noteType.type);
            noteTypeDict.Add(noteType.type, noteType);
        }
    }
    // Getters for limits
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