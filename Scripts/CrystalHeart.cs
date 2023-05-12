using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalHeart : MonoBehaviour
{
    // Crystal Heart implements the in game behaviour for the collection of crystal hearts

    [SerializeField]
    private GameObject crystal_heart_icon;
    [SerializeField]
    private GameObject crystal_heart_text_tens;
    [SerializeField]
    private GameObject crystal_heart_text_ones;
    [SerializeField]
    private Sprite[] number_sprites;
    [SerializeField]
    private int crystal_heart_num = 0;

    public void AddCrystal() {
        crystal_heart_num ++;
        Image tens = crystal_heart_text_tens.GetComponent<Image>();
        Image ones = crystal_heart_text_ones.GetComponent<Image>();

        int ones_index = crystal_heart_num % 10;
        int tens_index = (crystal_heart_num / 10 ) % 10;

        tens.sprite = number_sprites[tens_index];
        ones.sprite = number_sprites[ones_index];
    }

    public void Reset() {
        crystal_heart_num = 0;
        Image tens = crystal_heart_text_tens.GetComponent<Image>();
        Image ones = crystal_heart_text_ones.GetComponent<Image>();

        int ones_index = crystal_heart_num % 10;
        int tens_index = (crystal_heart_num / 10 ) % 10;

        tens.sprite = number_sprites[tens_index];
        ones.sprite = number_sprites[ones_index];

    }


    // TODO: Add function to obtain crystal heart num for persistence in levels
    public int EndGame() {
        // TODO: Add additional functionality here
        return crystal_heart_num;
    }
}
