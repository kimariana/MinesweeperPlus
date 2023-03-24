using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DifficultyButton : MonoBehaviour
{
    [SerializeField]
    public string difficulty = string.Empty;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Loads game on easy mode
        if(difficulty == "Easy") {
            // Set variables for easy difficulty
            Difficulty.difficulty = "Easy";
            Minesweeper.sMineCount = 5;
            Minesweeper.width = 8;
            Minesweeper.height = 8;
            SceneManager.LoadScene("Minesweeper");
        // Loads game on normal mode
        } else if(difficulty == "Normal") {
            // Set variables for normal difficulty
            Difficulty.difficulty = "Normal";
            Minesweeper.sMineCount = 29;
            Minesweeper.width = 16;
            Minesweeper.height = 16;
            SceneManager.LoadScene("Minesweeper");
        // Loads game on hard mode
        } else if(difficulty == "Hard") {
            // Set variables for hard difficulty
            Difficulty.difficulty = "Hard";
            Minesweeper.sMineCount = 45;
            Minesweeper.width = 20;
            Minesweeper.height = 16;
            SceneManager.LoadScene("Minesweeper");
        }
    }
}
