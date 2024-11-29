using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillQuestUI : MonoBehaviour
{
    public TextMeshProUGUI QuestText;
    public int KillQuestCount;

    // Start is called before the first frame update
    void Start()
    {
        QuestText = GetComponent<TextMeshProUGUI>();
        KillQuestCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (KillQuestCount >= 5)
        {
            QuestText.text = "Spiritual Energy Collected: Complete!";
        }
        else
        {
            QuestText.text = "Spiritual Energy Collected: " + KillQuestCount + "/5";
        }
    }
}
