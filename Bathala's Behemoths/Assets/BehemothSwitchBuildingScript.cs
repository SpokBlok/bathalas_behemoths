using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BehemothSwitchBuildingScript : MonoBehaviour
{
    private BehemothSwitchUIPanel[] uiList;
    public BehemothSwitchUIPanel switchPanel;
    public TextMeshProUGUI popUp;

    private bool isPanelUp;
    private bool isInTrigger;

    void Start()
    {
        isPanelUp = false;
        isInTrigger = false;
        UpdateCanvas();
    }

    void Update()
    {
        if (uiList == null || switchPanel == null)
        {
            uiList = Object.FindObjectsByType<BehemothSwitchUIPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (BehemothSwitchUIPanel UI in uiList)
            {
                switchPanel = UI;
            }
        }
    }

    private void Awake()
    {
        uiList = Object.FindObjectsByType<BehemothSwitchUIPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (BehemothSwitchUIPanel UI in uiList)
        {
            switchPanel = UI;
        }
    }

    public void UpdateCanvas()
    {
        uiList = Object.FindObjectsByType<BehemothSwitchUIPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (BehemothSwitchUIPanel UI in uiList)
        {
            switchPanel = UI;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        UpdateCanvas();
        if (!QuestState.Instance.pauseActive)
        {
            if (context.performed && isInTrigger && !isPanelUp)
            {
                isPanelUp = true;
                switchPanel.EnablePanel();
                QuestState.Instance.menuActive = true;

                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
            }
            else if (context.performed && isInTrigger && isPanelUp)
            {
                isPanelUp = false;
                switchPanel.DisablePanel();
                QuestState.Instance.menuActive = false;

                UnityEngine.Cursor.visible = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
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
            QuestState.Instance.menuActive = false;
            if (switchPanel != null)
            {
                switchPanel.DisablePanel();
            }
        }
    }
}
