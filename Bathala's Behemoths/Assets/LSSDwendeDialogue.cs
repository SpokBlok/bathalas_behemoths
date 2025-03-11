using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LSSDwendeDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public string[] linesRepeat;
    public string[] linesRepeat2;
    public string[] currentLines;
    public float textInterval;
    public GameObject pointer;
    public GameObject clueImage;
    public GameObject LSSDwende;

    private bool imageActive = false;
    Vector3 currentPosition;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        currentPosition = gameObject.transform.localPosition;
    }

    void OnEnable()
    {
        QuestState.Instance.pausedForDialogue = true;
        if(QuestState.Instance.lssDwendeRepeat && QuestState.Instance.fluteGet)
        {
            QuestState.Instance.lssDwendeRan = true;
            currentLines = linesRepeat;
        }
        else if(QuestState.Instance.lssDwendeRepeat)
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

            if(QuestState.Instance.lssDwendeRepeat && QuestState.Instance.fluteGet)
            {
                LSSDwende.SetActive(false);
            }
            
            Debug.Log("inside end state");
        }
        else
        {
            gameObject.transform.localPosition = new Vector3 (1000, 1000);
            if(QuestState.Instance.lssDwendeRepeat && QuestState.Instance.fluteGet)
            {
                PlayerStats.Instance.clue6 = true;
                clueImage.SetActive(true);
            }
            else
            {
                pointer.SetActive(true);
            }
            imageActive = true;
            QuestState.Instance.lssDwendeRepeat = true;
        }
    }
}
