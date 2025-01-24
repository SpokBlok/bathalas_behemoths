using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CharredTreeClue : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI popUp;
    
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

    public void OnInteract(InputAction.CallbackContext context)
    {
        Transform clue1 = transform.Find("QuestBoardPanel/LeftPanel/Clue 1");
        Transform clue1Tracking = transform.Find("QuestBoardPanel/LeftPanel/Clue 1 Tracking");

        // Check for the key press only when inside the trigger
        if (context.performed && isInTrigger)
        {
            playerStats.clue1 = true;
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
