using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class Swipe : BaseSkill
{
    // Start is called before the first frame update
    void Start()
    {
        maxCharges = 1;
        cooldown = 8;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        PlayerMovement playerMove = player.GetComponent<PlayerMovement>();

        yield return new WaitForSeconds(1.0f); //Charge up time, animation of swipe
        Vector3 worldSize = new(8f, playerMove.worldSize.y, playerMove.worldSize.z);
        Collider[] colliders = Physics.OverlapBox(playerMove.worldCenter, worldSize, playerMove.basicAttackHitbox.transform.rotation);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<EnemyMob>(out var mob))
            {
                mob.TakeDamage(PlayerStats.Instance.basicAttackDamage);
            }
        }
        
    }
}
