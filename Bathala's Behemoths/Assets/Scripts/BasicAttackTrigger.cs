using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackTrigger : MonoBehaviour
{
    public GameObject enemyHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //enemyHit is to prevent double damage triggers
        if (enemyHit != other.gameObject && other.gameObject.CompareTag("Enemy"))
        {
            enemyHit = other.gameObject;
            other.GetComponent<EnemyMob>().TakeDamage(PlayerStats.Instance.basicAttackDamage / 2);
        }
    }
}
