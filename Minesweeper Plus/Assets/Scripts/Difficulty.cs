using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Difficulty : MonoBehaviour
{
    private TextMeshProUGUI uiText;
    static public string difficulty = "Normal"; // Temporary

    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        uiText.text = difficulty; // Difficulty value is updated through buttons
    }
}
