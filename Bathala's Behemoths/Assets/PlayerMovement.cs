using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Vector2 move, mouseLook;
    private Vector3 rotationTarget;
    public float minDistanceToLook = 0.1f; // Minimum distance to start rotating towards mouse pointer

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateRotationTarget();
        movePlayer();
    }

    public void updateRotationTarget()
    {
        // Create a plane at the player's position, facing up
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(mouseLook);

        // Calculate where the ray hits the plane
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

        Vector3 aimDirection = new Vector3(rotationTarget.x, 0, rotationTarget.z);


        if (lookPos.magnitude > minDistanceToLook)
        {
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        Vector3 movement = new Vector3(move.x, 0f, move.y);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
