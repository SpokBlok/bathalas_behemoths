using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoPunch : BaseSkill
{
    public Animator animator;
    int isTornadoHash;
    public AudioClip punchSound;
    public AudioClip blowLands;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();
        isTornadoHash = Animator.StringToHash("isTornado");
        maxCharges = 1;
        cooldown = 10;
        skillCode = 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();
        
        animator.SetBool(isTornadoHash, true);
        player = GameObject.FindWithTag("Player");
        AudioSource.PlayClipAtPoint(punchSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        AudioSource.PlayClipAtPoint(blowLands, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        yield return new WaitForSeconds(1.0f); //Charge up time, animation of tornado punch
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, 8.0f);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<EnemyMob>(out var mob))
            {
                mob.TakeDamage(PlayerStats.Instance.basicAttackDamage);
            }
        }
        animator.SetBool(isTornadoHash, false);
    }
}
