using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathalasBlessing : BaseSkill
{
    public Animator animator;
    int isBBHash;
    public AudioClip bbSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();
        isBBHash = Animator.StringToHash("isBathblessed");
        maxCharges = 0;
        cooldown = 0;
        oneTimeUseAvailable = true;
        skillCode = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();
        
        AudioSource.PlayClipAtPoint(bbSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        
        animator.SetBool(isBBHash, true);
        player = GameObject.FindWithTag("Player");
        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
        playerScript.isBerserk = true;          //wont actually grow twice in size in final, just to see effect 
        playerScript.transform.localScale *= 2;
        PlayerStats.Instance.speedMultiplier = 2;
        yield return new WaitForSeconds(0.8f);
        animator.SetBool(isBBHash, false);

        //Also reset all equipped skills and cooldowns

        yield return new WaitForSeconds(10);
        
        AudioSource.PlayClipAtPoint(bbSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        
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
