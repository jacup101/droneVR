using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteDeleteButton : MonoBehaviour
{

    [SerializeField]
    EditorNote editorNote;

    public void SetEditorNote(EditorNote ed) {
        editorNote = ed;
    }

    public void Delete() {
        editorNote.Delete();
    }
}
