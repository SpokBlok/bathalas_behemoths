using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatticeLightning : AOEAttackRadius
{
    Tambanokano tammy;
    private BoxCollider[] colliders;
    public float attackDamage;

    private float timer;
    public float attackDuration;
    public AudioClip thunderSound;
    public GameObject lightning;
    public bool lightningSpawned = false;
    private bool hasDamagedPlayer;
    private bool hasDamagedEnemy;
    private bool audioSpawned;
    private GameObject spawnedLightning1;
    private GameObject spawnedLightning2;
    private GameObject spawnedLightning3;
    private GameObject spawnedLightning4;

    // Start is called before the first frame update
    void Start()
    {
        hasDamagedPlayer = false;
        audioSpawned = false;
        // Include inactive children so we can still use their
        // BoxCollider shapes for the overlap checks.
        colliders = GetComponentsInChildren<BoxCollider>(true);
    }

    public override void Damage()
    {
        foreach (BoxCollider collider in colliders)
        {
            // Use the collider's world-space bounds to define the overlap box
            Vector3 worldCenter = collider.bounds.center;
            Vector3 halfExtents = collider.bounds.extents;

            Collider[] hitColliders = Physics.OverlapBox(worldCenter, halfExtents);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player") && !hasDamagedPlayer)
                {
                    Debug.Log("Player hit by Lattice Lightning!");
                    hitCollider.GetComponent<PlayerMovement>().TakeDamage(attackDamage);
                    hasDamagedPlayer = true;
                }
                else if (hitCollider.CompareTag("Enemy"))
                {
                    hitCollider.GetComponent<EnemyMob>().TakeDamage(attackDamage);
                }
            }
        }
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
            Damage();
            
            if (!lightningSpawned && lightning != null)
            {
                // Spawn multiple lightning effects at the same time at the edges of the arena
                spawnedLightning1 = Instantiate(lightning, new Vector3(1000f, transform.position.y, 0f), Quaternion.identity);
                spawnedLightning1.transform.localScale = new Vector3(50.0f, 30.0f, 50.0f);

                spawnedLightning2 = Instantiate(lightning, new Vector3(0f, transform.position.y, 1000f), Quaternion.identity);
                spawnedLightning2.transform.localScale = new Vector3(50.0f, 30.0f, 50.0f);

                spawnedLightning3 = Instantiate(lightning, new Vector3(1000f, transform.position.y, 2000f), Quaternion.identity);
                spawnedLightning3.transform.localScale = new Vector3(50.0f, 30.0f, 50.0f);

                spawnedLightning4 = Instantiate(lightning, new Vector3(2000f, transform.position.y, 1000f), Quaternion.identity);
                spawnedLightning4.transform.localScale = new Vector3(50.0f, 30.0f, 50.0f);

                lightningSpawned = true;
            }

            if(PlayerStats.Instance.tammyScene)
            {
                tammy = GameObject.FindWithTag("Tambanokano").GetComponent<Tambanokano>();
                if(tammy != null)
                {
                    tammy.BlinkOnce();
                }
            }

            if(!audioSpawned)
            {
                AudioSource.PlayClipAtPoint(thunderSound, transform.position, 1f);
                audioSpawned = true;
            }

            Destroy(spawnedLightning1, 1.0f); // Destroys the lightning effect after 0.5 seconds
            Destroy(spawnedLightning2, 1.0f); // Destroys the lightning effect after 0.5 seconds
            Destroy(spawnedLightning3, 1.0f); // Destroys the lightning effect after 0.5 seconds
            Destroy(spawnedLightning4, 1.0f); // Destroys the lightning effect after 0.5 seconds
            Destroy(gameObject, 2.0f); // Destroys the fill effect after 0.5 seconds
        }
    }
}
