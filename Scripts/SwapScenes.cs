using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SwapScenes : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown dropdown;
    [SerializeField]
    TMP_InputField input;

    public void LoadGame() {
        PlayerPrefs.SetString("custom_level", input.text + " C");
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("game");    
    }

    public void LoadEditor() {
        string level = dropdown.options[dropdown.value].text;
        PlayerPrefs.SetString("editor_level", level);
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync ("editor");
    }
}
