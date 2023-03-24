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
        if(instructions == "HowToPlay") {
            HowToPlay.uiText.enabled = !HowToPlay.uiText.enabled;
            Controls.uiText.enabled = false;
        } else if(instructions == "Controls") {
            Controls.uiText.enabled = !Controls.uiText.enabled;
            HowToPlay.uiText.enabled = false;
        }
    }
}
