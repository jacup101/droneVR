using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Song {
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

    public Song(string name, int bpm, int lmin, int lsec, List<Beat> beats, List<Beat> notes) {
        this.name = name;
        this.bpm = bpm;
        this.length_min = lmin;
        this.length_sec = lsec;
        this.events = beats;
        this.notes = notes;
    }
}