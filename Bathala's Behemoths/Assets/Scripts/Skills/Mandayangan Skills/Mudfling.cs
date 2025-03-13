using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

public class Mudfling : BaseSkill
{
    public Animator animator;
    int isMudflingHash;

    public GameObject projectilePrefab;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();
        isMudflingHash = Animator.StringToHash("isMudfling");
        playerMovement = player.GetComponent<PlayerMovement>();
        maxCharges = 2;
        cooldown = 16;
        skillCode = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();

        animator.SetBool(isMudflingHash, true);
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
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
        StartCoroutine(projectileScript.Move(player.GetComponent<PlayerMovement>().lookPos.normalized));
        playerMovement.StateCheck();
        animator.SetBool(isMudflingHash, false);
    }
}
