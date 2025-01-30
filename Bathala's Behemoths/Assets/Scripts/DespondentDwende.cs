using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DespondentDwende : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI popUp;
    public GameObject dialogue;
    public GameObject footprints;
    public GameObject marker;
    
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        isInTrigger = false;
        if(QuestState.Instance.desponDwendeRepeat)
        {
            footprints.SetActive(true);
            marker.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Check for the key press only when inside the trigger
        if (context.performed && isInTrigger)
        {
            footprints.SetActive(true);
            marker.SetActive(false);
            dialogue.SetActive(true);
        }
    }

    // Called when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            popUp.gameObject.SetActive(true);
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            popUp.gameObject.SetActive(false);
        }
    }
}
