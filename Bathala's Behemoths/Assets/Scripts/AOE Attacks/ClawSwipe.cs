using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawSwipe : AOEAttackRadius
{
    public BoxCollider radiusCollider;
    private Vector3 worldCenter;
    private Vector3 worldSize;

    public float attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        radiusCollider = GetComponent<BoxCollider>();

        // Get the terrain height at the object's current position (X, Z)
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        Vector3 position = transform.position;
        transform.position = new(position.x, terrainHeight, position.z);

        worldCenter = radiusCollider.transform.TransformPoint(radiusCollider.center);
        worldSize = Vector3.Scale(radiusCollider.size, radiusCollider.transform.lossyScale) / 2;
    }

    public override void Damage()
    {
        Collider[] hitColliders = Physics.OverlapBox(worldCenter, worldSize, radiusCollider.transform.rotation);

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
