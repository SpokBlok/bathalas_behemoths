using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : BaseSkill
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        maxCharges = 3;
        cooldown = 40;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator RunSkill()
    {
        Debug.Log("SKILL ACTIVATED");
        player.GetComponent<PlayerMovement>().Heal(40);
        yield return null;
    }
}
