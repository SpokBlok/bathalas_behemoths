using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public enum KapreState
{
    Idle,
    Moving,
    Attacking,
    Knockback
}

public class KapreMob : MonoBehaviour
{
    private CharacterController kapreControl;
    private Transform playerTransform;
    private Transform target;

    public float health;
    public bool playerInRange;
    public int speed;

    //For intro kill quest tutorial maybe?
    public KillQuestUI[] killQuestUIList;
    public KillQuestUI killQuestUI;

    private GameObject objectHit;

    //Kapre State Machine
    private KapreState kapreState;

    private bool isAttacking = false;

    //Radius Trigger, maybe change to kapreRadius?
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

        kapreControl = GetComponent<CharacterController>();

        //Start off on terrain height
        TerrainGravity();

        //Starting state
        kapreState = KapreState.Idle;

        //KapreRadius?
        radius = GetComponentInChildren<EnemyRadiusTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (kapreState)
        {
            case KapreState.Idle:
                break;

            case KapreState.Moving:
                ChasePlayer();
                UpdateRotationTarget();
                break;

            case KapreState.Attacking:
                UpdateRotationTarget();
                if (!isAttacking)
                {
                    isAttacking = true;
                    StartCoroutine(BasicAttack());
                }
                break;

            case KapreState.Knockback:
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

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            PlayerStats.Instance.AddKapreCigars(1);
            //Disabled for now in lieu of having kapre cigar currency
            //Might need in for intro kill quest anyway?
            //if (killQuestUI.gameObject.activeSelf)
            //{
            //    killQuestUI.KillQuestCount += 1;
            //}
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
        kapreControl.Move(terrainMoveDirection * Time.deltaTime);

        TerrainGravity();
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
        if (objectHit != other.gameObject && other.gameObject.CompareTag("Projectile"))
        {
            objectHit = other.gameObject;
            takeDamage(PlayerStats.Instance.basicAttackDamage);
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            ChangeState(KapreState.Attacking);
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

    public void ChangeState(KapreState newState) //Logic for entering states (e.g. playing animations)
    {
        kapreState = newState;

        switch (newState)
        {
            case KapreState.Idle:
                // Enter Idle logic
                break;

            case KapreState.Moving:
                // Enter Moving logic
                break;

            case KapreState.Attacking:
                // Enter Attacking logic
                break;

            case KapreState.Knockback:
                // Enter Knockback logic
                break;
        }
    }
}
