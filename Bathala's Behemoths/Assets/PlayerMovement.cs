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

    // Movement variables
    private Vector2 move;
    public float speed = 3f;

    // Knockback variables
    public float pushBackDuration = 0.5f;
    public float pushBackForce = 10f;

    // Character Conroller
    private CharacterController charControl;
    private Collider playerCollider;

    // FSM State
    private PlayerState currentState;

    private void Start()
    {
        // Get references to char controller + collider
        charControl = GetComponent<CharacterController>();
        playerCollider = GetComponent<Collider>();

        // Start with Idle state
        currentState = PlayerState.Idle;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();

        // If there is input, transition to the Moving state
        if (move != Vector2.zero && currentState != PlayerState.Knockback)
        {
            ChangeState(PlayerState.Moving);
        }
        else if (move == Vector2.zero && currentState != PlayerState.Knockback)
        {
            ChangeState(PlayerState.Idle);
        }
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    void Update()
    {
        updateRotationTarget();
        switch (currentState)
        {
            case PlayerState.Idle:
                // Idle logic here
                break;

            case PlayerState.Moving:
                movePlayer();
                break;

            case PlayerState.Attacking:
                // Handle attacking behavior
                break;

            case PlayerState.Knockback:
                // Knockback behavior is handled in coroutine (SmoothPushBack)
                break;
        }
    }

    public void updateRotationTarget()
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
        var lookPos = rotationTarget - transform.position;
        lookPos.y = 0;

        //Rotate player toward mouse pointer
        if (lookPos.magnitude > minDistanceToLook)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
        }
    }

    public void movePlayer()
    {
        Vector3 moveDirection = new Vector3(move.x, 0f, move.y) * speed;
        charControl.Move(moveDirection * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("touchie");
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
                break;

            case PlayerState.Knockback:
                // Enter Knockback logic
                Debug.Log("Knockback State");
                break;
        }
    }
}
