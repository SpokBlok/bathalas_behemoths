using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public string[] linesRepeat;
    public string[] currentLines;
    public float textInterval;
    public GameObject pointer;
    public GameObject HUD;

    private bool pointerActive = false;
    Vector3 currentPosition;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        QuestState.Instance.frightenedDwendeRepeat = false;
        currentPosition = gameObject.transform.localPosition;
    }

    void OnEnable()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD");
        HUD.SetActive(false);
        QuestState.Instance.pausedForDialogue = true;
        if(QuestState.Instance.frightenedDwendeRepeat)
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
        else if(pointerActive == true)
        {
            gameObject.transform.localPosition = currentPosition;
            gameObject.SetActive(false);
            pointer.SetActive(false);
            pointerActive = false;
            QuestState.Instance.frightenedDwendeRepeat = true;
            QuestState.Instance.pausedForDialogue = false;
            HUD.SetActive(true);
            
            Debug.Log("inside end state");
        }
        else
        {
            gameObject.transform.localPosition = new Vector3 (1000, 1000);
            pointer.SetActive(true);
            pointerActive = true;
        }
    }
}
