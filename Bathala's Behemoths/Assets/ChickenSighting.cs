using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ChickenSighting : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject dialogue;
    
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        isInTrigger = false;
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
            if(QuestState.Instance.chickenSightTrigger == true)
            {
                QuestState.Instance.chickenSightTrigger = false;
                dialogue.SetActive(true);
                Debug.Log("CHICKEN TRIGGER TRIGGERED");
            }
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            QuestState.Instance.chickenSightTrigger = false;
            // popUp.gameObject.SetActive(false);
        }
    }
}
