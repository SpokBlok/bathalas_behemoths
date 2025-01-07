using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathalasBlessing : BaseSkill
{
    // Start is called before the first frame update
    void Start()
    {
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
        yield return null;
    }

    //subscribe this function to an event that gets called when entering the base and maybe leaving too
    public void RechargeUsages()
    {
        oneTimeUseAvailable = true;
    }
}
