using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DwendeSightingDia : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textInterval;
    public GameObject pointer;

    private bool pointerActive = false;
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
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
        Debug.Log("Index: " + index);
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textInterval);
        }
    }

    void NextLine()
    {
        if(index < lines.Length - 1)
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
            QuestState.Instance.desponDwendeSightTrigger = true;
            QuestState.Instance.pausedForDialogue = false;
            index = 0;
            
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
