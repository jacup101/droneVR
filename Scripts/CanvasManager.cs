using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    List<NoteValueButton> beat_buttons;
    [SerializeField]
    List<NoteSnapButton> x_snap_buttons;
    [SerializeField]
    List<NoteSnapButton> y_snap_buttons;
    [SerializeField]
    NoteTunnelButton rand;
    [SerializeField]
    NoteTunnelButton rainbow;
    [SerializeField]
    List<NoteRotationButton> rot_buttons;
    [SerializeField] 
    List<Image> normal_sprites;
    [SerializeField]
    List<Image> selected_sprites;




    [SerializeField]
    NoteTypeSelection selection;
    [SerializeField]
    NoteDeleteButton deletion;
    [SerializeField]
    NoteLengthText length_input;
    [SerializeField]
    NoteValueText val_text_input;
    [SerializeField]
    TMP_Text beat_val;

    [SerializeField]
    TMP_Text x_snap_val;
    [SerializeField]
    TMP_Text y_snap_val;
    [SerializeField]
    TMP_Text rot_text;

    public void SetupNote(EditorNote ed) {
        SetupBeats(ed);
        SetupType(ed);
        SetupLength(ed);
        SetupValue(ed);
        SetupSnaps(ed);
        SetupDelete(ed);
    }

    public void SetupEvent(EditorNote ed) {
        SetupBeats(ed);
        SetupType(ed);
        SetupValue(ed);
        SetupDelete(ed);
        SetupTunnelRotsAndType(ed);
    }   

    public void SetupDelete(EditorNote ed) {
        deletion.SetEditorNote(ed);
    }

    public void SetupBeats(EditorNote ed) {
        foreach(NoteValueButton button in beat_buttons) {
            button.SetEditorNote(ed);
        }
        SetText("Current Val: " + (ed.beatNum + ed.beatDecomposition));
        SelectIndex(ed.beatDecompIndex);
    }

    public void SetupSnaps(EditorNote ed) {
        foreach(NoteSnapButton button in x_snap_buttons) {
            button.SetEditorNote(ed);
        }

        foreach(NoteSnapButton button in y_snap_buttons) {
            button.SetEditorNote(ed);
        }
        SetXText("X: " + ed.GetSnapString(0));
        SetYText("Y: " + ed.GetSnapString(1));

        SelectXSnapIndex(ed.x_snapIndex);
        SelectYSnapIndex(ed.y_snapIndex);
    }

    public void SetupTunnelRotsAndType(EditorNote ed) {
        foreach(NoteRotationButton button in rot_buttons) {
            button.SetEditorNote(ed);
        }
        rand.SetEditorNote(ed);
        rand.ChooseSelect(ed.tunnel_is_rand);

        rainbow.SetEditorNote(ed);
        rainbow.ChooseSelect(ed.tunnel_is_rainbow);

        SetRotText("Rot: " + ed.GetRotationString());
        SelectRotIndex(ed.rotation);
    }

    public void SetupType(EditorNote ed) {
        selection.SetEditorNote(ed);
        selection.SetDropdownType(ed.type);
    }

    public void SetupLength(EditorNote ed) {
        length_input.SetEditorNote(ed);
        length_input.SetLengthVal(ed.length);
    }

    public void SetupValue(EditorNote ed) {
        val_text_input.SetEditorNote(ed);
        val_text_input.SetBeatVal(ed.beatNum);
    }

    public void SelectIndex(int index) {
        ResetButtons();
        beat_buttons[index].Select();
    }

    public void SelectRotIndex(int index) {
        ResetRotButtons();
        rot_buttons[index].Select();
    }

    public void SelectXSnapIndex(int index) {
        ResetXButtons();
        x_snap_buttons[index].Select();
    }

    public void SelectYSnapIndex(int index) {
        ResetYButtons();
        y_snap_buttons[index].Select();
    }

    public void ResetRotButtons() {
        for(int i = 0; i < rot_buttons.Count; i++) {
            rot_buttons[i].Unselect();
        }
    }


    public void ResetXButtons() {
        for(int i = 0; i < x_snap_buttons.Count; i++) {
            x_snap_buttons[i].Unselect();
        }
    }

    public void ResetYButtons() {
        for(int i = 0; i < y_snap_buttons.Count; i++) {
            y_snap_buttons[i].Unselect();
        }
    }

    public void ResetButtons() {
        for(int i = 0; i < beat_buttons.Count; i++) {
            beat_buttons[i].Unselect();
        }
    }

    public void SetText(string text) {
        beat_val.text = text;
    }

    public void SetXText(string text) {
        x_snap_val.text = text;
    }
    public void SetRotText(string text) {
        rot_text.text = text;
    }
    public void SetYText(string text) {
        y_snap_val.text = text;
    }
}
