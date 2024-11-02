using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyMob : MonoBehaviour
{
    private CharacterController enemyControl;
    private Transform playerTransform;

    public int health;
    public bool playerInRange;
    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        health = 50;
        speed = 4;

        SphereCollider triggerRadius = GetComponentInChildren<SphereCollider>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        enemyControl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Enemy killed!");
            Destroy(gameObject);
        }
    }

    public void ChasePlayer()
    {
        if (playerInRange){

            //Get vector from enemy to player and assign to x and z axes
            Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
            Vector3 terrainMoveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z) * speed;

            // Move the enemy in the x and z direction
            enemyControl.Move(terrainMoveDirection * Time.deltaTime);

            //Get terrain height + 2 and assign it to y axis position
            float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
            Vector3 newPosition = transform.position;
            newPosition.y = terrainHeight + 2;
            transform.position = newPosition;
        }
    }
}
