using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTo : MonoBehaviour
{
    [SerializeField]
    Staff note_staff;

    public void JumpToOnChange(string test) {
        try {
            note_staff.SetStaffX(-1 * float.Parse(test));


        } catch {
            Debug.LogWarning("Bad input for jump to, ignoring");
        }
    }  
}
