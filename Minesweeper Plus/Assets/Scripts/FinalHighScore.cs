using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalHighScore : MonoBehaviour
{
    private TextMeshProUGUI uiText;
    private int highScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        if(PlayerPrefs.HasKey("HighScore")) { // If the PlayerPrefs HighScore already exists, read it
            highScore = PlayerPrefs.GetInt("HighScore");
        }
        uiText.text = "High Score: " + highScore.ToString("#,0"); // Displays high score on gameover screen
    }
}
