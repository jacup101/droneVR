using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteValueText : MonoBehaviour
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
            int truncate = (int) input_normalized;
            input_normalized = (float) truncate;
        }
        catch {
            Debug.LogWarning("Bad input for value, defaulting to -1");
            input_normalized = -1;
        }
        SetNoteValue();
    }


    public void SetNoteValue() 
    {
        if (editorNote == null) {
            return;
        }
        editorNote.SetBeatValue(input_normalized);
    }

    public void SetBeatVal(float num) {
        input.text = "" + num;
    }
}
