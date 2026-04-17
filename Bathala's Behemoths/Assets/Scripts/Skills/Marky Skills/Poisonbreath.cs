using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

public class Poisonbreath : BaseSkill
{
    public Animator animator;
    int isPoisonbreathHash;

    public GameObject projectilePrefab;
    private PlayerMovement playerMovement;
    private ProjectileScript projectileScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();
        // isPoisonbreathHash = Animator.StringToHash("isPoisonbreath");
        playerMovement = player.GetComponent<PlayerMovement>();
        maxCharges = 2;
        cooldown = 16;
        skillCode = 3;
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();

        // animator.SetBool(isPoisonbreathHash, true);
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement.basicAttackCoroutine != null)
        {
            StopCoroutine(playerMovement.basicAttackCoroutine);
            playerMovement.basicAttackCoroutine = null;
        }     
        playerMovement.ChangeState(PlayerState.Idle);
        yield return new WaitForSeconds(1); //Charge up time, animation of making mudball
        GameObject projectile = Instantiate(projectilePrefab, player.transform.position, Quaternion.identity);
        projectileScript = projectile.GetComponent<ProjectileScript>();
        StartCoroutine(projectileScript.Move(playerMovement.forwardDirection.normalized));
        playerMovement.StateCheck();
        // animator.SetBool(isPoisonbreathHash, false);
    }
}
