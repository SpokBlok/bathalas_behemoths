using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class QuestBoardScript : MonoBehaviour
{
    public QuestUI[] questUIList;
    public KillQuestUI[] killQuestUIList;
    public QuestUI questUI;
    public KillQuestUI killUI;
    public TextMeshProUGUI popUp;

    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCanvas();
    }

    void UpdateCanvas()
    {
        questUIList = Resources.FindObjectsOfTypeAll<QuestUI>();

        foreach (QuestUI UI in questUIList)
        {
            questUI = UI;
        }

        killQuestUIList = Resources.FindObjectsOfTypeAll<KillQuestUI>();

        foreach (KillQuestUI UI in killQuestUIList)
        {
            killUI = UI;
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
            UpdateCanvas();
            Debug.Log("E key pressed while inside the trigger!");
            questUI.transform.parent.gameObject.SetActive(true);
            killUI.transform.parent.gameObject.SetActive(true);
        }
    }

    // Called when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popUp.gameObject.SetActive(true);
            isInTrigger = true;
            Debug.Log("Player entered the trigger");
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popUp.gameObject.SetActive(false);
            isInTrigger = false;
            Debug.Log("Player exited the trigger");
        }
    }
}
