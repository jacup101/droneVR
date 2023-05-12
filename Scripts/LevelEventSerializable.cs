using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class LevelEventSerializable
{
    [SerializeField] public float x_pos;
    [SerializeField] public float y_pos;
    [SerializeField] public float beatNum;
    [SerializeField] public float beatDecomposition;
    [SerializeField] public int beatDecompIndex;
    [SerializeField] public string type;
    [SerializeField] public int rotation;
    [SerializeField] public bool corroded;
    [SerializeField] public bool rainbow;



    // "definitions": "0 - Note, 1 - Center Hole, 2 - Horizontal Bar, 3 - Vertical Bar, 4 - Diag Bar L, 5 - Diag Bar R, 6 - Health, 7 - Health Ring, 8 - Death, 9 - Death Ring, 10 - Death Center Hole, 11 - Crystal Heart",


    public LevelEventSerializable(float x, float y, float bNum, float bDec, int bDI, string t, int rot, bool c, bool r) {
        this.x_pos = x;
        this.y_pos = y;
        this.beatNum = bNum;
        this.beatDecomposition = bDec;
        this.beatDecompIndex = bDI;
        this.type = t;
        this.rotation = rot;
        this.corroded = c;
        this.rainbow = r;

    }

    public Beat GetBeat(int offset) {
        return new Beat(this.beatNum + this.beatDecomposition + offset, this.GetBeatType());
    }

    private string GetBeatType() {
        if (this.type.Contains("stop")) {
            return "stop_tunnel";
        } else {
            string construct = "start_"  + type;
            if (corroded) {
                construct += "_random";
            }
            if (rainbow) {
                construct += "_color";
            }
            switch(this.rotation) {
                case 0:
                    return construct;
                case 1:
                    return construct + "_randrot";
                case 2:
                    return construct + "_rotating_left";
                case 3:
                    return construct + "_rotating_right";
            }
            return construct;
        }

    }





}
