using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : BaseSkill
{
    private PlayerInput input;
    public Animator animator;
    int isDashingHash;

    // Start is called before the first frame update
    void Start()
    {
        isDashingHash = Animator.StringToHash("isDashing");

        player = GameObject.FindWithTag("Player");
        input = GameObject.FindWithTag("Player Input").GetComponent<PlayerInput>();
        maxCharges = 3;
        cooldown = 6;
    }

    public override IEnumerator RunSkill()
    {
        animator.SetBool(isDashingHash, true);
        player = GameObject.FindWithTag("Player");
        input = GameObject.FindWithTag("Player Input").GetComponent<PlayerInput>();
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        Debug.Log("Dashing");
        //Disable children colliders
        playerMovement.CollisionToggle();
        gameObject.layer = LayerMask.NameToLayer("Pushback");

        Vector3 direction = -playerMovement.lookPos.normalized;
        if (playerMovement.move.magnitude > 0)
        {
            direction = playerMovement.move.normalized;
            direction.z = direction.y;
            direction.y = 0;
        }

        input.actions["Move"].Disable(); //Prevent moving while dashing
        float dashDuration = 0.4f; // Time for the dash

        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            playerMovement.charControl.Move(direction * PlayerStats.Instance.speed * Time.deltaTime * 2.5f);
            playerMovement.TerrainGravity();
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        input.actions["Move"].Enable();
        EventManager.Instance.InvokeOnDashComplete();
        gameObject.layer = LayerMask.NameToLayer("Default");
        //Enable children colliders
        playerMovement.CollisionToggle();
        
        animator.SetBool(isDashingHash, false);
    }
}
