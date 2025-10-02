using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protect : BaseSkill
{
    // Start is called before the first frame update
    void Start()
    {
        maxCharges = 1;
        cooldown = 15;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        PlayerStats.Instance.hasProtect = true;
        PlayerStats.Instance.speedMultiplier /= 2;
        yield return new WaitForSeconds(5.0f); //Duration of skill
        PlayerStats.Instance.hasProtect = false;
        PlayerStats.Instance.speedMultiplier *= 2;  
    }
}
