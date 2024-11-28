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
        QuestItemCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        QuestText.text = "Markupo Investigation Clues Found: " + QuestItemCount + "/3";
    }

    public void EnableQuestUI()
    {
        gameObject.SetActive(true);
    }

    public void DisableQuestUI()
    {
        gameObject.SetActive(false);
    }
}
