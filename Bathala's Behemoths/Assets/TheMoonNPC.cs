using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TheMoonNPC : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI popUp;
    public GameObject dialogue;
    public GameObject moonSighting;
    public GameObject moonBracelet;
    public GameObject moonNPC;
    
    private MeshFilter meshFilter;
    public Mesh moonMarkerMesh;
    public Mesh moonQuestMesh;
    
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = moonQuestMesh;

        isInTrigger = false;
        if(QuestState.Instance.moonNPCRepeat)
        {
            if(QuestState.Instance.moonSightingTrigger == false)
            {
                moonSighting.SetActive(true);
            }

            if(QuestState.Instance.moonQuestTrigger && QuestState.Instance.moonChunkGet == false)
            {
                moonBracelet.SetActive(true);
                TurnInvisible();
            }
            else if(QuestState.Instance.moonQuestEnded == false && QuestState.Instance.moonChunkGet)
            {
                moonBracelet.SetActive(false);
                moonNPC.SetActive(false);
                TurnVisible();
            }
            else
            {
                moonBracelet.SetActive(false);
                moonNPC.SetActive(false);
                TurnInvisible();
            }
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
            if(QuestState.Instance.moonSightingTrigger == false)
            {
                moonSighting.SetActive(true);
            }
            
            if(QuestState.Instance.moonChunkGet == false)
            {
                moonBracelet.SetActive(true);
            }
            QuestState.Instance.moonQuestTrigger = true;
            dialogue.SetActive(true);
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
            meshFilter.mesh = null;  // Change the mesh to null
        }
    }

    public void TurnVisible()
    {
        if (meshFilter != null)
        {
            meshFilter.mesh = moonMarkerMesh;  // Change the mesh to monMarkerMesh
        }
    }
}
