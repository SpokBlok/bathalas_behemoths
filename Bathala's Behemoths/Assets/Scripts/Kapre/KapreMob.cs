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

    // Variables cached for referencing later
    public Vector3 moveDirection;
    public Vector3 terrainMoveDirection;
    public Vector3 newPosition;
    public float terrainHeight;
    public Terrain myTerrain;
    public Plane playerPlane;
    public Vector3 lookPos;
    Quaternion targetRotation;

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

    public SkinnedMeshRenderer[] modelRenderer;
    public Coroutine takingDamage;
    public AudioClip kapreAttackAudio;
    public AudioClip clubSmackAudio;

    // Start is called before the first frame update
    void Start()
    {
        if (modelRenderer == null)
        {
            modelRenderer = kapreModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        killQuestUIList = Resources.FindObjectsOfTypeAll<KillQuestUI>();
        foreach (KillQuestUI UI in killQuestUIList)
        {
            killQuestUI = UI;
        }

        SphereCollider triggerRadius = GetComponentInChildren<SphereCollider>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        kapreControl = GetComponent<CharacterController>();
        myTerrain = GameObject.Find("Terrain").GetComponent<Terrain>();

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
        if(QuestState.Instance.pausedForDialogue){return;}

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
        terrainHeight = myTerrain.SampleHeight(transform.position);
        newPosition = transform.position;
        newPosition.y = terrainHeight + 1.2f;
        transform.position = newPosition;
    }

    public void ChasePlayer()
    {
        target = playerTransform;
        //Get vector from enemy to player and assign to x and z axes
        moveDirection = (target.position - transform.position).normalized;
        terrainMoveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z) * speed;

        // Move the enemy in the x and z direction
        kapreControl.Move(terrainMoveDirection * Time.deltaTime);

        TerrainGravity();
    }

    public void UpdateRotationTarget()
    {
        //Create a plane on the player's position
        playerPlane = new Plane(Vector3.up, transform.position);

        //Create vector from player position to mouse pointer
        lookPos = (target.position - transform.position).normalized;
        lookPos.y = 0;

        //Rotate enemy toward player
        targetRotation = Quaternion.LookRotation(lookPos);
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
        AudioSource.PlayClipAtPoint(kapreAttackAudio, Camera.main.transform.position + Camera.main.transform.forward * 2.5f, 1f);
        worldCenter = basicAttackHitbox.transform.TransformPoint(basicAttackHitbox.center);
        worldSize = Vector3.Scale(basicAttackHitbox.size, basicAttackHitbox.transform.lossyScale) / 2;

        yield return new WaitForSeconds(0.5f);
        AudioSource.PlayClipAtPoint(clubSmackAudio, Camera.main.transform.position + Camera.main.transform.forward * 2.5f, 1f);
        
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
        if(takingDamage == null)
        {
            takingDamage = StartCoroutine(SwitchToDamagedTex());
        }
        else if(takingDamage != null)
        {
            StopCoroutine(takingDamage);
            takingDamage = StartCoroutine(SwitchToDamagedTex());
        }

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
        kapreModel.setStunned(true);
        ChangeState(KapreState.Stunned);
        if(basicAttackCoroutine != null)
        {
            StopCoroutine(basicAttackCoroutine);
        }
        yield return new WaitForSeconds(duration);
        radius.TriggerCheck();
        yield return null;
        kapreModel.setStunned(false);
    }

    IEnumerator SwitchToDamagedTex()
    {
        foreach(SkinnedMeshRenderer bodypart in modelRenderer)
        {
            bodypart.material.color = Color.red; // Change color to red
        }
        
        yield return new WaitForSeconds(0.2f); // Wait

        foreach(SkinnedMeshRenderer bodypart in modelRenderer)
        {
            bodypart.material.color = Color.white; // Restore original color
        }
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
