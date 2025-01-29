using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DesDwendeSighting : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject dialogue;
    public bool doneSighting;
    
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        isInTrigger = false;
        doneSighting = false;
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
            if(doneSighting == false)
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
            doneSighting = true;
            // popUp.gameObject.SetActive(false);
        }
    }
}
