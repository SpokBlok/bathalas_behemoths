using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class QuestBoardScript : MonoBehaviour
{
    public QuestBoardUIPanel[] questUIList;
    public QuestBoardUIPanel questUIPanel;
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
        if(questUIList == null || questUIPanel == null)
        {
            questUIList = Resources.FindObjectsOfTypeAll<QuestBoardUIPanel>();
            foreach (QuestBoardUIPanel UI in questUIList)
            {
                questUIPanel = UI;
            }
        }
    }

    private void Awake()
    {
        questUIList = Resources.FindObjectsOfTypeAll<QuestBoardUIPanel>();
        foreach (QuestBoardUIPanel UI in questUIList)
        {
            questUIPanel = UI;
        }
    }

    void UpdateCanvas()
    {
        questUIList = Resources.FindObjectsOfTypeAll<QuestBoardUIPanel>();

        foreach (QuestBoardUIPanel UI in questUIList)
        {
            questUIPanel = UI;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        UpdateCanvas();
        if(!QuestState.Instance.pauseActive)
        {
            // Check for the key press only when inside the trigger
            if (context.performed && isInTrigger && !isPanelUp && 
                !QuestState.Instance.pausedForDialogue)
            {
                isPanelUp = true;
                questUIPanel.EnablePanel();
                QuestState.Instance.menuActive = true;

                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
            }
            else if (context.performed && isInTrigger && isPanelUp)
            {
                isPanelUp = false;
                questUIPanel.DisablePanel();
                QuestState.Instance.menuActive = false;

                UnityEngine.Cursor.visible = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    // Called when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Inside Trigger Enter of QuestBoard");
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            popUp.gameObject.SetActive(true);
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        // Debug.Log("Inside Trigger Exit of QuestBoard");
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            isPanelUp = false;
            popUp.gameObject.SetActive(false);
            if(questUIPanel != null)
            {
                questUIPanel.DisablePanel();
            }
        }
    }
}
