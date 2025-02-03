using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathalasBlessing : BaseSkill
{
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        maxCharges = 0;
        cooldown = 0;
        oneTimeUseAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
        playerScript.isBerserk = true;          //wont actually grow twice in size in final, just to see effect 
        playerScript.transform.localScale *= 2;
        PlayerStats.Instance.speedMultiplier = 2;

        //Also reset all equipped skills and cooldowns

        yield return new WaitForSeconds(10);
        playerScript.transform.localScale *= 0.5f;
        PlayerStats.Instance.speedMultiplier = 1;
        playerScript.isBerserk = false;
        yield return null;
    }

    //subscribe this function to an event that gets called when entering the base and maybe leaving too
    public void RechargeUsages()
    {
        oneTimeUseAvailable = true;
    }
}
