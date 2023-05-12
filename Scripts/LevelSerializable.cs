using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSerializable
{
    [SerializeField] public string name;
    [SerializeField] public int bpm;
    [SerializeField] public int length_min;
    [SerializeField] public int length_sec;
    [SerializeField] public int spawn_at;
    [SerializeField] public int start_at;
    [SerializeField] public int travel_time;
    [SerializeField] public List<LevelNoteSerializable> notes;
    [SerializeField] public List<LevelEventSerializable> events;

    public LevelSerializable(string n, int b, int m, int s, int sp, int st, int tt, List<LevelNoteSerializable> nt, List<LevelEventSerializable> ev) {
        this.name = n;
        this.bpm = b;
        this.length_min = m;
        this.length_sec = s;
        this.spawn_at = sp;
        this.start_at = st;
        this.travel_time = tt;
        this.notes = nt;
        this.events = ev;
    }
}
