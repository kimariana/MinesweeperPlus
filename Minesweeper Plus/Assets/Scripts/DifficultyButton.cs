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
        if(difficulty == "Easy") {
            Difficulty.difficulty = "Easy";
            Minesweeper.sMineCount = 5;
            Minesweeper.width = 8;
            Minesweeper.height = 8;
            SceneManager.LoadScene("Minesweeper");
        } else if(difficulty == "Normal") {
            Difficulty.difficulty = "Normal";
            Minesweeper.sMineCount = 29;
            Minesweeper.width = 16;
            Minesweeper.height = 16;
            SceneManager.LoadScene("Minesweeper");
        } else if(difficulty == "Hard") {
            Difficulty.difficulty = "Hard";
            Minesweeper.sMineCount = 45;
            Minesweeper.width = 20;
            Minesweeper.height = 16;
            SceneManager.LoadScene("Minesweeper");
        }
    }
}
