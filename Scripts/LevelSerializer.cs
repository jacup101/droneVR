using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class LevelSerializer : MonoBehaviour
{
    [SerializeField]
    Staff note_staff;
    [SerializeField]
    Staff event_staff;
    [SerializeField]
    bool do_loading;

    [SerializeField]
    TMP_InputField song_name;
    [SerializeField]
    TMP_InputField bpm;
    [SerializeField]
    TMP_InputField song_length;
    [SerializeField]
    TMP_InputField spawn_dist;
    [SerializeField]
    TMP_InputField start_at;
    [SerializeField]
    TMP_InputField travel_time;
    [SerializeField]
    List<GameObject> prefabs;
    [SerializeField]
    List<GameObject> evnt_prefabs;

    [SerializeField]
    List<string> ids;
    [SerializeField]
    List<string> evnt_ids;

    void Start() {
        if (do_loading) {
            string level_name = PlayerPrefs.GetString("editor_level", "new");
            PlayerPrefs.DeleteKey("editor_level");
            PlayerPrefs.Save();
            print(level_name);
            if (level_name.Equals("new")) {
                song_name.text = "CustomLevel";
                bpm.text = "120";
                song_length.text = "2:25";
                spawn_dist.text = "50";
                start_at.text = "4";
                travel_time.text = "4";
            } else {
                string data = LoadData("LevelEditorData", level_name);
                if(!data.Equals("null")) {
                    DeserializeToStaff(data);
                } else {
                    song_name.text = "CustomLevel";
                    bpm.text = "120";
                    song_length.text = "2:25";
                    spawn_dist.text = "50";
                    start_at.text = "4";
                    travel_time.text = "4";
                }
            }
        }
        
    }

    private string GetSongName() {
        // Make sure that it can save
        if (!song_name.text.Equals("")) {
            return song_name.text;
        }
        return "CustomLevel";
    }

    private int GetBpm() {
        try {
            return int.Parse(bpm.text);
        }
        catch {
            return 100;
        }
    }

    private int GetSpawnDist() {
        try {
            return int.Parse(spawn_dist.text);
        }
        catch {
            return 50;
        }
    }

    private int GetStartAt() {
        try {
            return int.Parse(start_at.text);
        }
        catch {
            return 4;
        }
    }

    private int GetTravelTime() {
        try {
            return int.Parse(travel_time.text);
        }
        catch {
            return 4;
        }
    }


    private int[] GetSongLength() {
        string[] split = song_length.text.Split(":");
        try {
            return new int[]{int.Parse(split[0]), int.Parse(split[1])};
        } catch {
            return new int[]{2,25};
        }
    }

    public void Serialize() {
        SerializeLevelEditor();
        SerializeLevel();
    }

    public void SerializeLevelEditor() {
        LevelSerializable ls = new LevelSerializable(GetSongName(), GetBpm(), GetSongLength()[0], GetSongLength()[1], GetSpawnDist(), GetStartAt(), GetTravelTime(),SerializeStaffToEditorNotes(), SerializeStaffToEditorEvents());
        string serialized_level = JsonUtility.ToJson(ls);
        SaveData(serialized_level, "LevelEditorData", GetSongName());
        SaveToConfig("level_editor_files", GetSongName());
        print(serialized_level);

    }



    public void SerializeLevel() {

        List<NoteType> types = new List<NoteType>();
        List<Beat> notes = new List<Beat>();
        List<Beat> events = new List<Beat>();
        List<string> type_strings = new List<string>();
        int start_at = GetStartAt();

        foreach(GameObject obj in note_staff.GetEditorNotes()) {
            LevelNoteSerializable serialized = obj.GetComponent<EditorNote>().Serialize();
            if (!type_strings.Contains(serialized.GetNoteTypeString())) {
                AddNewType(types, type_strings, serialized);
            }
            notes.Add(serialized.GetBeat(start_at));

        }
        foreach(GameObject obj in event_staff.GetEditorNotes()) {
            LevelEventSerializable serialized = obj.GetComponent<EditorNote>().SerializeEvent();
            events.Add(serialized.GetBeat(0));
        }

        notes.Sort();
        events.Sort();

        NoteDefinition ndf = new NoteDefinition(GetSpawnDist(), (-1 * GetSpawnDist()) + 5, new List<int>(){-2, 2, -2, 2}, types, GetTravelTime(), new List<string>(){"rotateZ"});
        Song song = new Song(GetSongName(), GetBpm(), GetSongLength()[0], GetSongLength()[1], events, notes);
        string serialized_definitions = JsonUtility.ToJson(ndf);
        string serialized_song = JsonUtility.ToJson(song);
        SaveData(serialized_definitions, "Definitions", GetSongName());
        SaveData(serialized_song, "Levels", GetSongName());
        SaveToConfig("levels", GetSongName());
        print("OffbeatVR Serialize Definition: \n\n" + serialized_definitions);
        print("OffbeatVR Serialize Song: \n\n" + serialized_song);
    }

    public void SaveToConfig(string config_file, string name) {
        string json = LoadData("Config", config_file);
        LevelContainer lc;
        if (!json.Equals("null")) {
            lc = JsonUtility.FromJson<LevelContainer>(json);
        } else {
            lc = new LevelContainer(new List<string>());
        }
        if (!lc.levels.Contains(name)) {
            lc.levels.Add(name);
        }
        SaveData(JsonUtility.ToJson(lc), "Config", config_file);
    }

    public void SaveData(string json, string directory, string filename) {
        try
        {
            if (!Directory.Exists(Application.persistentDataPath + "/" + directory))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/" + directory);
            }
 
        }
        catch (IOException ex)
        {
            Debug.LogError(ex.Message);
        }

        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + directory + "/" + filename + ".json", json);
    }

    public string LoadData(string directory, string filename) {
        try {
            return System.IO.File.ReadAllText(Application.persistentDataPath + "/" + directory + "/" + filename + ".json");
        }
        catch {
            return "null";
        }
    }
    

    private void DeserializeToStaff(string json) {
        Debug.Log("Deserializing on: " + json);
        LevelSerializable lvl = JsonUtility.FromJson<LevelSerializable>(json);
        song_name.text = lvl.name;
        bpm.text = "" + lvl.bpm;
        song_length.text = lvl.length_min + ":" + lvl.length_sec;
        spawn_dist.text = "" + lvl.spawn_at;
        start_at.text = "" + lvl.start_at;
        travel_time.text = "" + lvl.travel_time;

        foreach (LevelNoteSerializable note in lvl.notes) {
            note_staff.InstantiateNote(prefabs[ids.FindIndex(new IdSearch(note.type).Equals)], note.type, new Vector3(note.x_pos, note.y_pos, 0), note);
        }
        foreach (LevelEventSerializable evnt in lvl.events) {
            event_staff.InstantiateEvent(evnt_prefabs[evnt_ids.FindIndex(new IdSearch(evnt.type).Equals)], evnt.type, new Vector3(evnt.x_pos, evnt.y_pos, 0), evnt);

        }
    }


    private List<LevelNoteSerializable> SerializeStaffToEditorNotes() {
        List<LevelNoteSerializable> notes = new List<LevelNoteSerializable>();
        foreach(GameObject obj in note_staff.GetEditorNotes()) {
            notes.Add(obj.GetComponent<EditorNote>().Serialize());
        }
        return notes;
    }

    private List<LevelEventSerializable> SerializeStaffToEditorEvents() {
        List<LevelEventSerializable> events = new List<LevelEventSerializable>();
        foreach(GameObject obj in event_staff.GetEditorNotes()) {
            events.Add(obj.GetComponent<EditorNote>().SerializeEvent());
        }
        return events;
    }

    private void AddNewType(List<NoteType> types, List<string> existing_types, LevelNoteSerializable note) {
        NoteType type = new NoteType(note.GetNoteTypeString(), note.GetIndex(), note.x_snap, note.y_snap, note.GetAnim());
        types.Add(type);
        existing_types.Add(note.GetNoteTypeString());
    }
}
