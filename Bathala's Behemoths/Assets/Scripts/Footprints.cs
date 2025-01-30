using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Footprints : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI popUp;
    public GameObject dialogue;
    public GameObject dwendeFamily;
    
    private MeshFilter meshFilter;
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        isInTrigger = false;
        if(QuestState.Instance.footprintsRepeat)
        {
            dwendeFamily.SetActive(true);
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
            dwendeFamily.SetActive(true);
            TurnInvisible();
            dialogue.SetActive(true);
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
