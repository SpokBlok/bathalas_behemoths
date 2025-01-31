using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class QuestTutorial : MonoBehaviour
{
    public GameObject dialogue;
    
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        isInTrigger = false;

        if(QuestState.Instance.questTutorialTrigger == true)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && QuestState.Instance.questTutorialTrigger == false)
        {
            dialogue.SetActive(true);
            QuestState.Instance.questTutorialTrigger = true;
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            QuestState.Instance.questTutorialTrigger = true;
            // popUp.gameObject.SetActive(false);
        }
    }
}
