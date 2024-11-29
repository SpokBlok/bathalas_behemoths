using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyMob : MonoBehaviour
{
    private CharacterController enemyControl;
    private Transform playerTransform;
    private Transform target;

    public int health;
    public bool playerInRange;
    public int speed;

    public KillQuestUI[] killQuestUIList;
    public KillQuestUI killQuestUI;

    private GameObject bulletHit;

    // Start is called before the first frame update
    void Start()
    {
        killQuestUIList = Resources.FindObjectsOfTypeAll<KillQuestUI>();
        foreach (KillQuestUI UI in killQuestUIList)
        {
            killQuestUI = UI;
        }
        health = 50;
        speed = 4;

        SphereCollider triggerRadius = GetComponentInChildren<SphereCollider>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

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
            if (killQuestUI.gameObject.activeSelf)
            {
                killQuestUI.KillQuestCount += 1;
            }
            Debug.Log("Enemy killed!");
            Destroy(gameObject);
        }
    }

    public void ChasePlayer()
    {
        if (playerInRange){
            target = playerTransform;
            UpdateRotationTarget();
            //Get vector from enemy to player and assign to x and z axes
            Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
            Vector3 terrainMoveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z) * speed;

            // Move the enemy in the x and z direction
            enemyControl.Move(terrainMoveDirection * Time.deltaTime);

            //Get terrain height + 2 and assign it to y axis position
            float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
            Vector3 newPosition = transform.position;
            newPosition.y = terrainHeight + 1.2f;
            transform.position = newPosition;
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

    private void OnTriggerEnter(Collider other)
    {
        if (bulletHit != other.gameObject && other.gameObject.CompareTag("Projectile")) {
            bulletHit = other.gameObject;
            takeDamage(PlayerStats.Instance.basicAttackDamage);
        }
    }
}
