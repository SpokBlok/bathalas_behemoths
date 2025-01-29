using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public enum ChickenState
{
    Idle,
    Moving
}

public class ChickenClue : MonoBehaviour
{
    private CharacterController chickenControl;
    public GameObject player;
    private Transform playerTransform;
    
    //Chicken State Machine
    public ChickenState chickenState;

    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.transform;
        chickenControl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (chickenState)
        {
            case ChickenState.Idle:
                ChangeState(ChickenState.Moving);
                break;

            case ChickenState.Moving:
                ChasePlayer();
                break;
        }
    }

    private void TerrainGravity()
    {
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        Vector3 newPosition = transform.position;
        newPosition.y = terrainHeight + 1.2f;
        transform.position = newPosition;
    }

    public void ChasePlayer()
    {
        //Get vector from enemy to player and assign to x and z axes
        Vector3 moveDirection = (playerTransform.position + transform.position).normalized;
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
