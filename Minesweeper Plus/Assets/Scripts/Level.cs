using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level : MonoBehaviour
{
    private TextMeshProUGUI uiText;
    static public int level = 0;
    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        uiText.text = "Level: " + level.ToString("#,0"); // Displays level
    }
}
