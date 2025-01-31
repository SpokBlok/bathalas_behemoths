using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class IntroScript : MonoBehaviour
{
    public GameObject dialogue;
    public GameObject introScene;
    public GameObject introScene2;
    
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        isInTrigger = false;
        if(PlayerStats.Instance.introDone)
        {
            introScene.SetActive(false);
            introScene2.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            if(PlayerStats.Instance.introDone == false)
            {
                dialogue.SetActive(true);
            }
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            QuestState.Instance.chickenSightTrigger = true;
            // popUp.gameObject.SetActive(false);
        }
    }
}
