using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DesponDwenDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public string[] linesRepeat;
    public string[] currentLines;
    public float textInterval;
    public GameObject HUD;

    Vector3 currentPosition;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        QuestState.Instance.desponDwendeRepeat = false;
        currentPosition = gameObject.transform.localPosition;
    }

    void OnEnable()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD");
        HUD.SetActive(false);
        QuestState.Instance.pausedForDialogue = true;
        if(QuestState.Instance.desponDwendeRepeat)
        {
            currentLines = linesRepeat;
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
        else
        {
            gameObject.SetActive(false);
            QuestState.Instance.desponDwendeRepeat = true;
            QuestState.Instance.pausedForDialogue = false;
            HUD.SetActive(true);
        }
    }
}
