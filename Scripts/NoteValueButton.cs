using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteValueButton : MonoBehaviour
{
    [SerializeField]
    public Image img;
    [SerializeField]
    EditorNote editorNote;
    [SerializeField]
    float beatValue;

    [SerializeField]
    int beatId;
    [SerializeField]
    Sprite selected;
    [SerializeField]

    Sprite unselected;

    public void SetEditorNote(EditorNote ed) {
        editorNote = ed;
    }


    public void SetNoteValue() {
        editorNote.SetBeat(editorNote.GetBeatNum(), beatValue, beatId);
    }

    public void Select() {
        img.sprite = selected;
    }

    public void Unselect() {
        img.sprite = unselected;
    }
}
