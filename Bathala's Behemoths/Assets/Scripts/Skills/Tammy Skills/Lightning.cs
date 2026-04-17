using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : BaseSkill
{
    public Animator animator;
    public FillEffect lightningPrefab;
    public AudioClip punchSound;
    public AudioClip blowLands;
    public AudioClip thunderSound;
    public AudioClip overlaySound;
    
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
        if (player == null)
        {
            yield break;
        }

        animator = player.GetComponentInChildren<Animator>();

        AudioSource.PlayClipAtPoint(punchSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        AudioSource.PlayClipAtPoint(blowLands, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);

        if (lightningPrefab != null)
        {
            FillEffect lightning = Instantiate(lightningPrefab, player.transform.position, Quaternion.identity);
            lightning.SetSFX(thunderSound, overlaySound);

            TambanokanoLightningStrike strike = lightning.GetComponentInChildren<TambanokanoLightningStrike>();
            if (strike != null)
            {
                strike.attackDamage = PlayerStats.Instance.basicAttackDamage;
            }

            yield return new WaitForSeconds(lightning.attackDuration + 0.1f);
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, 15.0f);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<EnemyMob>(out var mob))
                {
                    mob.TakeDamage(PlayerStats.Instance.basicAttackDamage);
                }
            }
        }
    }
}
