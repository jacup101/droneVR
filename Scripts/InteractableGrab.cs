using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableGrab : MonoBehaviour
{

    bool clicked = false;

    [SerializeField] string id;
    [SerializeField] PrimaryInteractorConfig interactor;
    [SerializeField] GameObject notestaff;
    [SerializeField] GameObject eventstaff;
    [SerializeField] GameObject prefab;
    [SerializeField] Vector3 resetPosition;
    [SerializeField] NoteEventSwitcher nes;

    // Start is called before the first frame update
    void Start()
    {
        interactor = GameObject.Find("PrimaryInteractorConfig").GetComponent<PrimaryInteractorConfig>();
        nes = GameObject.Find("NoteEventSwitcher").GetComponent<NoteEventSwitcher>();
        notestaff = nes.note_staff_obj;
        eventstaff = nes.event_staff_obj;
    }

    // Update is called once per frame
    void Update()
    {
        if (clicked) {
            Vector3 interactor_pos = interactor.GetInteractor().transform.position;
            Vector3 interactor_rot = interactor.GetInteractor().transform.rotation.eulerAngles;
            // print(interactor_rot.y);
            float z_dist = Mathf.Abs(interactor_pos.z - notestaff.transform.position.z);
            
            float x_offset = Mathf.Tan((interactor_rot.y * Mathf.Deg2Rad)) * z_dist;
            float y_offset = Mathf.Tan((interactor_rot.x * Mathf.Deg2Rad)) * z_dist;

            float min_y = -1;
            float max_y = 1;
            float chosen_x = interactor_pos.x + x_offset;
            float chosen_y = interactor_pos.y - y_offset;

            if (chosen_y > max_y) {
                chosen_y = max_y;
            }
            if (chosen_y < min_y) {
                chosen_y = min_y;
            }

            chosen_x = Mathf.Floor(chosen_x * 100) / 100;
            chosen_y = Mathf.Floor(chosen_y * 100) / 100;

            Vector3 position = new Vector3(chosen_x, chosen_y, notestaff.transform.position.z);
            this.gameObject.transform.position = position;
        }
    }

    public void Click() {
        clicked = !clicked;

        if (!clicked) {
            // We have deselected an object
            if (nes.using_events) {
                eventstaff.GetComponent<Staff>().InstantiateEvent(prefab, id, this.transform.position);
            } else {
                notestaff.GetComponent<Staff>().InstantiateNote(prefab, id, this.transform.position);
            }
            this.transform.position = resetPosition;// new Vector3(0, -.5f, -3.5f);
        }
    }
}
