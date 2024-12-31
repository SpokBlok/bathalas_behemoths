using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KapreBasicAttackTrigger : MonoBehaviour
{
    public GameObject playerHit;

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
        if (playerHit != other.gameObject && other.gameObject.CompareTag("Player"))
        {
            playerHit = other.gameObject;
            other.GetComponent<PlayerMovement>().TakeDamage(5);
        }
    }
}
