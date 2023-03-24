using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controls : MonoBehaviour
{
    static public TextMeshProUGUI uiText;

    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        uiText.enabled = false; // Hides controls text at the start
    }
}
