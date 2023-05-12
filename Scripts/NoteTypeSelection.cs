using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NoteTypeSelection : MonoBehaviour
{
    [SerializeField]
    EditorNote editorNote;
    [SerializeField]
    List<GameObject> prefabs;

    [SerializeField]
    List<string> ids;
    [SerializeField]
    TMP_Dropdown dropdown;


    public void SetNoteType(int index) {
        if (editorNote == null) {
            return;
        }
        if (index >= ids.Count) {
            index = 0;
        }
        editorNote.SetPrefabWithId(prefabs[index], ids[index]);
    }
    public void SetEditorNote(EditorNote ed) {
        editorNote = ed;
    }
    public void SetDropdownType(string id) {
        int index = ids.FindIndex(new IdSearch(id).Equals);
        dropdown.value = index;

    }
}

public class IdSearch
{
   string _s;

   public IdSearch(string s)
   {
      _s = s;
   }

   public bool Equals(string e)
   {
      return _s.Equals(e);
   }
}
