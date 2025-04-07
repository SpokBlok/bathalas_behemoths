using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatticeLightning : AOEAttackRadius
{
    private BoxCollider[] colliders;
    public float attackDamage;

    private float timer;
    public float attackDuration;
    public AudioClip thunderSound;

    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponentsInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(QuestState.Instance.pausedForDialogue) {return;} // Prevents continued function while taming dialogue is active

        if (timer <= attackDuration)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Tambanokano tammy = GameObject.FindWithTag("Tambanokano").GetComponent<Tambanokano>();
            AudioSource.PlayClipAtPoint(thunderSound, Camera.main.transform.position + Camera.main.transform.forward * 1f, 1f);
            tammy.BlinkOnce();
            Damage();
            Destroy(gameObject);
        }
    }

    public override void Damage()
    {
        foreach (BoxCollider collider in colliders)
        {
            Vector3 worldCenter = collider.transform.TransformPoint(collider.center);
            Vector3 worldSize = Vector3.Scale(collider.size, collider.transform.lossyScale) / 2;

            Collider[] hitColliders = Physics.OverlapBox(worldCenter, worldSize, collider.transform.rotation);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    hitCollider.GetComponent<PlayerMovement>().TakeDamage(attackDamage);
                }
                else if (hitCollider.CompareTag("Enemy"))
                {
                    hitCollider.GetComponent<EnemyMob>().TakeDamage(attackDamage);
                }
            }
        }

    }
}
