using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteRotationButton : MonoBehaviour
{
    [SerializeField]
    public Image img;
    [SerializeField]
    EditorNote editorNote;
    [SerializeField]
    int id;
    [SerializeField]
    Sprite selected;
    [SerializeField]

    Sprite unselected;

    public void SetEditorNote(EditorNote ed) {
        editorNote = ed;
    }


    public void SetRotationValue() {
        editorNote.SetRotation(this.id);
    }

    public void Select() {
        img.sprite = selected;
    }

    public void Unselect() {
        img.sprite = unselected;
    }
}
