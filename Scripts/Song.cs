using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.Json;
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
    public List<Beat> beats;

    public Song(string name, int bpm, int lmin, int lsec, List<Beat> beats) {
        this.name = name;
        this.bpm = bpm;
        this.length_min = lmin;
        this.length_sec = lsec;
        this.beats = beats;
    }
}