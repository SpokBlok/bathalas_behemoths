using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : BaseSkill
{
    public Animator animator;
    public SkillLightningFill lightningPrefab;
    public AudioClip punchSound;
    public AudioClip blowLands;
    public AudioClip thunderSound;
    public AudioClip overlaySound;
    public float spawnDistance = 3f;
    public float stunDuration = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();
        // isSwipeHash = Animator.StringToHash("isSwipe");
        maxCharges = 1;
        cooldown = 10;
        skillCode = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null || lightningPrefab == null)
        {
            yield break;
        }

        AudioSource.PlayClipAtPoint(punchSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        AudioSource.PlayClipAtPoint(blowLands, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);

        Vector3 spawnPosition = player.transform.position + player.transform.forward * spawnDistance;
        SkillLightningFill lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);
        lightning.SetSFX(thunderSound, overlaySound);

        TammySkillLightning strike = lightning.GetComponentInChildren<TammySkillLightning>();
        if (strike != null)
        {
            strike.attackDamage = PlayerStats.Instance.basicAttackDamage;
            strike.stunDuration = stunDuration;
        }

        yield return new WaitForSeconds(lightning.attackDuration + 0.1f);
    }
}
