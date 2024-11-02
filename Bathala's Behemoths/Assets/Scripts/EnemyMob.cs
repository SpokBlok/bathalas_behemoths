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
        TerrainGravity();
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
            Vector3 moveDirection= (playerTransform.position - transform.position).normalized;
            Vector3 terrainMoveDirection = new Vector3(moveDirection.x, 0f, moveDirection.y) * speed;

            enemyControl.Move(terrainMoveDirection * Time.deltaTime);
        }
    }

    public void TerrainGravity()
    {
        Vector3 position = enemyControl.transform.position;

        // Get the terrain height at the character's current position (X, Z)
        float terrainHeight = Terrain.activeTerrain.SampleHeight(position);

        // Set the character's Y position to match the terrain height + 1
        position.y = terrainHeight + 2;

        enemyControl.enabled = false; // Temporarily disable to directly set position
        enemyControl.transform.position = position;
        enemyControl.enabled = true;

    }
}
