using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteTunnelButton : MonoBehaviour
{
    [SerializeField]
    public Image img;
    [SerializeField]
    EditorNote editorNote;
    [SerializeField]
    int id;
    [SerializeField]
    bool is_selected;
    [SerializeField]
    Sprite selected;
    [SerializeField]

    Sprite unselected;

    public void SetEditorNote(EditorNote ed) {
        editorNote = ed;
    }


    public void SetTunnelProperty() {
        editorNote.SetTunnelProperty(id, !is_selected);
        ChooseSelect(!is_selected);
    }

    public void ChooseSelect(bool val) {
        if (val) {
            this.is_selected = true;
            this.Select();
        }
        else {
            this.is_selected = false;
            this.Unselect();
        }
    }

    public void Select() {
        img.sprite = selected;

    }

    public void Unselect() {
        img.sprite = unselected;
    }
}
