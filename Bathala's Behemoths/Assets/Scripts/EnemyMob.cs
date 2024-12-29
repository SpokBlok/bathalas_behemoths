using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public enum EnemyState
{
    Idle,
    Moving,
    Attacking,
    Knockback
}

public class EnemyMob : MonoBehaviour
{
    private CharacterController enemyControl;
    private Transform playerTransform;
    private Transform target;

    public float health;
    public bool playerInRange;
    public int speed;

    public KillQuestUI[] killQuestUIList;
    public KillQuestUI killQuestUI;

    private GameObject objectHit;

    //Enemy State Machine
    private EnemyState enemyState;

    private bool isAttacking = false;

    //Radius Trigger
    private EnemyRadiusTrigger radius;

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

        //Starting state
        enemyState = EnemyState.Idle;

        radius = GetComponentInChildren<EnemyRadiusTrigger>();

        //Subscribe to trigger check event
        PlayerMovement.OnDashComplete += radius.TriggerCheck;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                break;

            case EnemyState.Moving:
                ChasePlayer();
                UpdateRotationTarget();
                break;

            case EnemyState.Attacking:
                UpdateRotationTarget();
                if (!isAttacking)
                {
                    isAttacking = true;
                    StartCoroutine(BasicAttack());
                }
                break;

            case EnemyState.Knockback:
                break;
        }
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            PlayerStats.Instance.AddKapreCigars(1);
            //Disabled for now in lieu of having kapre cigar currency
            //Might need to fully convert to separate kapre mob script
            //if (killQuestUI.gameObject.activeSelf)
            //{
            //    killQuestUI.KillQuestCount += 1;
            //}
            PlayerMovement.OnDashComplete -= radius.TriggerCheck;
            Debug.Log("Enemy killed!");
            Destroy(gameObject);
        }
    }

    public void ChasePlayer()
    {
        target = playerTransform;
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            ChangeState(EnemyState.Attacking);
        }
    }

    private IEnumerator BasicAttack()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Enemy Attack");
        isAttacking = false;
        radius.TriggerCheck();
        yield return new WaitForSeconds(0.3f);
        yield break;
    }

    public void ChangeState(EnemyState newState) //Logic for entering states (e.g. playing animations)
    {
        enemyState = newState;

        switch (newState)
        {
            case EnemyState.Idle:
                // Enter Idle logic
                break;

            case EnemyState.Moving:
                // Enter Moving logic
                break;

            case EnemyState.Attacking:
                // Enter Attacking logic
                break;

            case EnemyState.Knockback:
                // Enter Knockback logic
                break;
        }
    }
}
