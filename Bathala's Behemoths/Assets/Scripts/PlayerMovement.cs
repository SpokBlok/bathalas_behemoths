using System.Collections;
using System.Collections.Generic;
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
    private Vector2 move;

    // Knockback variables
    public float pushBackDuration = 0.5f;
    public float pushBackForce = 10f;

    //Attack variables
    public bool isAttacking = false;

    // Character Conroller
    private CharacterController charControl;
    private Collider playerCollider;

    //Basic attack variables
    private Collider basicAttackCollider;

    // FSM State
    private PlayerState currentState;
    private Coroutine activeCoroutine;

    //InputManager
    public PlayerInput playerInput;

    //Projectile Prefab
    public GameObject projectilePrefab;

    private void Start()
    {
        // Get references to char controller + collider
        charControl = GetComponent<CharacterController>();
        playerCollider = GetComponent<Collider>();

        //Reference to basic attack hitbox
        basicAttackCollider = transform.Find("BasicAttackHitBox").GetComponent<Collider>();
        if (basicAttackCollider == null)
        {
            Debug.Log("Error, no basic attack hitbox");
        }

        //Refernece to PlayerInput
        playerInput = GetComponent<PlayerInput>();

        // Start with Idle state
        currentState = PlayerState.Idle;

        //Starting alignment with terrain
        TerrainGravity();
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
                activeCoroutine = StartCoroutine(BasicAttack());
            }
        }
    }

    public void OnSkillTrigger(InputAction.CallbackContext context) {
        
        if (context.performed && !PlayerStats.Instance.noSkillEquipped)
        {
            Debug.Log("Right Click");
            if (context.performed && !isAttacking)
            {
                Debug.Log("Skill!");
                ChangeState(PlayerState.Attacking);
                activeCoroutine = StartCoroutine(SkillTrigger());
            }
        }
    }

    void Update()
    {
        UpdateRotationTarget();
        switch (currentState)
        {
            case PlayerState.Idle:
                // Idle logic here
                break;

            case PlayerState.Moving:
                MovePlayer();
                TerrainGravity();
                break;

            case PlayerState.Attacking:
                MovePlayer();
                break;

            case PlayerState.Knockback:
                // Knockback behavior is handled in coroutine (SmoothPushBack)
                break;
        }
    }

    public void TerrainGravity(){
        Vector3 position = charControl.transform.position;

        // Get the terrain height at the character's current position (X, Z)
        float terrainHeight = Terrain.activeTerrain.SampleHeight(position);

        // Set the character's Y position to match the terrain height + 1
        position.y = terrainHeight + 1.2f;

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
        Vector3 moveDirection = new Vector3(move.x, 0f, move.y) * PlayerStats.Instance.speed;
        charControl.Move(moveDirection * Time.deltaTime);
    }

    public void Knockback(GameObject hit)
    {
        if (hit.CompareTag("Enemy"))
        {
            // Calculate direction to push player away from enemy`, Player - Enemy position
            Vector3 direction = new Vector3(transform.position.x - hit.transform.position.x, 0, transform.position.z - hit.transform.position.z);
            StartCoroutine(SmoothPushBack(direction.normalized));
        }
    }

    private IEnumerator SmoothPushBack(Vector3 direction)
    {
        ChangeState(PlayerState.Knockback); // Change to Knockback state
        gameObject.layer = LayerMask.NameToLayer("Pushback");
        float elapsedTime = 0f;

        while (elapsedTime < pushBackDuration)
        {
            // Apply push-back force gradually
            charControl.Move(direction * pushBackForce * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        gameObject.layer = LayerMask.NameToLayer("Default");
        ChangeState(PlayerState.Idle); // Return to Idle state after knockback
    }

    private IEnumerator BasicAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.5f); // Wait for attack duration
        BasicAttackHitboxTrigger();
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;


        if (move.magnitude == 0)
        {
            ChangeState(PlayerState.Idle);
        }
        else
        {
            ChangeState(PlayerState.Moving);
        }

        activeCoroutine = null;
    }

    private void BasicAttackHitboxTrigger()
    {
        // Handle attack hitbox logic here
        Debug.Log("Hitbox check!");
        basicAttackCollider.enabled = true;

        // Define the hitbox's position and radius
        Vector3 hitboxPosition = basicAttackCollider.transform.position;
        Vector3 hitboxSize = basicAttackCollider.GetComponent<BoxCollider>().size / 2;

        // Get all colliders within the hitbox
        Collider[] hitEnemies = Physics.OverlapBox(hitboxPosition, hitboxSize, basicAttackCollider.transform.rotation);

        // Process each object inside the hitbox
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyMob enemyScript = enemy.GetComponent<EnemyMob>();
                enemyScript.takeDamage(PlayerStats.Instance.basicAttackDamage);
                Debug.Log("Enemy hit!");
            }
        }

        // Optionally deactivate the hitbox after checking
        basicAttackCollider.enabled = false;
    }

    public IEnumerator SkillTrigger()
    {
        if (PlayerStats.Instance.dashSkillEquipped)
        {
            isAttacking = true;
            StartCoroutine(DashSkill());
        }
        else if (PlayerStats.Instance.rangedSkillEquipped)
        {
            isAttacking = true;
            StartCoroutine(RangedSkill());
        }

        isAttacking = false;


        if (move.magnitude == 0)
        {
            ChangeState(PlayerState.Idle);
        }
        else
        {
            ChangeState(PlayerState.Moving);
        }

        activeCoroutine = null;
        yield break;
    }

    private IEnumerator DashSkill()
    {
        Debug.Log("Dashing");
        Vector3 direction = -lookPos.normalized;
        if (move.magnitude > 0)
        {
            direction = move.normalized;
            direction.z = direction.y;
            direction.y = 0;
        }
        playerInput.actions["Move"].Disable();//Prevent moving while dashing
        float dashDuration = 0.4f; // Time for the dash

        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            charControl.Move(direction * PlayerStats.Instance.speed * Time.deltaTime * 2.5f);
            TerrainGravity();
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        playerInput.actions["Move"].Enable();
    }

    public IEnumerator RangedSkill()
    {
        GameObject projectile = Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
        StartCoroutine(projectileScript.Move(lookPos.normalized));
        yield break;
    }

    private void ChangeState(PlayerState newState) //Logic for entering states (e.g. playing animations)
    {
        currentState = newState;

        // Optional: Handle logic when entering or leaving specific states.
        switch (newState)
        {
            case PlayerState.Idle:
                // Enter Idle logic
                Debug.Log("Idle State");
                break;

            case PlayerState.Moving:
                // Enter Moving logic
                Debug.Log("Moving State");
                break;

            case PlayerState.Attacking:
                // Enter Attacking logic
                Debug.Log("Attack State");
                break;

            case PlayerState.Knockback:
                // Enter Knockback logic
                Debug.Log("Knockback State");
                break;
        }
    }
}
