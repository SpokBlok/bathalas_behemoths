using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TammyTamedDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] currentLines;
    public float textInterval;
    public GameObject pointer;
    public GameObject HUD;

    private bool pointerActive = false;
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
            QuestState.Instance.pausedForDialogue = false;
            HUD.gameObject.transform.position = originalHUDPos;
            SceneManager.LoadScene("MainMenu");
            
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
