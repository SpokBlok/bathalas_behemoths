using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class MusicalFlute : BaseSkill
{
    private PlayerMovement playerMovement;

    public Animator animator;
    int isFluteHash;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();

        isFluteHash = Animator.StringToHash("isFlute");
        playerMovement = player.GetComponent<PlayerMovement>();
        maxCharges = 2;
        cooldown = 40;
        skillCode = 3;
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();

        if(PlayerStats.Instance.markyScene)
        {
            MarkupoScript marky = GameObject.FindWithTag("Markupo").GetComponent<MarkupoScript>();
            marky.GetFluteStunned();
        }
        
        animator.SetBool(isFluteHash, true);
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement.basicAttackCoroutine != null)
        {
            StopCoroutine(playerMovement.basicAttackCoroutine);
            playerMovement.basicAttackCoroutine = null;
        }
        playerMovement.ChangeState(PlayerState.Idle);
        GameObject.FindWithTag("Player Input").GetComponent<PlayerInput>().actions["Move"].Disable();
        yield return new WaitForSeconds(1.5f); //Charge up time, animation of flute playing
        GameObject.FindWithTag("Player Input").GetComponent<PlayerInput>().actions["Move"].Enable();
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, 10f);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<EnemyMob>(out var mob))
            {
                Debug.Log("Stunned");
                StartCoroutine(mob.Stun(5));
            }
        }
        playerMovement.StateCheck();
        
        animator.SetBool(isFluteHash, false);
    }
}
