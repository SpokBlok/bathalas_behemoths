using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossMob : MonoBehaviour
{
    private CharacterController enemyControl;
    private Transform playerTransform;
    private Transform target;

    public int health = 50;

    private GameObject bulletHit;

    public SpawnerScript spawner;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        spawner  = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerScript>();
        enemyControl = GetComponent<CharacterController>();

        //Start off on terrain height
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        Vector3 newPosition = transform.position;
        newPosition.y = terrainHeight + 1.2f;
        transform.position = newPosition;
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
            spawner.enemyCount--;
            Destroy(gameObject);
        }
    }

    public void UpdateRotationTarget()
    {
        //Create a plane on the player's position
        Plane playerPlane = new Plane(Vector3.up, transform.position);


        //Create vector from player position to mouse pointer
        var lookPos = (target.position - transform.position).normalized;
        lookPos.y = 0;

        //Rotate enemy toward player
        Quaternion targetRotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
    }

    public void ChasePlayer()
    {
        target = playerTransform;
        UpdateRotationTarget();
        //Get vector from enemy to player and assign to x and z axes
        Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
        Vector3 terrainMoveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z) * 4;

        // Move the enemy in the x and z direction
        enemyControl.Move(terrainMoveDirection * Time.deltaTime);

        //Get terrain height + 2 and assign it to y axis position
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        Vector3 newPosition = transform.position;
        newPosition.y = terrainHeight + 1.2f;
        transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bulletHit != other.gameObject && other.gameObject.CompareTag("Projectile"))
        {
            bulletHit = other.gameObject;
            takeDamage(PlayerStats.Instance.basicAttackDamage);
        }
    }
}
