using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMob : MonoBehaviour
{
    public int health;
    public bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        health = 50;
        SphereCollider triggerRadius = GetComponentInChildren<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Enemy killed!");
            Destroy(gameObject);
        }
    }

    public void ChasePlayer()
    {
        if (playerInRange){
            //Chase player position code
        }
    }
}
