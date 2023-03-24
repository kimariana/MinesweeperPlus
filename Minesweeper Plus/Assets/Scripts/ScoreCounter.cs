using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    static public TextMeshProUGUI uiText;
    static public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Minesweeper.gameover) {
            uiText.text = "Score: " + score.ToString("#,0");
        }
    }
}