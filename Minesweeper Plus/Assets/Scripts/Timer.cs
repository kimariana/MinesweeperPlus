using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    static public TextMeshProUGUI uiText;
    static public float timeCountdown = 59.0f;

    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Minesweeper.start && !Minesweeper.gameover) { // Updates time DURING the level
            timeCountdown -= Time.deltaTime;

            uiText.text = GetTime(); // Displays time
        }
    }

    static public string GetTime()
    {
        float minutes = Mathf.Floor(timeCountdown / 60);
        float seconds = timeCountdown % 60;
        if(minutes <= 0 && seconds <= 0) { // Checks for end of game
            minutes = 0;
            seconds = 0;
            Minesweeper.gameover = true;
            ScoreCounter.uiText.text = "Score: " + ScoreCounter.score.ToString("#,0"); // Displays final score
            Minesweeper.end = true;
        }

        return minutes.ToString("00") + ":" + seconds.ToString("00"); // Displays time
    }
}