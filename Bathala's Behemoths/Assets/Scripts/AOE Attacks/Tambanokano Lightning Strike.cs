using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TambanokanoLightningStrike : AOEAttackRadius
{
    public SphereCollider radiusCollider;
    GameObject player;
    private Vector3 sphereCenter;
    private float sphereRadius;

    public float attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        float scale = transform.parent.GetComponentInChildren<FillEffect>().finalScale;
        transform.localScale = new(scale, scale, scale);

        radiusCollider = GetComponent<SphereCollider>();

        player = GameObject.FindGameObjectWithTag("Player");
        // Get the terrain height at the character's current position (X, Z)
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        Vector3 position = transform.position;
        transform.position = new(position.x, terrainHeight, position.z);

        sphereCenter = radiusCollider.transform.position + radiusCollider.center;
        sphereRadius = radiusCollider.radius * Mathf.Max(
            radiusCollider.transform.lossyScale.x,
            radiusCollider.transform.lossyScale.y,
            radiusCollider.transform.lossyScale.z
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Damage()
    {
        // Detect all colliders within the sphere
        Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, sphereRadius);

        // Process detected colliders
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
