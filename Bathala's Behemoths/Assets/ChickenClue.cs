using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using TMPro;

public enum ChickenState
{
    Idle,
    Moving
}

public class ChickenClue : MonoBehaviour
{
    public TextMeshProUGUI popUp;
    public GameObject dialogue;
    public GameObject marker;
    
    private bool isInTrigger;

    private CharacterController chickenControl;
    public GameObject player;
    private Transform playerTransform;
    
    //Chicken State Machine
    public ChickenState chickenState;

    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        chickenControl = GetComponent<CharacterController>();
        speed = 25;

        if(QuestState.Instance.chickenRepeat)
        {
            marker.SetActive(false);
            PlayerStats.Instance.activeQuest4 = false;
        }
        
        isInTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (chickenState)
        {
            case ChickenState.Idle:
                break;

            case ChickenState.Moving:
                ChasePlayer();
                break;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Check for the key press only when inside the trigger
        if (context.performed && isInTrigger)
        {
            PlayerStats.Instance.clue4 = true;
            dialogue.SetActive(true);
            marker.SetActive(false);
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

    private void TerrainGravity()
    {
        Terrain myTerrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        float terrainHeight = myTerrain.SampleHeight(transform.position);
        Vector3 newPosition = transform.position;
        newPosition.y = terrainHeight + 2.5f;
        transform.position = newPosition;
    }

    public void ChasePlayer()
    {
        //Get vector from enemy to player and assign to x and z axes
        Vector3 moveDirection = (transform.position - playerTransform.position).normalized;
        Vector3 terrainMoveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z) * speed;

        // Move the enemy in the x and z direction
        chickenControl.Move(terrainMoveDirection * Time.deltaTime);
        
        TerrainGravity();
    }

    public void ChangeState(ChickenState newState) //Logic for entering states (e.g. playing animations)
    {
        chickenState = newState;

        switch (newState)
        {
            case ChickenState.Idle:
                // Enter Idle logic
                break;

            case ChickenState.Moving:
                // Enter Moving logic
                break;
        }
    }
}
