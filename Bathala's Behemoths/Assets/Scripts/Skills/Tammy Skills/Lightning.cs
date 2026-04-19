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

        if (Camera.main != null)
        {
            Vector3 audioPosition = Camera.main.transform.position + Camera.main.transform.forward * 2f;
            if (punchSound != null)
            {
                AudioSource.PlayClipAtPoint(punchSound, audioPosition, 1f);
            }

            if (blowLands != null)
            {
                AudioSource.PlayClipAtPoint(blowLands, audioPosition, 1f);
            }
        }

        Vector3 horizontalForward = Quaternion.Euler(0f, player.transform.eulerAngles.y, 0f) * Vector3.forward;
        Vector3 spawnPosition = player.transform.position + horizontalForward * spawnDistance;
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
