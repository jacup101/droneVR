using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Random=UnityEngine.Random;


public class RhythmSystem : MonoBehaviour
{
    
    public AudioSource music;

    public Song song;
    public float num_beats;
    public float sec_per_beat;
    public float start_time;
    public float time_since_start;
    public float current_time;
    public float threshold = .0005f;
    public float current_time_in_beats;
    public Material[] materials;

    
    void Start()
    {
        Debug.Log("dronesaber: initializing song and rhythm system");
        // Load the music component from the objects
        music = GetComponent<AudioSource>();
        // Load Song
        song = LoadJson("test");

        // Initialize properties used by the rhythm system
        num_beats = song.bpm * song.length_min + (song.bpm * (song.length_sec / 60));

        sec_per_beat = 60f / song.bpm;


        //Record the time when the music starts
        start_time = (float) AudioSettings.dspTime;
        current_time_in_beats = -1;
        //Start the music
        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        current_time = (float)(AudioSettings.dspTime - start_time);
        current_time_in_beats = current_time / sec_per_beat;
        if(song.events.Count > 0) {
            Beat current_beat = song.events[0];
            if(current_beat.beat_num < current_time_in_beats + threshold) {
                ChangeColor();
                

                song.events.RemoveAt(0);
            }
        }

        // Debug.Log(current_time_in_beats);
    }

    Song LoadJson(string song_name) {
        var json_obj = Resources.Load<TextAsset>("Levels/" + song_name);
        string json_string = json_obj.text;
        Debug.Log(json_string);
        Song my_song = JsonUtility.FromJson<Song>(json_string);
        return my_song;
    }

    void ChangeColor() {
        float randRed = Random.Range(0.0f, 1.0f);
        float randBlue = Random.Range(0.0f, 1.0f);
        float randGreen = Random.Range(0.0f, 1.0f);

        GetComponent<Renderer>().material.color = new Color(randRed, randBlue, randGreen);
    }

}