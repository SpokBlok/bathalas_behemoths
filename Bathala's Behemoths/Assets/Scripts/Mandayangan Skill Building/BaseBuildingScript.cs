using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BaseBuildingScript : MonoBehaviour
{
    public MandayanganSkillsUIPanel[] uiList;
    public MandayanganSkillsUIPanel upgradePanel;
    public TambanokanoSkillsUIPanel tammyUpgradePanel;
    public MarkupoSkillsUIPanel markyUpgradePanel;
    public TextMeshProUGUI popUp;

    private bool isPanelUp;
    private bool isInTrigger;
    private bool isBuilt;
    private int openedPanelModelIndex = 1;

    private void Start()
    {
        isPanelUp = false;
        isInTrigger = false;
        isBuilt = false;
        UpdateCanvas();
    }

    public void UpdateCanvas()
    {
        uiList = Object.FindObjectsByType<MandayanganSkillsUIPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        upgradePanel = FindPanelByName(uiList, "BehemothPanel");

        TambanokanoSkillsUIPanel[] tammyPanels = Object.FindObjectsByType<TambanokanoSkillsUIPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        tammyUpgradePanel = FindPanelByName(tammyPanels, "TammyPanel");

        MarkupoSkillsUIPanel[] markyPanels = Object.FindObjectsByType<MarkupoSkillsUIPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        markyUpgradePanel = FindPanelByName(markyPanels, "MarkyPanel");
    }

    private void Awake()
    {
        UpdateCanvas();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        UpdateCanvas();
        if(!QuestState.Instance.pauseActive)
        {
            // Check for the key press only when inside the trigger
            if (context.performed && isInTrigger && !isPanelUp)
            {
                if (!TryEnableResolvedPanel())
                {
                    return;
                }

                isPanelUp = true;
                QuestState.Instance.menuActive = true;
                
                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;

                if (!isBuilt)
                {
                    gameObject.GetComponent<MeshRenderer>().enabled = true;
                    isBuilt = true;
                }
            } 
            else if (context.performed && isInTrigger && isPanelUp)
            {
                isPanelUp = false;
                DisableOpenedPanel();
                QuestState.Instance.menuActive = false;

                UnityEngine.Cursor.visible = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
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
            QuestState.Instance.menuActive = false;
            DisableOpenedPanel();
            // Debug.Log("Player exited the trigger");
        }
    }

    private bool TryEnableResolvedPanel()
    {
        UpdateCanvas();
        openedPanelModelIndex = PlayerStats.Instance != null ? PlayerStats.Instance.playerModelIndex : 1;

        switch (openedPanelModelIndex)
        {
            case 2:
                if (tammyUpgradePanel == null)
                {
                    Debug.LogError("BaseBuildingScript: TammyPanel could not be found.");
                    return false;
                }

                tammyUpgradePanel.EnablePanel();
                return true;

            case 3:
                if (markyUpgradePanel == null)
                {
                    Debug.LogError("BaseBuildingScript: MarkyPanel could not be found.");
                    return false;
                }

                markyUpgradePanel.EnablePanel();
                return true;

            default:
                if (upgradePanel == null)
                {
                    Debug.LogError("BaseBuildingScript: BehemothPanel could not be found.");
                    return false;
                }

                upgradePanel.EnablePanel();
                return true;
        }
    }

    private void DisableOpenedPanel()
    {
        switch (openedPanelModelIndex)
        {
            case 2:
                if (tammyUpgradePanel != null)
                {
                    tammyUpgradePanel.DisablePanel();
                }
                break;

            case 3:
                if (markyUpgradePanel != null)
                {
                    markyUpgradePanel.DisablePanel();
                }
                break;

            default:
                if (upgradePanel != null)
                {
                    upgradePanel.DisablePanel();
                }
                break;
        }
    }

    private static T FindPanelByName<T>(T[] panels, string panelName) where T : MonoBehaviour
    {
        T fallback = null;

        foreach (T panel in panels)
        {
            if (panel == null)
            {
                continue;
            }

            if (fallback == null)
            {
                fallback = panel;
            }

            if (panel.gameObject.name == panelName)
            {
                return panel;
            }
        }

        return fallback;
    }
}
