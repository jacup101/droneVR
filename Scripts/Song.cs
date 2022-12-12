using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Song {
    // Contains the events and notes queues (in the form of a list, due to JSON definition), as well as tempo, song length, and song name
    // Fields
    [SerializeField]
    public string name;
    [SerializeField]
    public int bpm;
    [SerializeField]
    public int length_min;
    [SerializeField]
    public int length_sec;
    [SerializeField]
    public List<Beat> events;
    public List<Beat> notes;
    // Constructor
    public Song(string name, int bpm, int lmin, int lsec, List<Beat> beats, List<Beat> notes) {
        this.name = name;
        this.bpm = bpm;
        this.length_min = lmin;
        this.length_sec = lsec;
        this.events = beats;
        this.notes = notes;
    }
}