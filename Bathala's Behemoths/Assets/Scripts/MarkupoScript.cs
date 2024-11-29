using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkupoScript : MonoBehaviour
{
    public int health;
    private GameObject bulletHit;

    // Start is called before the first frame update
    void Start()
    {
        health = 1000;

        //Start off on terrain height
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        Vector3 newPosition = transform.position;
        newPosition.y = terrainHeight + 1.2f;
        transform.position = newPosition;
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
            //Trigger win cutscene
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bulletHit != other.gameObject && other.gameObject.CompareTag("Projectile"))
        {
            bulletHit = other.gameObject;
            takeDamage(PlayerStats.Instance.basicAttackDamage);
        }
    }
}
