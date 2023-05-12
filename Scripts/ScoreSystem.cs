using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    // Crystal Heart implements the in game behaviour for the collection of crystal hearts

    [SerializeField]
    private GameObject score_icon;
    [SerializeField]
    private GameObject score_text_hundreds;
    [SerializeField]
    private GameObject score_text_tens;
    [SerializeField]
    private GameObject score_text_ones;
    [SerializeField]
    private Sprite[] number_sprites;
    [SerializeField]
    private int score = 0;

    public void AddScore(int addAmount) {
        score += addAmount;
        SetScoreUI();
    }

    public void SetScoreUI() {
        Image hundreds = score_text_hundreds.GetComponent<Image>();
        Image tens = score_text_tens.GetComponent<Image>();
        Image ones = score_text_ones.GetComponent<Image>();

        int ones_index = score % 10;
        int tens_index = (score / 10 ) % 10;
        int hundreds_index = (score / 100 ) % 10;

        tens.sprite = number_sprites[tens_index];
        ones.sprite = number_sprites[ones_index];
        hundreds.sprite = number_sprites[hundreds_index];
    }


    // TODO: Add function to obtain crystal heart num for persistence in levels
    public int EndGame() {
        // TODO: Add additional functionality here
        return score;
    }

    public void Reset() {
        score = 0;
        SetScoreUI();
    }
}
