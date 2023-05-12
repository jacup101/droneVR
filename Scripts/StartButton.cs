using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    // Controls the Start Button in the Main Menu UI
    // UI Elements
    public TMPro.TMP_Dropdown dropdown;
    public GameObject canvas;
    public GameObject raycast;
    // System references
    public ResetSystem reset;
    public RhythmSystem system;
    public MotionControls mc;

    // Game Objects To Set Active
    public GameObject[] UIObjs;


    // Start is called before the first frame update
    void Start()
    {
        // Conditional compilation, remove the UI if run in Unity Editor, since the UI can not be interacted with outside of VR
        #if UNITY_EDITOR
        canvas.SetActive(false);
        raycast.SetActive(false);
        SetActive();
        #endif
    }
    // Upon pressing the button, start the game
    public void OnPressed() {
        reset.ResetGame(dropdown.options[dropdown.value].text);
        mc.Unpause();
        canvas.SetActive(false);
        raycast.SetActive(false);
        SetActive();
    }

    void SetActive() {
        foreach(GameObject uiObj in UIObjs) {
            uiObj.SetActive(true);
        }
    }
}
