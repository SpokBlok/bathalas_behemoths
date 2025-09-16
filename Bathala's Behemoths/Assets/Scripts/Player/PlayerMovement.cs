using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    Idle,
    Moving,
    Attacking,
    Knockback
}

public class PlayerMovement : MonoBehaviour
{
    // [SerializeField] Transform cam;
    // private Vector3 camForward;
    // private Vector3 camRight;
    // private Vector3 forwardRelative;
    // private Vector3 rightRelative;
    // private Vector3 moveDir;

    // Look rotation variables
    private Vector2 mouseLook;
    private Vector3 rotationTarget;
    public float minDistanceToLook = 0.1f;
    public Vector3 lookPos;
    public Vector3 forwardDirection;

    public Animator animator;
    public Coroutine enterFightingStance;
    public Coroutine exitFightingStance;
    bool isExitingFightStance;
    private float fightStanceValue = 0.0f;
    int basicAttackHash;
    Collider[] objsInRange;

    // Movement variables
    public Vector2 move;
    public Vector3 currentVelocity = Vector3.zero;
    
    // Camera Controls
    private float yaw = 0f;
    private float pitch = 0f;
    [SerializeField] private float mouseSensitivity = 100f;

    // Knockback variables
    public float pushBackDuration = 0.5f;
    public float pushBackForce = 10f;

    //Attack variables
    public bool isAttacking = false;
    public bool attackOngoing = false;
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

    private PauseSystem pauseSystem;
    public UICanvas ui;
    public JournalScript journal;
    public AudioClip basicAttackAudio;

    // Variable caching for performance
    private Vector3 position;
    private Terrain myTerrain;
    private float terrainHeight;
    private Vector3 moveDirection;
    private Vector3 forward;
    private Vector3 right;
    private float mouseX;
    private float mouseY;
    private Vector3 forwardRelativeInput;
    private Vector3 rightRelativeInput;
    private Vector3 cameraRelativeMove;
    private float currentHealth;
    private float maxHealth;
    private float healAmount;


    private Camera mainCamera;

    private void Start()
    {
        if (!QuestState.Instance.menuActive)
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }

        yaw = transform.eulerAngles.y;
        pitch = 0f;

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0);

        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        if (modelRenderer == null)
        {
            modelRenderer = msModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        }
        ui = GameObject.FindObjectOfType<UICanvas>();
        pauseSystem = FindObjectOfType<PauseSystem>();
        journal = FindObjectsOfType<JournalScript>(true)[0];

        // Get references to char controller + collider
        charControl = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        // Debug.Log("animator found: " + (animator != null ? animator : "NULL"));

        stats = PlayerStats.Instance;

        basicAttackHash = Animator.StringToHash("basicAttacking");

        // Start with Idle state
        currentState = PlayerState.Idle;

        //Starting alignment with terrain
        TerrainGravity();

        mainCamera = Camera.main;


        basicAttackHitbox = transform.Find("Basic Attack/Basic Attack Hitbox").GetComponent<BoxCollider>();

        if (stats.tammyScene && !stats.outdoorsScene)
        {
            gameObject.transform.position = new Vector3(463.2f, 175.2f, 100f);
        }
        else if (stats.markyScene && !stats.outdoorsScene)
        {
            gameObject.transform.position = new Vector3(240f, 20f, 100f);
        }
        else if (stats.tammyScene && stats.outdoorsScene)
        {
            gameObject.transform.position = new Vector3(676f, 62.5f, 1336.6f);
            stats.tammyScene = false;
        }
        else if (stats.markyScene && stats.outdoorsScene)
        {
            gameObject.transform.position = new Vector3(997f, 70.2f, 276.5f);
            stats.markyScene = false;
        }
        else if (stats.introDone && stats.outdoorsScene)
        {
            gameObject.transform.position = new Vector3(632.2f, 59.5f, 285.138f);
        }
        else if (stats.outdoorsScene)
        {
            gameObject.transform.position = new Vector3(876.24f, 79.24f, 72.68f);
        }
        else if (stats.ruinsScene)
        {
            ui.gameObject.SetActive(true);
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
        forwardDirection = mainCamera.transform.forward;
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
        if (context.performed && !QuestState.Instance.pausedForDialogue)
        {
            if (context.performed && !isAttacking && stats.introDone && (stats.outdoorsScene || stats.tammyScene || stats.markyScene))
            {
                ChangeState(PlayerState.Attacking);
                attackOngoing = true;
                basicAttackCoroutine = StartCoroutine(BasicAttack());
            }
        }
    }

    public void OnMCSkillTrigger(InputAction.CallbackContext context) 
    {
        // Debug.Log("Calling OnMCSkillTrigger!!!");
        if (context.performed)
        {
            if (context.performed && !isAttacking && stats.introDone && (stats.outdoorsScene || stats.tammyScene || stats.markyScene))
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
            if (context.performed && !isAttacking && stats.introDone && (stats.outdoorsScene || stats.tammyScene || stats.markyScene))
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
            if (context.performed && !isAttacking && stats.introDone && (stats.outdoorsScene || stats.tammyScene || stats.markyScene))
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
        if(QuestState.Instance.pausedForDialogue) { return; }

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
        position = charControl.transform.position;

        // Get the terrain height at the character's current position (X, Z)
        myTerrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        terrainHeight = myTerrain.SampleHeight(position);

        // Set the character's Y position to match the terrain height + 1, more if berserk
        position.y = terrainHeight + 2.1f;

        charControl.transform.position = position;
    }

    public void UpdateRotationTarget()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -60f, 60f); // Limit vertical look angle

        // Rotate player body horizontally (yaw)
        // playerBody.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Rotate camera vertically (pitch)
        // virtualCamTransform.localRotation = Quaternion.Euler(pitch, yaw, 0f);

        // Rotate Player object (the parent obj) to reflect the mouseLook direction
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }

    private void UpdateBasicAttackHitboxPosition()
    {
        // Get the BoxCollider's world position, size, and rotation
        worldCenter = basicAttackHitbox.transform.TransformPoint(basicAttackHitbox.center);
        worldSize = Vector3.Scale(basicAttackHitbox.size, basicAttackHitbox.transform.lossyScale) / 2;
    }

    public void MovePlayer()
    {
        // Old movement code
        // Vector3 moveDirection = new Vector3(move.x, 0f, move.y) * stats.speed;

        // New movement code

        // Get Main Camera's normalized directional vectors
        forward = mainCamera.transform.forward;
        right = mainCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        // Create cam-direction-relative input vectors
        forwardRelativeInput = move.y * forward;
        rightRelativeInput = move.x * right;

        // Apply camera relative movement
        cameraRelativeMove = forwardRelativeInput + rightRelativeInput;
        moveDirection = cameraRelativeMove.normalized * stats.speed;
        lookPos = moveDirection;

        // Debug.Log(moveDirection);

        // Smooth Acceleration and Deceleration
        currentVelocity = Vector3.Lerp(currentVelocity, moveDirection, (moveDirection.magnitude > 0 ? acceleration : deceleration) * Time.deltaTime);
        if (isSkillingOrUlting)
        {
            charControl.Move(currentVelocity * 0);
        }
        else if (isAttacking)
        {
            charControl.Move(((currentVelocity / 5) * stats.speedMultiplier) * Time.deltaTime);
        } 
        else
        {
            charControl.Move((currentVelocity) * stats.speedMultiplier * Time.deltaTime);
        }
    }

    private IEnumerator PlayBAAudio()
    {
        AudioSource.PlayClipAtPoint(basicAttackAudio, mainCamera.transform.position + mainCamera.transform.forward * 2f, 1f);

        yield return null;
    }

    private IEnumerator BasicAttack()
    {
        isAttacking = true;

        animator.SetBool(basicAttackHash, true);
        StartCoroutine(PlayBAAudio());
        yield return new WaitForSeconds(0.47f / stats.speedMultiplier); // Wait for attack duration
        BasicAttackHitboxCheck(Physics.OverlapBox(worldCenter, worldSize, basicAttackHitbox.transform.rotation));
        yield return new WaitForSeconds(0.47f / stats.speedMultiplier);
        BasicAttackHitboxCheck(Physics.OverlapBox(worldCenter, worldSize, basicAttackHitbox.transform.rotation));

        if (mouseAction.action.ReadValue<float>() > 0)
        {
            basicAttackCoroutine = StartCoroutine(BasicAttack());
        }
        else
        {
            isAttacking = false;
            basicAttackCoroutine = null;
            animator.SetBool(basicAttackHash, false);

            StateCheck();
        }
    }

    private void BasicAttackHitboxCheck(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<EnemyMob>(out var mob))
            {
                mob.TakeDamage(stats.basicAttackDamage / 2);
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

    public void OpenJournal(InputAction.CallbackContext context)
    {
        journal.OnInteract();
    }

    public void Heal(float percent)
    {
        percent *= 0.01f;

        currentHealth = stats.currentHealth;
        maxHealth = stats.maxHealth;

        healAmount = maxHealth * percent;
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

        if (stats.hasMudArmor)
        {
            stats.currentHealth -= damage / 1.5f;
        } 
        else
        {
            stats.currentHealth -= damage;
        }

        EventManager.Instance.InvokeOnTakingDamage();
        if (stats.currentHealth <= 0)
        {
            Debug.Log("Dead");
            //trigger death cutscene
            stats.dead = true;

            // // Go to DeathScreen
            // ui.gameObject.SetActive(false);
            stats.tammyScene = false;
            stats.markyScene = false;
            QuestState.Instance.pausedForDialogue = true;

            QuestState.Instance.menuActive = true;
            SceneManager.LoadScene("DeathScreen");
        }
    }

    IEnumerator EnterFightingStance()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        float startValue = 0.0f;
        float endValue = 0.69f;

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
        exitFightingStance = null;
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
                // Debug.Log("Idle State");
                // Debug.Log("Objects inside: " + objsInRange.Length);
                if((fightStanceValue >= 0.69f && !enemyFound && exitFightingStance == null))
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
                if((fightStanceValue >= 0.69f && !enemyFound && exitFightingStance == null))
                {
                    exitFightingStance = StartCoroutine(ExitFightingStance());
                    enemyFound = false;
                }
                break;

            case PlayerState.Attacking:
                // Enter Attacking logic
                // Debug.Log("Attack State");
                if(exitFightingStance != null)
                {
                    StopCoroutine(exitFightingStance);
                    exitFightingStance = null;
                }
                if(fightStanceValue == 0)
                {
                    enterFightingStance = StartCoroutine(EnterFightingStance());
                }
                // Debug.Log("Objects inside: " + objsInRange.Length);
                if((fightStanceValue >= 0.69f && !enemyFound && exitFightingStance == null))
                {
                    exitFightingStance = StartCoroutine(ExitFightingStance());
                    enemyFound = false;
                }
                break;

            case PlayerState.Knockback:
                // Enter Knockback logic
                isAttacking = false;
                // Debug.Log("Knockback State");
                // Debug.Log("Objects inside: " + objsInRange.Length);
                if((fightStanceValue >= 0.69f && !enemyFound && exitFightingStance == null))
                {
                    exitFightingStance = StartCoroutine(ExitFightingStance());
                    enemyFound = false;
                }
                break;
        }
    }
}
