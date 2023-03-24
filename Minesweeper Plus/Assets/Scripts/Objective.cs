using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Objective : MonoBehaviour
{
    private TextMeshProUGUI uiText;
    
    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Displays number of safe squares clicked and objective number
        uiText.text = Minesweeper.counter.ToString("#,0") + "/" + Minesweeper.objective.ToString("#,0");
    }
}
