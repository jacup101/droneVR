using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class LevelNoteSerializable
{
    [SerializeField] public float x_pos;
    [SerializeField] public float y_pos;
    [SerializeField] public float beatNum;
    [SerializeField] public float beatDecomposition;
    [SerializeField] public int beatDecompIndex;
    [SerializeField] public string type;
    [SerializeField] public float length;
    [SerializeField] public bool has_length;
    [SerializeField] public int x_snap;
    [SerializeField] public int x_snapIndex;
    [SerializeField] public int y_snap;
    [SerializeField] public int y_snapIndex;

    // "definitions": "0 - Note, 1 - Center Hole, 2 - Horizontal Bar, 3 - Vertical Bar, 4 - Diag Bar L, 5 - Diag Bar R, 6 - Health, 7 - Health Ring, 8 - Death, 9 - Death Ring, 10 - Death Center Hole, 11 - Crystal Heart",

    static Dictionary<string, int> type_to_int = new Dictionary<string, int>(){
        {"center_hole", 1},
        {"death_ring", 9},
        {"diagonal_left", 4},
        {"diagonal_right", 5},
        {"health_ring", 7},
        {"horizontal", 2},
        {"vertical", 3},
        {"death_center_hole", 10},
        {"note", 0},
        {"death", 8},
        {"health", 6},
        {"crystal_heart", 11}
    }; 

    public LevelNoteSerializable(float x, float y, float bNum, float bDec, int bDI, float l, bool hl, string t, int xs, int xsi, int ys, int ysi) {
        this.x_pos = x;
        this.y_pos = y;
        this.beatNum = bNum;
        this.beatDecomposition = bDec;
        this.beatDecompIndex = bDI;
        this.length = l;
        this.has_length = hl;
        this.type = t;
        this.x_snap = xs;
        this.x_snapIndex = xsi;
        this.y_snap = ys;
        this.y_snapIndex = ysi;
    }

    public Beat GetBeat(int offset) {
        Beat toReturn = new Beat(beatNum + beatDecomposition + offset, GetNoteTypeString());
        if (has_length) {
            toReturn.beat_length = length;
        }
        return toReturn;
    }

    public string GetNoteTypeString() {
        return type + "_" + XSnapToStr(x_snap) + "_" + YSnapToStr(y_snap);

    }

    public string XSnapToStr(int i) {
        switch(i) {
            case 0:
                return "RAND";
            case -1:
                return "LEFT";
            case 1:
                return "RIGHT";
            case 2:
                return "MID";
        }
        return "RAND";
    }

    public string YSnapToStr(int i) {
        switch(i) {
            case 0:
                return "RAND";
            case -1:
                return "DOWN";
            case 1:
                return "UP";
            case 2:
                return "MID";
        }
        return "RAND";
    }

    public int GetIndex() {
        return type_to_int[type];
    }

    public string GetAnim() {
        if (type.Equals("health_ring") || type.Equals("death_ring")) {
            return "rotateZ";
        }
        return "none";
    }
}
