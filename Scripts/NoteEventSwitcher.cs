using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEventSwitcher : MonoBehaviour
{
    [SerializeField] public Staff note_staff;
    [SerializeField] public Staff event_staff;
    [SerializeField] public CanvasManager note_manager;
    [SerializeField] public CanvasManager event_manager;
    [SerializeField] public GameObject note_staff_obj;
    [SerializeField] public GameObject event_staff_obj;
    [SerializeField] public List<GameObject> note_objs;
    [SerializeField] public List<GameObject> event_objs;

    [SerializeField] public bool using_events = false;

    int cooldown = 0;


    void Update() {
        if (OVRInput.Get(OVRInput.Button.One) && cooldown == 0){
            cooldown = 200;
            if (using_events) {
                SetActive(note_objs, true);
                SetActive(event_objs, false);
                using_events = false;
            }
            else {
                SetActive(note_objs, false);
                SetActive(event_objs, true);
                using_events = true;
            }
        }

        if (cooldown > 0) cooldown--;
    }

    private void SetActive(List<GameObject> objs, bool val) {
        foreach(GameObject gobj in objs) {
            gobj.SetActive(val);
        }

    }
}
