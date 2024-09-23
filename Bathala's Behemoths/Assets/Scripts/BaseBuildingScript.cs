using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseBuildingScript : MonoBehaviour
{

    private bool isInTrigger = false;
    private PlayerMovement player;

    private void Start()
    {

    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Check for the key press only when inside the trigger
        if (context.performed && isInTrigger)
        {
            Debug.Log("E key pressed while inside the trigger!");
            //For demo purposes
            //To do for demo + future proofing, convert to using Singleton pattern for player stats
            player.speed *= 2;
        }
    }

    // Called when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            Debug.Log("Player entered the trigger");
            player = other.GetComponent<PlayerMovement>();
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            Debug.Log("Player exited the trigger");
        }
    }
}
