using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class LevelContainer
{
    [SerializeField] public List<string> levels;

    public LevelContainer(List<string> strs) {
        this.levels = strs;
    }


}
