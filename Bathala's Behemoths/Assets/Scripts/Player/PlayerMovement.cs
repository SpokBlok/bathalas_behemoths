using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum PlayerState
{
    Idle,
    Moving,
    Attacking,
    Knockback
}

public class PlayerMovement : MonoBehaviour
{
    // Look rotation variables
    private Vector2 mouseLook;
    private Vector3 rotationTarget;
    public float minDistanceToLook = 0.1f;
    public Vector3 lookPos;

    // Movement variables
    public Vector2 move;

    // Knockback variables
    public float pushBackDuration = 0.5f;
    public float pushBackForce = 10f;

    //Attack variables
    public bool isAttacking = false;
    public bool isSkillingOrUlting = false;

    // Character Conroller
    public CharacterController charControl;
    private float acceleration = 10f;
    private float deceleration = 15f;

    // FSM State
    private PlayerState currentState;

    //Projectile Prefab
    public GameObject projectilePrefab;

    //Berserk Ult bool
    public bool isBerserk;

    private PlayerStats stats;

    //For basic attack logic
    private BoxCollider basicAttackHitbox;
    private Vector3 worldCenter;
    private Vector3 worldSize;
    public Coroutine basicAttackCoroutine;
    public InputActionReference mouseAction;

    private void Start()
    {
        // Get references to char controller + collider
        charControl = GetComponent<CharacterController>();

        // Start with Idle state
        currentState = PlayerState.Idle;

        //Starting alignment with terrain
        TerrainGravity();

        stats = PlayerStats.Instance;

        basicAttackHitbox = transform.Find("Basic Attack/Basic Attack Hitbox").GetComponent<BoxCollider>();

        if (stats.introDone && stats.outdoorsScene)
        {
            gameObject.transform.position = new Vector3(588.2f, 81.7f, 261.7f);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();

        // Only act on the input when it is performed
        // If there is input when not attacking, transition to the Moving state
        if (context.performed)
        {
            if (move != Vector2.zero && currentState != PlayerState.Moving && currentState != PlayerState.Attacking)
            {
                ChangeState(PlayerState.Moving);
            }
        }
        else if (context.canceled)
        {
            if (currentState != PlayerState.Idle && currentState != PlayerState.Attacking)
            {
                ChangeState(PlayerState.Idle);
            }
        }
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    public void OnBasicAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.performed && !isAttacking)
            {
                ChangeState(PlayerState.Attacking);
                basicAttackCoroutine = StartCoroutine(BasicAttack());
            }
        }
    }

    public void OnMCSkillTrigger(InputAction.CallbackContext context) {
        
        if (context.performed)
        {
            if (context.performed && !isAttacking)
            {
                ChangeState(PlayerState.Attacking);
                GameObject skillManager = GameObject.FindGameObjectWithTag("Player Skills");
                StartCoroutine(skillManager.GetComponent<PlayerSkills>().RunMainCharacterSkill());
            }
        }
    }

    public void OnBehemothSkillQTrigger(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.performed && !isAttacking)
            {
                ChangeState(PlayerState.Attacking);
                GameObject skillManager = GameObject.FindGameObjectWithTag("Player Skills");
                StartCoroutine(skillManager.GetComponent<PlayerSkills>().RunBehemothSkillQ());

            }
        }
    }

    public void OnBehemothSkillETrigger(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.performed && !isAttacking)
            {
                ChangeState(PlayerState.Attacking);
                GameObject skillManager = GameObject.FindGameObjectWithTag("Player Skills");
                StartCoroutine(skillManager.GetComponent<PlayerSkills>().RunBehemothSkillE());

            }
        }
    }

    void Update()
    {
        UpdateRotationTarget();

        switch (currentState)
        {
            case PlayerState.Idle:
                break;

            case PlayerState.Moving:
                MovePlayer();
                TerrainGravity();
                break;

            case PlayerState.Attacking:
                MovePlayer();
                TerrainGravity();
                UpdateBasicAttackHitboxPosition();
                break;

            case PlayerState.Knockback:
                break;
        }
    }

    public void StateCheck()
    {
        if (move.magnitude == 0)
        {
            ChangeState(PlayerState.Idle);
        }
        else
        {
            ChangeState(PlayerState.Moving);
        }
    }

    public void TerrainGravity(){
        Vector3 position = charControl.transform.position;

        // Get the terrain height at the character's current position (X, Z)
        float terrainHeight = Terrain.activeTerrain.SampleHeight(position);

        // Set the character's Y position to match the terrain height + 1, more if berserk
        position.y = terrainHeight + 2.7f;

        charControl.transform.position = position;
    }

    public void UpdateRotationTarget()
    {
        //Create a plane on the player's position
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(mouseLook);

        //Get mouse pointer position
        if (playerPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            rotationTarget = targetPoint;
        }

        //Create vector from player position to mouse pointer
        lookPos = rotationTarget - transform.position;
        lookPos.y = 0;

        //Rotate player toward mouse pointer
        if (lookPos.magnitude > minDistanceToLook)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f);
        }
    }

    private void UpdateBasicAttackHitboxPosition()
    {
        // Get the BoxCollider's world position, size, and rotation
        worldCenter = basicAttackHitbox.transform.TransformPoint(basicAttackHitbox.center);
        worldSize = Vector3.Scale(basicAttackHitbox.size, basicAttackHitbox.transform.lossyScale) / 2;
    }

    public void MovePlayer()
    {
        Vector3 moveDirection = new Vector3(move.x, 0f, move.y) * stats.speed;
        Vector3 currentVelocity = Vector3.zero;
        // Smooth Acceleration and Deceleration
        currentVelocity = Vector3.Lerp(currentVelocity, moveDirection, (moveDirection.magnitude > 0 ? acceleration : deceleration) * Time.deltaTime);
        if (isSkillingOrUlting)
        {
            charControl.Move(moveDirection * 0);
        }
        else if (isAttacking)
        {
            charControl.Move(((moveDirection / 5) * stats.speedMultiplier) * Time.deltaTime);
        } 
        else
        {
            charControl.Move((moveDirection) * stats.speedMultiplier * Time.deltaTime);
        }
    }

    private IEnumerator BasicAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.5f / stats.speedMultiplier); // Wait for attack duration
        BasicAttackHitboxCheck(Physics.OverlapBox(worldCenter, worldSize, basicAttackHitbox.transform.rotation));
        yield return new WaitForSeconds(0.5f / stats.speedMultiplier);
        BasicAttackHitboxCheck(Physics.OverlapBox(worldCenter, worldSize, basicAttackHitbox.transform.rotation));

        if (mouseAction.action.ReadValue<float>() > 0)
        {
            basicAttackCoroutine = StartCoroutine(BasicAttack());
        }
        else
        {
            isAttacking = false;
            basicAttackCoroutine = null;

            StateCheck();
        }
    }

    private void BasicAttackHitboxCheck(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<EnemyMob>(out var mob))
            {
                mob.TakeDamage(PlayerStats.Instance.basicAttackDamage / 2);
            }
        }
    }

    public void CollisionToggle()
    {
        foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
        {
            if (col.transform != transform)  // Avoid disabling the parent collider
            {
                col.enabled = !col.enabled;
            }
        }
    }

    public void Heal(float percent)
    {
        percent *= 0.01f;

        float currentHealth = stats.currentHealth;
        float maxHealth = stats.maxHealth;

        float healAmount = maxHealth * percent;
        //Sets the current health as the smaller number between the current health + healed amount
        //and the max possible health
        stats.currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        if (stats.IsMaxHealth())
        {
            EventManager.Instance.InvokeOnFullHealth();
        }
    }

    public void TakeDamage(float damage)
    {
        if (PlayerStats.Instance.hasMudArmor)
        {
            stats.currentHealth -= damage / 1.5f;
        } 
        else
        {
            stats.currentHealth -= damage;
        }

        EventManager.Instance.InvokeOnTakingDamage();
        if (stats.currentHealth < 0)
        {
            Debug.Log("Dead");
            //trigger death cutscene
        }
    }

    public void ChangeState(PlayerState newState) //Logic for entering states (e.g. playing animations)
    {
        currentState = newState;

        switch (newState)
        {
            case PlayerState.Idle:
                // Enter Idle logic
                isAttacking = false;
                Debug.Log("Idle State");
                break;

            case PlayerState.Moving:
                // Enter Moving logic
                isAttacking = false;
                Debug.Log("Moving State");
                break;

            case PlayerState.Attacking:
                // Enter Attacking logic
                Debug.Log("Attack State");
                break;

            case PlayerState.Knockback:
                // Enter Knockback logic
                isAttacking = false;
                Debug.Log("Knockback State");
                break;
        }
    }
}
