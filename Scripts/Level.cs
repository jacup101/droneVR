using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Level
{
    public string name;
    public bool is_custom;

    public Level(string n, bool i) {
        this.name = n;
        this.is_custom = i;

    }

    public Song LoadLevelJSON() {
        if (is_custom) {
            return LoadLevelJSONCustom();
        }
        return LoadLevelJSONDefined();
    }

    public NoteDefinition LoadDefinitionJSON() {
        if (is_custom) {
            return LoadDefinitionJSONCustom();
        }
        return LoadDefinitionJSONDefined();
    }



    public Song LoadLevelJSONDefined() {

        // Load the json file and put it into a string
        var json_obj = Resources.Load<TextAsset>("Levels/" + name);
        string json_string = json_obj.text;
        // Debug.Log(json_string);
        // Use Unity's JSON serialization to convert from a string to a Song instance
        Song my_song = JsonUtility.FromJson<Song>(json_string);
        return my_song;
    
    }

    public Song LoadLevelJSONCustom() {

        string json_string = LoadData("Levels", name);
        // Debug.Log(json_string);
        // Use Unity's JSON serialization to convert from a string to a Song instance
        Song my_song = JsonUtility.FromJson<Song>(json_string);
        return my_song;
    
    }

    public NoteDefinition LoadDefinitionJSONDefined() {
        // Load the json file and put it into a string
        var json_obj = Resources.Load<TextAsset>("Definitions/" + name);
        string json_string = json_obj.text;
        // Debug.Log(json_string);
        // Use Unity's JSON serialization to convert from a string to a NoteDefinition instance
        NoteDefinition definition = JsonUtility.FromJson<NoteDefinition>(json_string);
        return definition;
    }

    public NoteDefinition LoadDefinitionJSONCustom() {
        string json_string = LoadData("Definitions", name);
        // Debug.Log(json_string);
        // Use Unity's JSON serialization to convert from a string to a Song instance
        NoteDefinition my_song = JsonUtility.FromJson<NoteDefinition>(json_string);
        return my_song;
    }
    public string LoadData(string directory, string filename) {
        try {
            return System.IO.File.ReadAllText(Application.persistentDataPath + "/" + directory + "/" + filename + ".json");
        }
        catch {
            return "null";
        }
    }

}
