using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MoonBraceletSighting : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject dialogue;
    public bool doneSighting;
    
    public bool moonQuestTrigger;
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        isInTrigger = false;
        doneSighting = false;
        moonQuestTrigger = false;
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
            if(doneSighting == false && moonQuestTrigger == true)
            {
                dialogue.SetActive(true);
                doneSighting = true;
            }
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            // popUp.gameObject.SetActive(false);
        }
    }
}
