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

    //Basic attack variables
    private Transform leftHook;
    private Transform rightHook;
    private Transform leftAnchor;
    private Transform rightAnchor;

    // FSM State
    private PlayerState currentState;

    //Projectile Prefab
    public GameObject projectilePrefab;

    //Berserk Ult bool
    public bool isBerserk;

    private PlayerStats stats;

    public Coroutine basicAttackCoroutine;

    private void Start()
    {
        // Get references to char controller + collider
        charControl = GetComponent<CharacterController>();

        //Reference to basic attack objects
        leftHook = gameObject.transform.Find("Basic Attack/Left Hook");
        rightHook = gameObject.transform.Find("Basic Attack/Right Hook");
        leftAnchor = gameObject.transform.Find("Basic Attack/Left Anchor");
        rightAnchor = gameObject.transform.Find("Basic Attack/Right Anchor");

        if (leftAnchor == null)
        {
            Debug.Log("Error, no basic attack left hook");
        }

        // Start with Idle state
        currentState = PlayerState.Idle;

        //Starting alignment with terrain
        TerrainGravity();

        stats = PlayerStats.Instance;

        if (stats.introDone && stats.outdoorsScene)
        {
            gameObject.transform.position = new Vector3(313.5f, 23.52272f, 195.11f);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();

        // Only act on the input when it is performed
        // If there is input, transition to the Moving state
        if (context.performed)
        {
            if (move != Vector2.zero && currentState != PlayerState.Moving && currentState != PlayerState.Knockback)
            {
                ChangeState(PlayerState.Moving);
            }
        }
        else if (context.canceled)
        {
            if (currentState != PlayerState.Idle && currentState != PlayerState.Knockback)
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
            Debug.Log("Click");
            if (context.performed && !isAttacking)
            {
                Debug.Log("Attack!");
                ChangeState(PlayerState.Attacking);
                basicAttackCoroutine = StartCoroutine(BasicAttack());
            }
        }
    }

    public void OnMCSkillTrigger(InputAction.CallbackContext context) {
        
        if (context.performed)
        {
            Debug.Log("Right Click");
            if (context.performed && !isAttacking)
            {
                Debug.Log("Skill!");
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
        if (isBerserk)
        {
            position.y = terrainHeight + 2.2f;
        }
        else
        {
            position.y = terrainHeight + 1.2f;
        }

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

    public void MovePlayer()
    {
        Vector3 moveDirection = new Vector3(move.x, 0f, move.y) * stats.speed;
        if (isSkillingOrUlting)
        {
            charControl.Move(moveDirection * 0);
        }
        else if (isAttacking)
        {
            charControl.Move(((moveDirection / 3) * stats.speedMultiplier) * Time.deltaTime);
        } else
        {
            charControl.Move((moveDirection) * stats.speedMultiplier * Time.deltaTime);
        }
    }

    public void Knockback(GameObject hit)
    {
        if (hit.CompareTag("Enemy"))
        {
            if (isBerserk)
            {
                hit.GetComponent<EnemyMob>().takeDamage(stats.basicAttackDamage);
            }
            // Calculate direction to push player away from enemy`, Player - Enemy position
            Vector3 direction = new Vector3(transform.position.x - hit.transform.position.x, 0, transform.position.z - hit.transform.position.z);
            StartCoroutine(SmoothPushBack(direction.normalized));
        }
        else if (hit.CompareTag("Boss"))
        {
            if (isBerserk)
            {
                hit.GetComponent<MarkupoScript>().takeDamage(stats.basicAttackDamage);
            }
            // Calculate direction to push player away from enemy`, Player - Enemy position
            Vector3 direction = new Vector3(transform.position.x - hit.transform.position.x, 0, transform.position.z - hit.transform.position.z);
            StartCoroutine(SmoothPushBack(direction.normalized));
        }
    }

    private IEnumerator SmoothPushBack(Vector3 direction)
    {
        TakeDamage(15);
        ChangeState(PlayerState.Knockback); // Change to Knockback state
        gameObject.layer = LayerMask.NameToLayer("Pushback");
        //playerInput.actions["BasicAttack"].Disable();
        //playerInput.actions["SkillTrigger"].Disable();
        //playerInput.actions["UltTrigger"].Disable();
        float elapsedTime = 0f;

        while (elapsedTime < pushBackDuration)
        {
            // Apply push-back force gradually
            charControl.Move(direction * pushBackForce * Time.deltaTime);
            TerrainGravity();
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        //playerInput.actions.Enable();
        gameObject.layer = LayerMask.NameToLayer("Default");
        ChangeState(PlayerState.Idle); // Return to Idle state after knockback
    }

    private IEnumerator BasicAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.1f / stats.speedMultiplier); // Wait for attack duration
        yield return StartCoroutine(MoveToPosition(rightHook, 0.4f / stats.speedMultiplier, true));
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(MoveToPosition(leftHook, 0.4f / stats.speedMultiplier, false));
        yield return new WaitForSeconds(0.1f / stats.speedMultiplier);
        leftHook.position = leftAnchor.position;
        rightHook.position = rightAnchor.position;
        leftHook.GetComponent<BasicAttackTrigger>().enemyHit = null;
        rightHook.GetComponent<BasicAttackTrigger>().enemyHit = null; //From preventing double damage trigger

        if (move.magnitude == 0)
        {
            ChangeState(PlayerState.Idle);
        }
        else
        {
            ChangeState(PlayerState.Moving);
        }
        isAttacking = false;
        StopCoroutine(basicAttackCoroutine);
        basicAttackCoroutine = null;
        yield break;
    }

    private IEnumerator MoveToPosition(Transform obj, float duration, bool toLeft)
    {
        float elapsedTime = 0f;
        Vector3 targetPosition;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Dynamically calculate the left or rightmost point
            if (toLeft)
            {
                targetPosition = leftAnchor.position;
            } 
            else
            {
                targetPosition = rightAnchor.position;
            }
            
            // Interpolate towards the updated target
            obj.position = Vector3.Lerp(obj.position, targetPosition, elapsedTime / duration);

            yield return null;
        }     
    }

    public IEnumerator SkillTrigger()
    {
        //if (stats.dashSkillEquipped)
        //{
        //    isAttacking = true;
        //    isSkillingOrUlting = true;
        //    yield return StartCoroutine(DashSkill());
        //}
        //else if (stats.rangedSkillEquipped)
        //{
        //    isAttacking = true;
        //    isSkillingOrUlting = true;
        //    yield return new WaitForSeconds(1.0f); // Skill charge up duration
        //    yield return StartCoroutine(RangedSkill(lookPos.normalized));
        //}

        Debug.Log("Skill end");
        isSkillingOrUlting = false;
        isAttacking = false;


        if (move.magnitude == 0)
        {
            ChangeState(PlayerState.Idle);
        }
        else
        {
            ChangeState(PlayerState.Moving);
        }

        yield break;
    }

    private IEnumerator DashSkill()
    {
        Debug.Log("Dashing");
        //Disable children colliders
        CollisionToggle();
        gameObject.layer = LayerMask.NameToLayer("Pushback");

        Vector3 direction = -lookPos.normalized;
        if (move.magnitude > 0)
        {
            direction = move.normalized;
            direction.z = direction.y;
            direction.y = 0;
        }
        
        //playerInput.actions["Move"].Disable(); //Prevent moving while dashing
        float dashDuration = 0.4f; // Time for the dash

        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            charControl.Move(direction * stats.speed * Time.deltaTime * 2.5f);
            TerrainGravity();
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        //playerInput.actions["Move"].Enable();
        EnemyTriggerCheck();
        gameObject.layer = LayerMask.NameToLayer("Default");
        //Enable children colliders
        CollisionToggle();
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

    public void EnemyTriggerCheck()
    {
        EventManager.Instance.InvokeOnDashComplete();
        //Need to update for new enemy mob types/scripts
        //EnemyRadiusTrigger[] enemyRadiusTriggers = GameObject.FindObjectsOfType<EnemyRadiusTrigger>();
        //foreach (EnemyRadiusTrigger trigger in enemyRadiusTriggers)
        //{
        //    trigger.TriggerCheck();
        //}
    }

    public IEnumerator RangedSkill(Vector3 target)
    {
        GameObject projectile = Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
        StartCoroutine(projectileScript.Move(target));
        yield break;
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

    public IEnumerator UltTrigger()
    {
        //if (stats.rangedUltEquipped)
        //{
        //    isAttacking = true;
        //    isSkillingOrUlting = true;
        //    yield return new WaitForSeconds(1.5f); // Ult charge up duration
        //    yield return StartCoroutine(RangedUlt());
        //}
        //else if (stats.berserkUltEquipped)
        //{
        //    yield return StartCoroutine(BerserkUlt());
        //}

        isSkillingOrUlting = false;
        isAttacking = false;


        if (move.magnitude == 0)
        {
            ChangeState(PlayerState.Idle);
        }
        else
        {
            ChangeState(PlayerState.Moving);
        }

        yield break;
    }

    public IEnumerator RangedUlt()
    {
        Vector3[] directions = new Vector3[]{
            new Vector3(0, 0, 1),                                // North
            new Vector3(1, 0, 1).normalized,                     // Northeast
            new Vector3(1, 0, 0),                                // East
            new Vector3(1, 0, -1).normalized,                    // Southeast
            new Vector3(0, 0, -1),                               // South
            new Vector3(-1, 0, -1).normalized,                   // Southwest
            new Vector3(-1, 0, 0),                               // West
            new Vector3(-1, 0, 1).normalized                     // Northwest
        };

        foreach (Vector3 direction in directions)
        {
            StartCoroutine(RangedSkill(direction));
        }
        yield break;
    }

    public IEnumerator BerserkUlt()
    {
        isBerserk = true;
        transform.localScale *= 2;
        float elapsedTime = 0f;
        while (elapsedTime < 5)
        {
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        transform.localScale *= 0.5f;
        isBerserk = false;
        yield break;
    }

    public void TakeDamage(int damage)
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
