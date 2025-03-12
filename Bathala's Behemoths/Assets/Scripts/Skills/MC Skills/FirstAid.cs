using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : BaseSkill
{
    public Animator animator;
    int isHealHash;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();

        isHealHash = Animator.StringToHash("isHealing");
        maxCharges = 3;
        cooldown = 40;
        skillCode = 2;
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();
        
        animator.SetBool(isHealHash, true);
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerMovement>().Heal(40);
        yield return new WaitForSeconds(0.8f); //Charge up time, animation of heal
        animator.SetBool(isHealHash, false);
        yield return null;
    }
}
