using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BaseBuildingScript : MonoBehaviour
{
    public BaseUpgradeUIPanel upgradePanel;
    public TextMeshProUGUI popUp;

    private bool isPanelUp;
    private bool isInTrigger;

    private void Start()
    {
        isPanelUp = false;
        isInTrigger = false;
}

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Check for the key press only when inside the trigger
        if (context.performed && isInTrigger && !isPanelUp)
        {
            isPanelUp = true;
            upgradePanel.EnablePanel();
        } 
        else if (context.performed && isInTrigger && isPanelUp)
        {
            isPanelUp = false;
            upgradePanel.DisablePanel();
        }
    }

    // Called when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            popUp.gameObject.SetActive(true);
            Debug.Log("Player entered the trigger");
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            isPanelUp = false;
            popUp.gameObject.SetActive(false);
            upgradePanel.DisablePanel();
            Debug.Log("Player exited the trigger");
        }
    }
}
