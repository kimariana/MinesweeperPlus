using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalScore : MonoBehaviour
{
    private TextMeshProUGUI uiText;

    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        uiText.text = "Score: " + Minesweeper.finalScore.ToString("#,0"); // Displays score on gameover screen
    }
}
