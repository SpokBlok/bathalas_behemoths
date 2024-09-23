using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMob : MonoBehaviour
{
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        health = 50;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
