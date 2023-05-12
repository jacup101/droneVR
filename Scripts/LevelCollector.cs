using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCollector : MonoBehaviour
{

    [SerializeField] List<string> level_string_list;
    [SerializeField] List<string> level_editor_list;

    Dictionary<string, Level> level_list;
    

    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TMP_Dropdown le_dropdown;
    public void LoadLevels() {
        level_string_list = new List<string>();
        level_editor_list = new List<string>();
        level_editor_list.Add("new");
        level_list = new Dictionary<string, Level>();
        LoadDefinedLevels();
        LoadCustomLevels();

        dropdown.ClearOptions();
        dropdown.AddOptions(level_string_list);

        LoadCustomLevelEditor();
        le_dropdown.ClearOptions();
        le_dropdown.AddOptions(level_editor_list);


        string pref_level = PlayerPrefs.GetString("custom_level", "abc");
        Debug.Log(pref_level);
        PlayerPrefs.DeleteKey("custom_level");
        PlayerPrefs.Save();

        int index = level_string_list.FindIndex(new IdSearch(pref_level).Equals);
        Debug.Log("Index " + index);
        dropdown.value = index;
    }


    void LoadDefinedLevels() {
        
        var json_obj = Resources.Load<TextAsset>("Config/levels");
        string json_string = json_obj.text;
        LevelContainer lc = JsonUtility.FromJson<LevelContainer>(json_string);
        level_string_list = lc.levels;
        foreach(string l in level_string_list) {
            level_list[l] = new Level(l, false);
        }
    }

    void LoadCustomLevels() {
        
        string json_string = LoadData("Config", "levels");
        if (json_string.Equals("null")) {
            return;
        }
        LevelContainer lc = JsonUtility.FromJson<LevelContainer>(json_string);
        foreach(string lvl in lc.levels) {
            level_string_list.Add(lvl + " C");
        }
        foreach(string l in lc.levels) {
            level_list[l + " C"] = new Level(l, true);
        }
    }

    void LoadCustomLevelEditor() {
        
        string json_string = LoadData("Config", "level_editor_files");
        if (json_string.Equals("null")) {
            return;
        }
        LevelContainer lc = JsonUtility.FromJson<LevelContainer>(json_string);
        foreach(string lvl in lc.levels) {
            level_editor_list.Add(lvl);
        }
    }

    public string LoadData(string directory, string filename) {
        try {
            return System.IO.File.ReadAllText(Application.persistentDataPath + "/" + directory + "/" + filename + ".json");
        }
        catch {
            return "null";
        }
    }

    public Level GetLevel(string name) {
        return level_list[name];
    }
}
