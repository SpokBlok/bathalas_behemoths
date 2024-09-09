using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float pushBackDuration = 0.5f;
    public float pushBackForce = 10f;
    private Vector2 move, mouseLook;
    private Vector3 rotationTarget;
    public float minDistanceToLook = 0.1f;
    private bool isBeingPushed = false;

    private CharacterController charControl;
    private Collider playerCollider;

    private void Start()
    {
        charControl = GetComponent<CharacterController>();
        playerCollider = GetComponent<Collider>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    void Update()
    {
        updateRotationTarget();
        if (!isBeingPushed)
        {
            movePlayer();
        }
        else
        {
            //capsule collider off
        }
    }

    public void updateRotationTarget()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(mouseLook);

        if (playerPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            rotationTarget = targetPoint;
        }
    }

    public void movePlayer()
    {
        var lookPos = rotationTarget - transform.position;
        lookPos.y = 0;

        Vector3 moveDirection = new Vector3(move.x, 0f, move.y) * speed;
        charControl.Move(moveDirection * Time.deltaTime);

        if (lookPos.magnitude > minDistanceToLook)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookPos);
            transform.rotation =Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
        }
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
        isBeingPushed = true;
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
        isBeingPushed = false;
    }
}
