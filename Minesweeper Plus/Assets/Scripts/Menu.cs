using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Menu : MonoBehaviour
{
    [SerializeField]
    public string action = string.Empty;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if(action == "Restart") {
            ScoreCounter.score = 0;
            Timer.timeCountdown = 59;
            Level.level = 0;
            SceneManager.LoadScene("Minesweeper");
        } else if(action == "Exit") {
            ScoreCounter.score = 0;
            Timer.timeCountdown = 59;
            Level.level = 0;
            SceneManager.LoadScene("Start");
        }
    }
}
