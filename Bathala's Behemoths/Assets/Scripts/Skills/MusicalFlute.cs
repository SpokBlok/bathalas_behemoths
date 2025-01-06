using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MusicalFlute : BaseSkill
{
    private GameObject player;

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
            KapreMob mob = collider.GetComponent<KapreMob>();
            if (mob != null)
            {
                StartCoroutine(mob.Stun(5));
            }
        }

        yield return null;
    }
}
