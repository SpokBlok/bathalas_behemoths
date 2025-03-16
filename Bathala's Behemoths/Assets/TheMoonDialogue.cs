using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TheMoonDialogue : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public string[] linesRepeat;
    public string[] linesRepeat2;
    public string[] currentLines;
    public float textInterval;
    public GameObject pointer;
    public GameObject clueImage;
    public GameObject HUD;
    public GameObject endQuestMarker;

    private bool imageActive = false;
    Vector3 currentPosition;
    Vector3 originalHUDPos;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        currentPosition = gameObject.transform.localPosition;
    }

    void OnEnable()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD");
        originalHUDPos = HUD.gameObject.transform.position;
        HUD.gameObject.transform.position = new Vector3(10000, 10000, 10000);
        QuestState.Instance.pausedForDialogue = true;
        if(QuestState.Instance.moonNPCRepeat && QuestState.Instance.moonChunkGet)
        {
            QuestState.Instance.moonQuestEnded = true;
            currentLines = linesRepeat;
        }
        else if(QuestState.Instance.moonNPCRepeat)
        {
            currentLines = linesRepeat2;
        }
        else
        {
            currentLines = lines;
        }
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(textComponent.text == currentLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = currentLines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in currentLines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textInterval);
        }
    }

    void NextLine()
    {
        if(index < currentLines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else if(imageActive == true)
        {
            gameObject.transform.localPosition = currentPosition;
            gameObject.SetActive(false);
            clueImage.SetActive(false);
            pointer.SetActive(false);
            imageActive = false;
            QuestState.Instance.pausedForDialogue = false;
            HUD.gameObject.transform.position = originalHUDPos;
            
            Debug.Log("inside end state");
        }
        else
        {
            gameObject.transform.localPosition = new Vector3 (1000, 1000);
            if(QuestState.Instance.moonNPCRepeat && QuestState.Instance.moonChunkGet)
            {
                PlayerStats.Instance.clue3 = true;
                clueImage.SetActive(true);
                endQuestMarker.SetActive(false);
            }
            else
            {
                pointer.SetActive(true);
            }
            imageActive = true;
            QuestState.Instance.moonNPCRepeat = true;
            QuestState.Instance.moonQuestTrigger = true;
        }
    }
}
