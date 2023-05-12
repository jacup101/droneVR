using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{


    [SerializeField] const int beatValue = 1;
    [SerializeField] const float staffOffset = 0.2f;

    [SerializeField] int timeSigCount;
    [SerializeField] int timeSigValue;

    [SerializeField] int numBars;
    


    [SerializeField] GameObject[] staffLines;
    [SerializeField] GameObject barPrefab;

    [SerializeField] List<GameObject> bars;

    [SerializeField] List<GameObject> notes = new List<GameObject>();
    [SerializeField] GameObject editorNotePrefab;

    [SerializeField] GameObject barsHolder;
    [SerializeField] GameObject notesHolder;
    [SerializeField] bool isEventStaff;


    // Start is called before the first frame update
    void Start()
    {
        GenerateNewStaff();
        bars = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateNewStaff() {
        SetStaffLineSizes();
        AddBars(staffLines[0].transform.position.z, transform.position.y, transform.position.z);
        if (isEventStaff) {
            this.gameObject.SetActive(false);
        }
    }

    void SetStaffLineSizes() {
        float finalLength = beatValue * timeSigCount * numBars;
        foreach(GameObject obj in staffLines) {
            obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, 0);
            Resize(obj, finalLength, new Vector3(1, 0, 0), new Vector3(0, 0, 1));
        }
    }

    void AddBars(float x, float y, float z) {
        for(float i = x; i <= beatValue * timeSigCount * numBars + x; i += timeSigCount * beatValue) {
            Vector3 position = new Vector3(i, y, z);
            GameObject createdBar = Instantiate(barPrefab,position, Quaternion.identity, barsHolder.transform);
            bars.Add(createdBar);
            createdBar.transform.SetParent(barsHolder.transform);
        }
    }

    public void Resize(GameObject obj, float amount, Vector3 moveDirection, Vector3 direction)
    {
        obj.transform.position += moveDirection * amount / 2; // Move the object in the direction of scaling, so that the corner on ther side stays in place
        obj.transform.localScale += direction * amount; // Scale object in the specified direction
    }

    public void InstantiateNote(GameObject prefab, string type, Vector3 position) {
        // Assuming staff is at a negative position (it wouldn't really ever be positive but we will take that into account)
        float x_on_staff = position.x - this.transform.position.x;
        
        x_on_staff = Mathf.Floor(x_on_staff) + staffOffset;

        float y_on_staff = SelectY(position.y);
        Vector3 new_position = new Vector3(x_on_staff, y_on_staff, this.transform.position.z);
        Vector3 new_position_swapped = new Vector3(this.transform.position.z, y_on_staff, x_on_staff);

        GameObject createdNote = Instantiate(editorNotePrefab, new_position, Quaternion.identity, notesHolder.transform);
        notes.Add(createdNote);
        createdNote.transform.SetParent(notesHolder.transform);
        createdNote.transform.localPosition = new_position_swapped;
        createdNote.GetComponent<EditorNote>().SetupNote(prefab, type, x_on_staff - staffOffset);
    }

    public void InstantiateEvent(GameObject prefab, string type, Vector3 position) {
        // Assuming staff is at a negative position (it wouldn't really ever be positive but we will take that into account)
        float x_on_staff = position.x - this.transform.position.x;
        
        x_on_staff = Mathf.Floor(x_on_staff) + staffOffset;

        float y_on_staff = SelectY(position.y);
        Vector3 new_position = new Vector3(x_on_staff, y_on_staff, this.transform.position.z);
        Vector3 new_position_swapped = new Vector3(this.transform.position.z, y_on_staff, x_on_staff);

        GameObject createdNote = Instantiate(editorNotePrefab, new_position, Quaternion.identity, notesHolder.transform);
        notes.Add(createdNote);
        createdNote.transform.SetParent(notesHolder.transform);
        createdNote.transform.localPosition = new_position_swapped;
        createdNote.GetComponent<EditorNote>().SetupEvent(prefab, type, x_on_staff - staffOffset);
    }

    public void InstantiateNote(GameObject prefab, string type, Vector3 position, LevelNoteSerializable note) {
        // Assuming staff is at a negative position (it wouldn't really ever be positive but we will take that into account)


        Vector3 new_position = new Vector3(position.x + staffOffset, position.y, this.transform.position.z);
        Vector3 new_position_swapped = new Vector3(this.transform.position.z, position.y, position.x + staffOffset);

        GameObject createdNote = Instantiate(editorNotePrefab, new_position, Quaternion.identity, notesHolder.transform);
        notes.Add(createdNote);
        createdNote.transform.SetParent(notesHolder.transform);
        createdNote.transform.localPosition = new_position_swapped;
        createdNote.GetComponent<EditorNote>().SetupNote(prefab, type, position.x, note);
    }

    public void InstantiateEvent(GameObject prefab, string type, Vector3 position, LevelEventSerializable evnt) {
        // Assuming staff is at a negative position (it wouldn't really ever be positive but we will take that into account)


        Vector3 new_position = new Vector3(position.x + staffOffset, position.y, this.transform.position.z);
        Vector3 new_position_swapped = new Vector3(this.transform.position.z, position.y, position.x + staffOffset);

        GameObject createdNote = Instantiate(editorNotePrefab, new_position, Quaternion.identity, notesHolder.transform);
        notes.Add(createdNote);
        createdNote.transform.SetParent(notesHolder.transform);
        createdNote.transform.localPosition = new_position_swapped;
        createdNote.GetComponent<EditorNote>().SetupEvent(prefab, type, position.x, evnt);
    }

    private float SelectY(float y_input) {
        // Debug.Log("SelectY: " + y_input);
        if (y_input >= 0.5f) {
            return 0.75f;
        }
        if (y_input < 0.5f && y_input >= 0.0f) {
            return 0.25f;
        }
        if (y_input < 0.00f && y_input >= -0.5f) {
            return -0.25f;
        }
        if (y_input < -0.5f) {
            return -.75f;
        }
        return 0.0f;
    }

    public void MoveStaffX(float amount) {
        this.transform.position += new Vector3(amount, 0, 0);
    }

    public void SetStaffX(float val) {
        this.transform.position = new Vector3(val, this.transform.position.y, this.transform.position.z);
    }

    public List<GameObject> GetEditorNotes() {
        return notes;
    }

    public void Delete(GameObject note) {
        notes.Remove(note);
    }




    //TODO: implement refresh bars to regenerate them upon time sig change
}
