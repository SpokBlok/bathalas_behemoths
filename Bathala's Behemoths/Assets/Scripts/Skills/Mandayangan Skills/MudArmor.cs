using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudArmor : BaseSkill
{

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        maxCharges = 1;
        cooldown = 40;
    }

    public override IEnumerator RunSkill()
    {
        yield return new WaitForSeconds(1f); //Skill animation
        PlayerStats.Instance.hasMudArmor = true;
        yield return new WaitForSeconds(20);
        PlayerStats.Instance.hasMudArmor = false;
    }
}
