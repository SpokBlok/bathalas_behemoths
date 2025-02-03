using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoPunch : BaseSkill
{
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        maxCharges = 1;
        cooldown = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        yield return new WaitForSeconds(0.3f); //Charge up time, animation of tornado punch
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, 5f);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<EnemyMob>(out var mob))
            {
                mob.TakeDamage(PlayerStats.Instance.basicAttackDamage);
            }
        }
    }
}
