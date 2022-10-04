using UnityEngine;
using Random = UnityEngine.Random;


public class RhythmSystem : MonoBehaviour
{
    
    public AudioSource music;

    public Song song;
    public NoteDefinition noteDefs;
    public float num_beats;
    public float sec_per_beat;
    public float start_time;
    public float time_since_start;
    public float current_time;
    public float threshold = .0005f;
    public float current_time_in_beats;
    public GameObject[] prefabs;
    public float[] limits = new float[4];
    public float num_beats_diff = 2f;
    public Material[] materials;

    
    void Start()
    {
        Debug.Log("dronesaber: initializing song and rhythm system");
        // Load the music component from the objects
        music = GetComponent<AudioSource>();
        // Load Song
        // TODO: Define the speed
        song = LoadSongJson("abc");
        // Load note definitions
        noteDefs = LoadDefJson("objects");
        noteDefs.ConstructDict();

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
        if(song.notes.Count > 0) {
            Beat current_note = song.notes[0];
            if((current_note.beat_num - num_beats_diff) < current_time_in_beats + threshold) {
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
        SpawnObject(new Vector3(x, y, noteDefs.z), new Vector3(x, y, noteDefs.z + noteDefs.distance), prefabs[spawn.index]);
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

    void SpawnObject(Vector3 start, Vector3 end, GameObject prefab) {
        GameObject newNote = GameObject.Instantiate(prefab, start, this.transform.rotation);
        MoveInTime moveInTime = newNote.GetComponent<MoveInTime>();
        moveInTime.Setup(sec_per_beat * num_beats_diff, end);
    }

    void ChangeColor() {
        float randRed = Random.Range(0.0f, 1.0f);
        float randBlue = Random.Range(0.0f, 1.0f);
        float randGreen = Random.Range(0.0f, 1.0f);

        GetComponent<Renderer>().material.color = new Color(randRed, randBlue, randGreen);
    }

}