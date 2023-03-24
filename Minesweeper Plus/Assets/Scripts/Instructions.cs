using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Instructions : MonoBehaviour
{
    [SerializeField]
    public string instructions = string.Empty;
    private Button button;
    
    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if(instructions == "HowToPlay") { // HowToPlay button is pressed
            HowToPlay.uiText.enabled = !HowToPlay.uiText.enabled; // If displayed, hide text; if hidden, show text
            Controls.uiText.enabled = false; // Hides controls text
        } else if(instructions == "Controls") { // Controls button is pressed
            Controls.uiText.enabled = !Controls.uiText.enabled; // If displayed, hide text; if hidden, show text
            HowToPlay.uiText.enabled = false; // Hides how to play text
        }
    }
}
