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
        isHealHash = Animator.StringToHash("isHealing");
        player = GameObject.FindWithTag("Player");
        maxCharges = 3;
        cooldown = 40;
    }

    public override IEnumerator RunSkill()
    {
        animator.SetBool(isHealHash, true);
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerMovement>().Heal(40);
        yield return new WaitForSeconds(0.8f); //Charge up time, animation of heal
        animator.SetBool(isHealHash, false);
        yield return null;
    }
}
