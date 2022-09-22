using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;


public class RhythmSystem : MonoBehaviour
{
    
    public AudioSource music;

    public Song song;
    public float num_beats;
    public float sec_per_beat;
    public float start_time;
    public float time_since_start;
    public float current_time;
    public float current_time_in_beats;

    
    void Start()
    {
        // Load the music component from the objects
        music = GetComponent<AudioSource>();
        // Load Song
        song = LoadJson("test");
        // Initialize properties used by the rhythm system
        num_beats = song.bpm * song.length_min + (song.bpm * (song.length_sec / 60));

        sec_per_beat = 60f / song.bpm;


        //Record the time when the music starts
        start_time = (float) AudioSettings.dspTime;

        //Start the music
        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        current_time = (float)(AudioSettings.dspTime - start_time);
        current_time_in_beats = current_time / sec_per_beat;

        Debug.Log(current_time_in_beats);
    }

    Song LoadJson(string song_name) {
        string path = "Assets/Levels/" + song_name + ".json";
        StreamReader reader = new StreamReader(path);
        string json_string = reader.ReadToEnd();
        reader.Close();
        Song my_song = JsonUtility.FromJson<Song>(json_string);
        return my_song;
    }

}