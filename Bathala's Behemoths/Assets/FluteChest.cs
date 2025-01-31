using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class FluteChest : MonoBehaviour
{
    public TextMeshProUGUI popUp;
    public GameObject dialogue;
    public GameObject closedChest;
    public GameObject openChest;
    public GameObject markerDwende;
    
    private MeshFilter meshFilter;
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        isInTrigger = false;

        if(QuestState.Instance.fluteGet == true)
        {
            closedChest.SetActive(false);
            openChest.SetActive(true);
            markerDwende.SetActive(true);
            TurnInvisible();
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
            closedChest.SetActive(false);
            openChest.SetActive(true);
            QuestState.Instance.fluteGet = true;
            dialogue.SetActive(true);
            markerDwende.SetActive(true);
            TurnInvisible();
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
            popUp.gameObject.SetActive(false);
        }
    }

    public void TurnInvisible()
    {
        if (meshFilter != null)
        {
            meshFilter.mesh = null;  // Change the mesh to model1
        }
    }
}
