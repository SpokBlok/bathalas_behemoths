using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseBuildingScript : MonoBehaviour
{
    public BaseUpgradeUIPanel upgradePanel;

    private bool isInTrigger = false;

    private void Start()
    {

    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("E key e while inside the trigger!");
        // Check for the key press only when inside the trigger
        if (context.performed && isInTrigger)
        {
            Debug.Log("E key pressed while inside the trigger!");
            upgradePanel.gameObject.SetActive(true);
        }
    }

    // Called when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            Debug.Log("Player entered the trigger");
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            upgradePanel.DisablePanel();
            Debug.Log("Player exited the trigger");
        }
    }
}
