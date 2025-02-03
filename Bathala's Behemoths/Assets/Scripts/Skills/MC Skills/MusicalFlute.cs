using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class MusicalFlute : BaseSkill
{
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        maxCharges = 1;
        cooldown = 40;
    }

    public override IEnumerator RunSkill()
    {
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
    }
}
