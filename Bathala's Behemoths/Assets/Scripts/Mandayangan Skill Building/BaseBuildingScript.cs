using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BaseBuildingScript : MonoBehaviour
{
    public MandayanganSkillsUIPanel[] uiList;
    public MandayanganSkillsUIPanel upgradePanel;
    public TextMeshProUGUI popUp;

    private bool isPanelUp;
    private bool isInTrigger;
    private bool isBuilt;

    private void Start()
    {
        isPanelUp = false;
        isInTrigger = false;
        isBuilt = false;
        UpdateCanvas();
    }

    public void UpdateCanvas()
    {
        uiList = Resources.FindObjectsOfTypeAll<MandayanganSkillsUIPanel>();

        foreach (MandayanganSkillsUIPanel UI in uiList)
        {
            upgradePanel = UI;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {

        // Check for the key press only when inside the trigger
        if (context.performed && isInTrigger && !isPanelUp)
        {
            isPanelUp = true;
            upgradePanel.EnablePanel();
            if (!isBuilt)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                isBuilt = true;
            }
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
