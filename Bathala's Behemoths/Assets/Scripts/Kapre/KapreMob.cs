using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public enum KapreState
{
    Idle,
    Moving,
    Attacking,
    Knockback,
    Stunned
}

public class KapreMob : EnemyMob
{
    private CharacterController kapreControl;
    private Transform playerTransform;
    private Transform target;
    public bool markupoMob;

    public bool playerInRange;
    public int speed;
    public float attackDamage;

    //For intro kill quest tutorial maybe?
    public KillQuestUI[] killQuestUIList;
    public KillQuestUI killQuestUI;

    private GameObject objectHit;

    //Kapre State Machine
    public KapreState kapreState;

    private bool isAttacking = false;
    public KapreAnimController kapreModel;

    private KapreRadiusTrigger radius;

    //For basic attack logic
    private BoxCollider basicAttackHitbox;
    public Coroutine basicAttackCoroutine;
    Vector3 worldCenter;
    Vector3 worldSize;

    // Start is called before the first frame update
    void Start()
    {
        killQuestUIList = Resources.FindObjectsOfTypeAll<KillQuestUI>();
        foreach (KillQuestUI UI in killQuestUIList)
        {
            killQuestUI = UI;
        }

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

        radius = GetComponentInChildren<KapreRadiusTrigger>();

        //Subscribe to trigger check event
        EventManager.OnDashComplete += radius.TriggerCheck;

        basicAttackHitbox = transform.Find("Basic Attack/Basic Attack Hitbox").GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (kapreState)
        {
            case KapreState.Idle:
                if (markupoMob)
                {
                    ChangeState(KapreState.Moving);
                }
                break;

            case KapreState.Moving:
                ChasePlayer();
                UpdateRotationTarget();
                break;

            case KapreState.Attacking:
                if (!isAttacking)
                {
                    UpdateRotationTarget();
                    isAttacking = true;
                    basicAttackCoroutine = StartCoroutine(BasicAttack());
                }
                break;

            case KapreState.Knockback:
                break;

            case KapreState.Stunned:
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
        if (markupoMob)
        {
            target = playerTransform;
        }
        //Get vector from enemy to player and assign to x and z axes
        Vector3 moveDirection = (target.position - transform.position).normalized;
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            ChangeState(KapreState.Attacking);
        }
    }

    private IEnumerator BasicAttack()
    {
        worldCenter = basicAttackHitbox.transform.TransformPoint(basicAttackHitbox.center);
        worldSize = Vector3.Scale(basicAttackHitbox.size, basicAttackHitbox.transform.lossyScale) / 2;

        yield return new WaitForSeconds(0.5f);
        BasicAttackHitboxCheck(Physics.OverlapBox(worldCenter, worldSize, basicAttackHitbox.transform.rotation));
        if (!markupoMob)
        {
            radius.TriggerCheck();
        }
        isAttacking = false;
    }

    private void BasicAttackHitboxCheck(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<PlayerMovement>().TakeDamage(attackDamage);
            } 
            else if (collider.CompareTag("Markupo"))
            {
                collider.GetComponent<EnemyMob>().TakeDamage(attackDamage);
            }
        }
    }

    public override void TakeDamage(float damage)
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
            EventManager.OnDashComplete -= radius.TriggerCheck;
            Debug.Log("Enemy killed!");
            Destroy(gameObject);
        }
    }

    public override IEnumerator Stun(float duration)
    {
        ChangeState(KapreState.Stunned);
        StopCoroutine(basicAttackCoroutine);
        yield return new WaitForSeconds(duration);
        radius.TriggerCheck();
        yield return null;
    }

    public void ChangeState(KapreState newState) //Logic for entering states (e.g. playing animations)
    {
        kapreState = newState;

        switch (newState)
        {
            case KapreState.Idle:
                // Enter Idle logic
                isAttacking = false;
                kapreModel.setAttacking(false);
                kapreModel.setMoving(false);
                break;

            case KapreState.Moving:
                // Enter Moving logic
                isAttacking = false;
                kapreModel.setAttacking(false);
                kapreModel.setMoving(true);
                break;

            case KapreState.Attacking:
                // Enter Attacking logic
                kapreModel.setAttacking(true);
                kapreModel.setMoving(false);
                break;

            case KapreState.Knockback:
                // Enter Knockback logic
                isAttacking = false;
                kapreModel.setAttacking(false);
                kapreModel.setMoving(false);
                break;

            case KapreState.Stunned:
                isAttacking = false;
                kapreModel.setAttacking(false);
                kapreModel.setMoving(false);
                Debug.Log("Got stunned");
                break;
        }
    }

    private void OnDestroy()
    {
        EventManager.OnDashComplete -= radius.TriggerCheck;
    }
}
