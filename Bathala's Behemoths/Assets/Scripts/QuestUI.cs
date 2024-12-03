using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public TextMeshProUGUI QuestText;
    public int QuestItemCount;

    // Start is called before the first frame update
    void Start()
    {
        QuestText = GetComponent<TextMeshProUGUI>();
        DontDestroyOnLoad(gameObject); // allows it to persist btw scenes
        QuestItemCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestItemCount == 3)
        {
            QuestText.text = "Markupo Investigation Clues Found: Complete!";
        }
        else
        {
            QuestText.text = "Markupo Investigation Clues Found: " + QuestItemCount + "/3";
        }
    }
}
