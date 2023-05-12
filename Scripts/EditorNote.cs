using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditorNote : MonoBehaviour
{
    const int beatValue = 1;
    [SerializeField] const float staffOffset = 0.2f; 
    [SerializeField] GameObject note;
    [SerializeField] GameObject[] notePrefabs = new GameObject[]{};
    [SerializeField] GameObject[] eventPrefabs = new GameObject[]{};
    [SerializeField] CanvasManager cm;
    [SerializeField] CanvasManager cme;
    [SerializeField] Staff staff;
    [SerializeField] Staff event_staff;

    [SerializeField] GameObject lengthObject;
    [SerializeField] TMP_Text val_text;
    
    // Properties Needed to be Saved
    [SerializeField] public float beatNum;
    [SerializeField] public float beatDecomposition;
    [SerializeField] public int beatDecompIndex;
    [SerializeField] public string type;
    [SerializeField] public float length;
    [SerializeField] public bool has_length;
    [SerializeField] public int x_snap;
    [SerializeField] public int x_snapIndex;
    [SerializeField] public int y_snap;
    [SerializeField] public int y_snapIndex;
    // 0 - none
    // 1 - randrot
    // 2 - rotating_left
    // 3 - rotating_right
    [SerializeField] public int rotation;
    [SerializeField] public bool tunnel_is_rand;
    [SerializeField] public bool tunnel_is_rainbow;
    [SerializeField] public bool isEvent = false;


    // Start is called before the first frame update
    void Start()
    {
        SetupRefs();
    }
    
    private void SetupRefs() {
        NoteEventSwitcher nes = GameObject.Find("NoteEventSwitcher").GetComponent<NoteEventSwitcher>();
        cm = nes.note_manager;
        cme = nes.event_manager;
        staff = nes.note_staff;
        event_staff = nes.event_staff;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetBeatNum() {
        return beatNum;
    }

    public void SetBeat(float num, float decomposition, int index) {
        this.beatNum = num;
        this.beatDecomposition = decomposition;
        this.beatDecompIndex = index;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, beatNum + beatDecomposition + staffOffset);
        if (isEvent) {
            cme.SelectIndex(this.beatDecompIndex);
            cme.SetText("Current Val: " + (this.beatNum + this.beatDecomposition));
        } else {
            cm.SelectIndex(this.beatDecompIndex);
            cm.SetText("Current Val: " + (this.beatNum + this.beatDecomposition));
        }
        val_text.text = "" + (this.beatNum + this.beatDecomposition);
    }

    public void SetBeatValue(float num) {
        if (num == -1) {
            return;
        }
        this.beatNum = num;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, beatNum + beatDecomposition + staffOffset);
        if (isEvent) {
            cme.SelectIndex(this.beatDecompIndex);
            cme.SetText("Current Val: " + (this.beatNum + this.beatDecomposition));
        } else {
            cm.SelectIndex(this.beatDecompIndex);
            cm.SetText("Current Val: " + (this.beatNum + this.beatDecomposition));
        }
        
        val_text.text = "" + (this.beatNum + this.beatDecomposition);
    }

    public void SetSnap(int dir, int sn, int sni) {
        if (dir == 0) {
            // X
            this.x_snap = sn;
            this.x_snapIndex = sni;
            cm.SetXText("X: " + GetSnapXHelper(this.x_snap));
            cm.SelectXSnapIndex(x_snapIndex);
        }
        if (dir == 1) {
            // Y
            this.y_snap = sn;
            this.y_snapIndex = sni;
            cm.SetYText("Y: " + GetSnapYHelper(this.y_snap));
            cm.SelectYSnapIndex(y_snapIndex);
        }
    }

    public string GetSnapString(int i) {
        if (i == 0) {
            return GetSnapXHelper(x_snap);
        }
        return GetSnapYHelper(y_snap);
    }
    
    public string GetRotationString() {
        switch(rotation) {
            case 0:
                return "None";
            case 1:
                return "Random";
            case 2:
                return "Left";
            case 3:
                return "Right";
        }
        return "None";
    }

    private string GetSnapXHelper(int i) {
        switch(i) {
            case -1:
                return "Left";
            case 0:
                return "Random";
            case 1:
                return "Right";
            case 2:
                return "Midpoint";
        }
        return "Random";
    }
    private string GetSnapYHelper(int i) {
        switch(i) {
            case -1:
                return "Down";
            case 0:
                return "Random";
            case 1:
                return "Up";
            case 2:
                return "Midpoint";
        }
        return "Random";
    }

    public void SetLength(float l) {
        this.length = l;
        if (this.length != -1) {
            float in_world_length = this.length * beatValue;
            has_length = true;
            ResetLength(lengthObject);
            Resize(lengthObject, in_world_length, new Vector3(1, 0, 0), new Vector3(1, 0, 0));
        } else {
            has_length = false;
        }
    }

    public void SetRotation(int rot) {
        this.rotation = rot;
        cme.SetRotText("Rot: " + GetRotationString());
        cme.SelectRotIndex(this.rotation);
    }

    public void SetTunnelProperty(int id, bool value) {
        switch(id) {
            case 0:
                this.tunnel_is_rand = value;
                break;
            case 1:
                this.tunnel_is_rainbow = value;
                break;
        }
    }

    public void SetPrefab(GameObject toSpawn) {
        // Destroy current note
        if (note != null) {
            Destroy(note);
        }
        
        note = Instantiate(toSpawn);
        note.transform.SetParent(this.transform);
        note.transform.localPosition = new Vector3(0, 0, 0);
        // Disable the collider on the prefab
        note.GetComponent<Collider>().enabled = false;
    }

    public void SetPrefabWithId(GameObject toSpawn, string id) {
        this.type = id;
        SetPrefab(toSpawn);
    }

    public void SetupNote(GameObject toSpawn, string type, float num) {
        SetPrefab(toSpawn);
        this.type = type;
        this.beatNum = num;
        this.length = -1;
        this.has_length = false;
        this.beatDecompIndex = 0;
        this.val_text.text = "" + beatNum;
        SetupRefs();
        this.cm.SetupNote(this);
    }

    public void SetupNote(GameObject toSpawn, string type, float num, LevelNoteSerializable note) {
        SetPrefab(toSpawn);
        this.type = type;
        this.beatNum = FixBeatNum(note.beatNum);
        this.length = note.length;
        this.has_length = note.has_length;
        SetLength(this.length);
        this.beatDecomposition = FixDecomp(note.beatDecomposition);
        this.beatDecompIndex = note.beatDecompIndex;
        this.x_snap = note.x_snap;
        this.x_snapIndex = note.x_snapIndex;
        this.y_snap = note.y_snap;
        this.y_snapIndex = note.y_snapIndex;
        this.val_text.text = "" + (beatNum + beatDecomposition);
    }

    public void SetupEvent(GameObject toSpawn, string type, float num) {
        SetPrefab(toSpawn);
        this.type = type;
        this.beatNum = num;
        this.beatDecompIndex = 0;
        this.rotation = 0;
        this.tunnel_is_rainbow = false;
        this.tunnel_is_rand = false;
        this.isEvent = true;
        this.val_text.text = "" + beatNum;
        SetupRefs();
        this.cme.SetupEvent(this);
    }

    public void SetupEvent(GameObject toSpawn, string type, float num, LevelEventSerializable my_event) {
        SetPrefab(toSpawn);
        this.type = type;
        this.beatNum = FixBeatNum(my_event.beatNum);
        this.beatDecomposition = FixDecomp(my_event.beatDecomposition);
        this.beatDecompIndex = my_event.beatDecompIndex;
        this.rotation = my_event.rotation;
        this.tunnel_is_rand = my_event.corroded;
        this.tunnel_is_rainbow = my_event.rainbow;
        this.isEvent = true;
        this.val_text.text = "" + (beatNum + beatDecomposition);
    }

    public float FixBeatNum(float newBeatNum) {
        return Mathf.Round(newBeatNum);
    }

    public float FixDecomp(float decomp) {
        if(decomp > .24f && decomp < .26f) {
            return .25f;
        }
        if(decomp > .32f && decomp < .34f) {
            return .33f;
        }
        if(decomp > .49f && decomp < .51f) {
            return .5f;
        }
        if(decomp > .66f && decomp < .68f) {
            return .67f;
        }
        if(decomp > .74f && decomp < .76f) {
            return .75f;
        }

        return decomp;
    }

    public void Click() {
        if (isEvent) {
            cme.SetupEvent(this);
        } else {
            cm.SetupNote(this);
        }
    }

    public void Resize(GameObject obj, float amount, Vector3 moveDirection, Vector3 direction)
    {
        obj.transform.position += moveDirection * amount / 2; // Move the object in the direction of scaling, so that the corner on ther side stays in place
        obj.transform.localScale += direction * amount; // Scale object in the specified direction
    }
    
    private void ResetLength(GameObject obj) {
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localScale = new Vector3(0, 0.1f, 0.1f);
    }

    public LevelNoteSerializable Serialize() {
        return new LevelNoteSerializable((this.beatNum + this.beatDecomposition), this.transform.position.y, this.beatNum, this.beatDecomposition, this.beatDecompIndex, this.length, this.has_length, this.type, this.x_snap, this.x_snapIndex, this.y_snap, this.y_snapIndex);
    }

    public LevelEventSerializable SerializeEvent() {
        return new LevelEventSerializable((this.beatNum + this.beatDecomposition), this.transform.position.y, this.beatNum, this.beatDecomposition, this.beatDecompIndex, this.type, this.rotation, this.tunnel_is_rand, this.tunnel_is_rainbow);
    }

    public void Delete() {
        staff.Delete(this.gameObject);
        Destroy(this.gameObject);
    }
}
