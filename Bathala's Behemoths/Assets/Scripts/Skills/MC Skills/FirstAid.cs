using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : BaseSkill
{
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        maxCharges = 3;
        cooldown = 40;
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerMovement>().Heal(40);
        yield return null;
    }
}
