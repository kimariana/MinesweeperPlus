using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HowToPlay : MonoBehaviour
{
    static public TextMeshProUGUI uiText;

    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        uiText.enabled = false; // Hides how to play text at the start
    }
}
