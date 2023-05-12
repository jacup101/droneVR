using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableNoteLength : MonoBehaviour
{

    [SerializeField]
    private float beat_length = 1;
    [SerializeField]
    private float shape_length = 1;
    [SerializeField]
    private GameObject[] components;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Length" + length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Setup(float z_start, float z_end, float beats, float len) {
        this.beat_length = len;
        float dist = z_start - z_end;
        float one_beat_dist = dist / beats;
        this.shape_length = beat_length * one_beat_dist;
        this.Resize_All();
    }
    public void SetLength(float len) {
        this.beat_length = len;
        // Debug.Log("Length set to: " + beat_length);
    }
    public void Resize_All() {
        foreach(GameObject component in components) {
            Resize(component, shape_length, new Vector3(0, 0, 1));
        }
    }
    public void Resize(GameObject obj, float amount, Vector3 direction)
    {
        obj.transform.position += direction * amount / 2; // Move the object in the direction of scaling, so that the corner on ther side stays in place
        obj.transform.localScale += direction * amount; // Scale object in the specified direction
    }

    public float GetInWorldLength() {
        return shape_length;
    }
}
