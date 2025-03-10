using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using TMPro;

public class NormalChicken : MonoBehaviour
{
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
        speed = 20;
        
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

    // Called when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
        }
    }

    private void TerrainGravity()
    {
        Terrain myTerrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        float terrainHeight = myTerrain.SampleHeight(transform.position);
        Vector3 newPosition = transform.position;
        newPosition.y = terrainHeight + 5.5f;
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
