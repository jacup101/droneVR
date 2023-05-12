using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RhythmSystem : MonoBehaviour
{
    // Rhythm system handles the spawning of notes and the changing of visual events, in time with the music as defined in the level json
    // Music Related Fields
    public AudioSource music;
    public Song song;
    public NoteDefinition noteDefs;
    public float num_beats;
    public float sec_per_beat;
    // Rhythm System Related Fields
    public float start_time;
    public float time_since_start;
    public float current_time;
    public float threshold = .0005f;
    public float current_time_in_beats;
    // Spawning Related Fields
    public GameObject[] prefabs;
    public Material[] materials;
    // Spawned in related fields
    public List<GameObject> inWorldObjects;
    // State fields
    public bool stopped = true;
    public bool spawnNotes = true;
    public bool spawnEvents = true;
    public bool dead = false;
    // Current Song
    string current_song = "abc";
    // Other classes
    public CorridorHandler ch;
    // Game Settings
    bool simpleVisual = false;
    // Offset for when the rhythm system is paused, to account for elapsed time where the game is not actually running
    float pauseOffset;
    // Level System
    public LevelCollector lc;
    // Score System
    public ScoreSystem ss;
    void Start()
    {
        lc.LoadLevels();
        // If run in the Unity editor, jump straight into the song defined in dev_level instead of loading main menu (since there's no way to interact with the menu when not in VR)
        #if UNITY_EDITOR
        string dev_level = "test_length";
        BeginLevel(dev_level);
        #endif
    }
    // Start a level, based on the song name
    public void BeginLevel(string song_name) {
        Debug.Log("offbeat: initializing song and rhythm system");
        current_song = song_name;
                
        // Load Song
        song = LoadSongJson(song_name);
        // Load note definitions
        noteDefs = LoadDefJson(song_name);
        noteDefs.ConstructDict();

        // Load the song file from the resources folder
        AudioClip ac = (AudioClip) Resources.Load("Songs/" + song_name);
        music.clip = ac;

        // Initialize properties used by the rhythm system
        num_beats = song.bpm * song.length_min + (song.bpm * (song.length_sec / 60));

        sec_per_beat = 60f / song.bpm;


        //Record the time when the music starts
        start_time = (float) AudioSettings.dspTime;
        current_time_in_beats = -1;
        //Start the music
        music.Play();
        // Reset other variables
        stopped = false;
        pauseOffset = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // If paused, pass the elapsed time to a variable so that it can be taken into account when game is unpaused
        if(stopped) {
            pauseOffset += Time.deltaTime;
        }
        // If not paused, run the game
        if(!stopped && !dead) {
           RunRhythmSystem();
        }
    }
    // Runs the rhythm system
    void RunRhythmSystem() {
        // Grab the current time
        current_time = (float)(AudioSettings.dspTime - start_time - pauseOffset);
        // Calculate the current time, but in terms of beats (i.e. the current beat)
        current_time_in_beats = current_time / sec_per_beat;
        // Handle the spawning of events
        if(song.events.Count > 0 && spawnEvents) {
            Beat current_beat = song.events[0];
            // If an event is played at current beat, handle it and then remove from queue
            if(current_beat.beat_num < current_time_in_beats + threshold) {
                EventCorridorDecision(current_beat);
                song.events.RemoveAt(0);
            }
        }
        // Handle the spawning of notes
        if(song.notes.Count > 0 && spawnNotes) {
            Beat current_note = song.notes[0];
            // If a note is spawned at current beat, handle it and then remove from queue
            if((current_note.beat_num - noteDefs.beats) < current_time_in_beats + threshold) {
                SpawnObject(current_note.type, current_note.beat_length);
                song.notes.RemoveAt(0);
            }
        }
    }
    // Loads the level json for a level
    Song LoadSongJson(string song_name) {
        Level lvl = lc.GetLevel(song_name);
        return lvl.LoadLevelJSON();
    }
    // Loads the definition json for a level
    NoteDefinition LoadDefJson(string loc) {
        Level lvl = lc.GetLevel(loc);
        return lvl.LoadDefinitionJSON();
    }
    // Spawns an object at the given position with the desired properties.
    // Note that this uses NoteDefinition, however, this method can not be placed in that class as it is not a monobehaviour
    void SpawnObject(string property, float beat_length) {
        // Grab the appropriate note type from the definitions
        // Note that it is assumed levels are defined correctly
        NoteType spawn = noteDefs.noteTypeDict[property];
        // Grab the in world coordinates from the definition
        float x = GetCoordinate(spawn.x, noteDefs.GetXMin(), noteDefs.GetXMax());
        float y = GetCoordinate(spawn.y, noteDefs.GetYMin(), noteDefs.GetYMax());
        // Set animation type, handle if it is random
        string animType = spawn.anim;
        if(animType == "random")
            animType = noteDefs.animTypes[Random.Range(0, noteDefs.animTypes.Count)];
        if(simpleVisual)
            animType = "none";
            // Spawn the Object
        SpawnObject(new Vector3(x, y, noteDefs.z), new Vector3(x, y, noteDefs.z + noteDefs.distance), beat_length, animType, prefabs[spawn.index]);
    }
    // Converts the note definition coordinate to an in game coordinate, based on the limits provided
    float GetCoordinate(int modifier, int min, int max) {
        switch (modifier) {
            case -1:
                return min;
            case 0:
                return Random.Range(min, max);
            case 1:
                return max;
            case 2:
                return (min + max) / 2;
        }
        return 0;
    }
    // Spawns an note in game
    void SpawnObject(Vector3 start, Vector3 end, float beat_length, string anim, GameObject prefab) {
        // Instantiate the note
        GameObject newNote = GameObject.Instantiate(prefab, start, Quaternion.Euler(0, 0, 0));
        // Add the note game object to the list, each game object will take care of removing itself when necessary
        inWorldObjects.Add(newNote);
        VariableNoteLength varLength = newNote.GetComponent<VariableNoteLength>();
        if (beat_length != -1) {
            varLength.Setup(start.z, end.z, noteDefs.beats, beat_length);
        }
        // Apply the move in time script and set it up with all necessary variables
        MoveInTime moveInTime = newNote.GetComponent<MoveInTime>();
        moveInTime.Setup(sec_per_beat * noteDefs.beats, end, anim, inWorldObjects, noteDefs.limits, varLength, ss);
        
    }
    // Change the corridor based on events
    void EventCorridorDecision(Beat beat) {
        // Start an event, either as defined in the event beat, or as a simple corridor if accessibility mode is enabled (simple visual)
        if(beat.type.Contains("start")) {
            if(!simpleVisual) {
                ch.HandleEvent(noteDefs.z, .5f, beat.type);
            } else {
                ch.HandleEvent(noteDefs.z, .5f, "start_tunnel_random");
            }
        }
        // Stop the corridor
        if(beat.type == "stop_tunnel") {
            ch.StopCorridor();
        }
        // For demo purposes, transitions to the next song if a reset event is reached
        if (beat.type == "reset") {
            string[] song_sequence = new string[]{"abc", "pixel_galaxy"};
            if(current_song == "abc") {
                BeginLevel(song_sequence[1]);
            } else {
                BeginLevel(song_sequence[0]);
            }
        }
    }
    // Not used
    // Another event type which changes the color of the player, although player is not currently rendered so has no effect
    void ChangeColor() {
        float randRed = Random.Range(0.0f, 1.0f);
        float randBlue = Random.Range(0.0f, 1.0f);
        float randGreen = Random.Range(0.0f, 1.0f);
        GetComponent<Renderer>().material.color = new Color(randRed, randBlue, randGreen);
    }
    // Called when player loses the game
    public void Die() {
        // Stop music and spawning of notes/events
        this.stopped = true;
        music.Stop();
        // Explode the notes, or fall if simple visual
        foreach(GameObject gameobj in inWorldObjects) {
            if(gameobj != null) {
                Rigidbody rb = gameobj.GetComponent<Rigidbody>();
                if(rb == null) {
                    rb = gameobj.AddComponent<Rigidbody>();
                    if(!simpleVisual) {
                        rb.AddForce(Random.onUnitSphere * 1000);
                    }
                    rb.useGravity = true;
                    MoveInTime mit = gameobj.GetComponent<MoveInTime>();
                    mit.moving = false;
                }
            } 
        }
        // Explode the corridor, or pause the corridor if simple visual
        if(!simpleVisual) {
            ch.Explode();
        }
        else {
            ch.Pause();
        }
        // Mark that the player has lost
        dead = true;
    }
    // Resets the game, with current song
    public void ResetGame() {
        ResetGame(current_song);
    }
    // Resets the game
    public void ResetGame(string level) {
        // Stop music if it is playing
        if (music != null) {
            music.Stop();
        }
        // Destroy in world objects
        foreach(GameObject gameobj in inWorldObjects) {
            if(gameobj != null) {
                Destroy(gameobj);
            }
        }
        // Mark that the player is no longer dead
        dead = false;
        // Start rhythm system
        BeginLevel(level);
    }
    // Pauses the game
    public void Pause() {
        this.stopped = true;
        // Pause the music
        music.Pause();
        // Pause the corridor
        ch.Pause();
        // Pause each note
        foreach(GameObject gameobj in inWorldObjects) {
            if(gameobj != null) {
                gameobj.GetComponent<MoveInTime>().Pause();
            }
        }
    }
    // Unpauses the game
    public void Unpause() {
        this.stopped = false;
        // Unpause each note
        foreach(GameObject gameobj in inWorldObjects) {
            if(gameobj != null) {
                gameobj.GetComponent<MoveInTime>().Unpause();
            }
        }
        // Unpause the corridor and music
        ch.Unpause();
        music.UnPause();
    }
    // Sets accessibility mode
    public void SetAccessibility(bool mode) {
        simpleVisual = mode;
    }
}