using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsTutorial : MonoBehaviour
{
    public GameObject dialogue;
    
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        isInTrigger = false;

        if(QuestState.Instance.skillsTutorialTrigger == true)
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
        if (other.CompareTag("Player") && QuestState.Instance.skillsTutorialTrigger == false)
        {
            dialogue.SetActive(true);
            QuestState.Instance.skillsTutorialTrigger = true;
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            QuestState.Instance.skillsTutorialTrigger = true;
            // popUp.gameObject.SetActive(false);
        }
    }
}