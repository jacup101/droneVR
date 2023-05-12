using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteLengthText : MonoBehaviour
{
    [SerializeField]
    EditorNote editorNote;
    [SerializeField]
    string input_msg;
    [SerializeField]
    float input_normalized;
    [SerializeField]
    TMP_InputField input;


    public void SetEditorNote(EditorNote ed) {
        editorNote = ed;
    }

    public void InputChanged(string text) {
        input_msg = text;
        try {
            input_normalized =  float.Parse(text);
        }
        catch {
            Debug.LogWarning("Bad input for size, defaulting to -1");
            input_normalized = -1;
        }
        SetNoteValue();
    }


    public void SetNoteValue() 
    {
        if (editorNote == null) {
            return;
        }
        editorNote.SetLength(input_normalized);
    }

    public void SetLengthVal(float length) {
        if (length == -1) {
            input.text = "Instant";
        }
        else {
            input.text = "" + length;
        }
    }
}
