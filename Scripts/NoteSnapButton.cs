using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteSnapButton : MonoBehaviour
{
    [SerializeField]
    public Image img;
    [SerializeField]
    EditorNote editorNote;
    [SerializeField]
    int id;
    [SerializeField]
    int snapValue;

    [SerializeField]
    int snapId;
    [SerializeField]
    Sprite selected;
    [SerializeField]

    Sprite unselected;

    public void SetEditorNote(EditorNote ed) {
        editorNote = ed;
    }


    public void SetSnapValue() {
        editorNote.SetSnap(id, snapValue, snapId);
    }

    public void Select() {
        img.sprite = selected;
    }

    public void Unselect() {
        img.sprite = unselected;
    }
}
