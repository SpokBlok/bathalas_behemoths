using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MusicalFlute : BaseSkill
{
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        maxCharges = 1;
        cooldown = 40;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator RunSkill()
    {
        float detectionRadius = 10f;
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<KapreMob>(out var mob))
            {
                StartCoroutine(mob.Stun(5));
            }
        }

        yield return null;
    }
}
