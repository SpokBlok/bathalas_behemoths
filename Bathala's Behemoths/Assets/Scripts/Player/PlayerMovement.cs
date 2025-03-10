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

    public Animator animator;
    public Coroutine enterFightingStance;
    public Coroutine exitFightingStance;
    bool isExitingFightStance;
    private float fightStanceValue = 0.0f;
    int basicAttackHash;
    Collider[] objsInRange;

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

    public MSAnimController msModel;
    public SkinnedMeshRenderer[] modelRenderer;
    public Coroutine takingDamage;

    private void Start()
    {
        if (modelRenderer == null)
        {
            modelRenderer = msModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        // Get references to char controller + collider
        charControl = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        Debug.Log("animator found: " + (animator != null ? animator : "NULL"));

        stats = PlayerStats.Instance;

        basicAttackHash = Animator.StringToHash("basicAttacking");

        // Start with Idle state
        currentState = PlayerState.Idle;

        //Starting alignment with terrain
        TerrainGravity();


        basicAttackHitbox = transform.Find("Basic Attack/Basic Attack Hitbox").GetComponent<BoxCollider>();

        if (stats.introDone && stats.outdoorsScene)
        {
            gameObject.transform.position = new Vector3(632.2f, 59.5f, 285.138f);
        }
        else if(stats.outdoorsScene)
        {
            gameObject.transform.position = new Vector3(876.24f, 79.24f, 72.68f);
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

    // Detects any Enemy entering the sphere collider and if so, activates combat layer to blend with movement layer
    private void OnTriggerEnter(Collider other)
    {
        if (fightStanceValue == 0 && other.CompareTag("Enemy"))
        {
            enterFightingStance = StartCoroutine(EnterFightingStance());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (fightStanceValue == 0 && other.CompareTag("Enemy"))
        {
            enterFightingStance = StartCoroutine(EnterFightingStance());
        }
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

            StateCheck();
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

            StateCheck();
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

            StateCheck();
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
        else if(!isAttacking && move.magnitude != 0)
        {
            ChangeState(PlayerState.Moving);
        }
        else
        {
            ChangeState(PlayerState.Moving);
        }
    }

    public void TerrainGravity(){
        Vector3 position = charControl.transform.position;

        // Get the terrain height at the character's current position (X, Z)
        Terrain myTerrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        float terrainHeight = myTerrain.SampleHeight(position);

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
        animator.SetBool(basicAttackHash, true);
        yield return new WaitForSeconds(0.5f / stats.speedMultiplier); // Wait for attack duration
        BasicAttackHitboxCheck(Physics.OverlapBox(worldCenter, worldSize, basicAttackHitbox.transform.rotation));
        yield return new WaitForSeconds(0.5f / stats.speedMultiplier);
        BasicAttackHitboxCheck(Physics.OverlapBox(worldCenter, worldSize, basicAttackHitbox.transform.rotation));
        animator.SetBool(basicAttackHash, false);

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
        if(takingDamage == null)
        {
            takingDamage = StartCoroutine(SwitchToDamagedTex());
        }
        else if(takingDamage != null)
        {
            StopCoroutine(takingDamage);
            takingDamage = StartCoroutine(SwitchToDamagedTex());
        }

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

    IEnumerator EnterFightingStance()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        float startValue = 0.0f;
        float endValue = 0.69f;
        Debug.Log("Starting Fight Stance");

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            fightStanceValue = Mathf.Lerp(startValue, endValue, t);
            animator.SetLayerWeight(1, fightStanceValue);
            yield return null;
        }
        
        fightStanceValue = endValue; // Ensure it fully reaches the target
        animator.SetLayerWeight(1, fightStanceValue);
    }

    IEnumerator ExitFightingStance()
    {
        yield return new WaitForSeconds(3.0f);
        isExitingFightStance = true;
        Debug.Log("Exiting Fight Stance");

        float duration = 0.5f;
        float elapsedTime = 0f;
        float startValue = 0.69f;
        float endValue = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            fightStanceValue = Mathf.Lerp(startValue, endValue, t);
            animator.SetLayerWeight(1, fightStanceValue);
            yield return null;
        }
        
        fightStanceValue = endValue; // Ensure it fully reaches the target
        animator.SetLayerWeight(1, fightStanceValue);
        isExitingFightStance = false;
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

    public void ChangeState(PlayerState newState) //Logic for entering states (e.g. playing animations)
    {
        currentState = newState;
        animator = GetComponentInChildren<Animator>();

        if(fightStanceValue > 0.0f && fightStanceValue < 0.69f)
        {
            fightStanceValue = 0.0f;
        }

        bool enemyFound = false;
        objsInRange = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);

        foreach (Collider collider in objsInRange)
        {
            if (collider.CompareTag("Enemy"))
            {
                enemyFound = true;
            }
        }

        switch (newState)
        {
            case PlayerState.Idle:
                // Enter Idle logic
                isAttacking = false;
                Debug.Log("Idle State");
                // Debug.Log("Objects inside: " + objsInRange.Length);
                if((fightStanceValue >= 0.69f && !enemyFound))
                {
                    exitFightingStance = StartCoroutine(ExitFightingStance());
                    enemyFound = false;
                }
                break;

            case PlayerState.Moving:
                // Enter Moving logic
                isAttacking = false;
                Debug.Log("Moving State");
                // Debug.Log("Objects inside: " + objsInRange.Length);
                if((fightStanceValue >= 0.69f && !enemyFound))
                {
                    exitFightingStance = StartCoroutine(ExitFightingStance());
                    enemyFound = false;
                }
                break;

            case PlayerState.Attacking:
                // Enter Attacking logic
                Debug.Log("Attack State");
                if(fightStanceValue == 0)
                {
                    enterFightingStance = StartCoroutine(EnterFightingStance());
                }
                // Debug.Log("Objects inside: " + objsInRange.Length);
                if((fightStanceValue >= 0.69f && !enemyFound))
                {
                    exitFightingStance = StartCoroutine(ExitFightingStance());
                    enemyFound = false;
                }
                if(isExitingFightStance && exitFightingStance != null)
                {
                    StopCoroutine(exitFightingStance);
                }
                break;

            case PlayerState.Knockback:
                // Enter Knockback logic
                isAttacking = false;
                Debug.Log("Knockback State");
                Debug.Log("Objects inside: " + objsInRange.Length);
                if((fightStanceValue >= 0.69f && !enemyFound))
                {
                    exitFightingStance = StartCoroutine(ExitFightingStance());
                    enemyFound = false;
                }
                break;
        }
    }
}
