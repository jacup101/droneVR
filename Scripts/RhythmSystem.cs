using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class RhythmSystem : MonoBehaviour
{
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
    public bool stopped = false;
    public GameObject[] prefabs;
    public float[] limits = new float[4];
    public Material[] materials;
    // Spawned in related fields
    public List<GameObject> inWorldObjects;
    // Event related fields
    public CorridorHandler ch;
    // Bools to control things
    [SerializeField]
    bool spawnNotes = true;
    [SerializeField]
    bool spawnEvents = true;

    
    void Start()
    {
        Debug.Log("dronesaber: initializing song and rhythm system");
        // Load the music component from the objects
        music = GetComponent<AudioSource>();
        // Load Song
        // TODO: Define the speed
        song = LoadSongJson("abc");
        // Load note definitions
        noteDefs = LoadDefJson("abc");
        noteDefs.ConstructDict();

        ch = GetComponent<CorridorHandler>();

        // Initialize properties used by the rhythm system
        num_beats = song.bpm * song.length_min + (song.bpm * (song.length_sec / 60));

        sec_per_beat = 60f / song.bpm;


        //Record the time when the music starts
        start_time = (float) AudioSettings.dspTime;
        current_time_in_beats = -1;
        //Start the music
        music.Play();
        // Initialize the list
    }

    // Update is called once per frame
    void Update()
    {
        if(!stopped) {
           RunRhythmSystem();
        }
    }

    void RunRhythmSystem() {

         current_time = (float)(AudioSettings.dspTime - start_time);
            current_time_in_beats = current_time / sec_per_beat;
            if(song.events.Count > 0 && spawnEvents) {
                Beat current_beat = song.events[0];
                if(current_beat.beat_num < current_time_in_beats + threshold) {
                    EventCorridorDecision(current_beat);
                    song.events.RemoveAt(0);
                }
            }
            if(song.notes.Count > 0 && spawnNotes) {
                Beat current_note = song.notes[0];
                if((current_note.beat_num - noteDefs.beats) < current_time_in_beats + threshold) {
                    // Debug.Log(current_note.beat_num - num_beats_diff);
                    // Debug.Log(current_time_in_beats + threshold);
                    SpawnObject(current_note.type);
                    song.notes.RemoveAt(0);
                }
            }
    }

    Song LoadSongJson(string song_name) {
        var json_obj = Resources.Load<TextAsset>("Levels/" + song_name);
        string json_string = json_obj.text;
        Debug.Log(json_string);
        Song my_song = JsonUtility.FromJson<Song>(json_string);
        return my_song;
    }

    NoteDefinition LoadDefJson(string loc) {
        var json_obj = Resources.Load<TextAsset>("Definitions/" + loc);
        string json_string = json_obj.text;
        Debug.Log(json_string);
        NoteDefinition definition = JsonUtility.FromJson<NoteDefinition>(json_string);
        return definition;
    }

    // Spawns an object at the given position with the desired properties.
    // Note that this uses NoteDefinition, however, this method can not be placed in that class as it is not a monobehaviour
    void SpawnObject(string property) {
        NoteType spawn = noteDefs.noteTypeDict[property];
        float x = GetCoordinate(spawn.x, noteDefs.GetXMin(), noteDefs.GetXMax());
        float y = GetCoordinate(spawn.y, noteDefs.GetYMin(), noteDefs.GetYMax());
        string animType = spawn.anim;
        if(animType == "random")
            animType = noteDefs.animTypes[Random.Range(0, noteDefs.animTypes.Count)];
        SpawnObject(new Vector3(x, y, noteDefs.z), new Vector3(x, y, noteDefs.z + noteDefs.distance), animType,prefabs[spawn.index]);
    }

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

    void SpawnObject(Vector3 start, Vector3 end, string anim, GameObject prefab) {
        GameObject newNote = GameObject.Instantiate(prefab, start, this.transform.rotation);
        // Add the note game object to the list, each game object will take care of removing itself when necessary
        inWorldObjects.Add(newNote);
        MoveInTime moveInTime = newNote.GetComponent<MoveInTime>();
        moveInTime.Setup(sec_per_beat * noteDefs.beats, end, anim, inWorldObjects);
    }

    void EventCorridorDecision(Beat beat) {
        if(beat.type.Contains("start")) {
            ch.StartCorridor(noteDefs.z, .5f, beat.type);
        }
        if(beat.type == "stop_tunnel") {
            ch.StopCorridor();
        }
    }

    void ChangeColor() {
        float randRed = Random.Range(0.0f, 1.0f);
        float randBlue = Random.Range(0.0f, 1.0f);
        float randGreen = Random.Range(0.0f, 1.0f);

        GetComponent<Renderer>().material.color = new Color(randRed, randBlue, randGreen);
    }

    public void Die() {
        this.stopped = true;
        music.Stop();
        foreach(GameObject gameobj in inWorldObjects) {
            if(gameobj != null) {
                Rigidbody rb = gameobj.GetComponent<Rigidbody>();
                if(rb == null) {
                    rb = gameobj.AddComponent<Rigidbody>();
                    rb.AddForce(Random.onUnitSphere * 1000);
                    rb.useGravity = true;
                    MoveInTime mit = gameobj.GetComponent<MoveInTime>();
                    mit.moving = false;
                }
            } 
            // rb.AddForce(Random.onUnitSphere * 1000);
        }
        ch.Explode();
    }

}