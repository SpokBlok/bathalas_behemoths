using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InvestigationClueScript : MonoBehaviour
{
    public QuestUI questUI;
    public TextMeshProUGUI popUp;

    private bool isInTrigger;
    private bool itemActivated;

    // Start is called before the first frame update
    void Start()
    {
        isInTrigger = false;
        itemActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Check for the key press only when inside the trigger
        if (context.performed && isInTrigger && !itemActivated)
        {
            Debug.Log("Item activated");
            itemActivated = true;
            questUI.QuestItemCount += 1;
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
