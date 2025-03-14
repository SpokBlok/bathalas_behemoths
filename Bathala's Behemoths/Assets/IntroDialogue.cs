using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textInterval;
    public GameObject pointer;
    public GameObject introScene;
    public GameObject introScene2;
    public GameObject player;
    public SteveAnimController steveModel;
    public MSAnimController mountedModel;
    public GameObject HUD;


    private bool pointerActive = false;
    Vector3 currentPosition;
    Vector3 originalHUDPos;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        steveModel = FindAnyObjectByType<SteveAnimController>(FindObjectsInactive.Include);
        mountedModel = FindAnyObjectByType<MSAnimController>(FindObjectsInactive.Include);
        player = GameObject.FindWithTag("Player");
        
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
            PlayerStats.Instance.introDone = true;
            QuestState.Instance.pausedForDialogue = false;
            index = 0;

            introScene.SetActive(false);
            introScene2.SetActive(true);
            switchToManny();
            PlayerStats.Instance.initSpeed = 5;

            player.gameObject.transform.position = new Vector3(735.2f, 60.7f, 280.3f);
            HUD.gameObject.transform.position = originalHUDPos;
            
            Debug.Log("inside end state");
        }
        else
        {
            gameObject.transform.localPosition = new Vector3 (1000, 1000);
            pointer.SetActive(true);
            pointerActive = true;
        }
    }

    public void switchToManny()
    {
        steveModel.gameObject.SetActive(false);
        mountedModel.gameObject.SetActive(true);
    }
}
