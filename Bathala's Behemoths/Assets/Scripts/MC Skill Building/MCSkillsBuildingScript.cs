using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MCSkillsBuildingScript : MonoBehaviour
{
    private MCSkillsUIPanel[] uiList;
    public MCSkillsUIPanel upgradePanel;
    public TextMeshProUGUI popUp;

    private bool isPanelUp;
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        isPanelUp = false;
        isInTrigger = false;
        UpdateCanvas();
    }

    void update()
    {
        if(uiList == null || upgradePanel == null)
        {
            uiList = Resources.FindObjectsOfTypeAll<MCSkillsUIPanel>();
            foreach (MCSkillsUIPanel UI in uiList)
            {
                upgradePanel = UI;
            }
        }
    }

    private void Awake()
    {
        uiList = Resources.FindObjectsOfTypeAll<MCSkillsUIPanel>();
        foreach (MCSkillsUIPanel UI in uiList)
        {
            upgradePanel = UI;
        }
    }

    public void UpdateCanvas()
    {
        uiList = Resources.FindObjectsOfTypeAll<MCSkillsUIPanel>();

        foreach (MCSkillsUIPanel UI in uiList)
        {
            upgradePanel = UI;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        UpdateCanvas();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            popUp.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            isPanelUp = false;
            popUp.gameObject.SetActive(false);
            if(upgradePanel != null)
            {
                upgradePanel.DisablePanel();
            }
        }
    }
}
