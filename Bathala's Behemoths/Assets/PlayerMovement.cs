using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    private Vector2 move, mouseLook;
    private Vector3 rotationTarget;
    public float minDistanceToLook = 0.1f; // Minimum distance to start rotating towards mouse pointer

    private CharacterController charControl;

    private void Start()
    {
        charControl = GetComponent<CharacterController>();
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
        movePlayer();
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
            Debug.Log("touchie enemy");
            //Debug.Log("touchie");
            //Vector3 direction = new Vector3(transform.position.x - collision.transform.position.x, 0, transform.position.z - collision.transform.position.z);
            //rb.MovePosition(transform.position + direction.normalized * 2); // Use Rigidbody.MovePosition to handle movement
        }
    }
}
