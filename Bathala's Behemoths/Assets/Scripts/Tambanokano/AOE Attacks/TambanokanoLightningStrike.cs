using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TambanokanoLightningStrike : AOEAttackRadius
{
    public SphereCollider radiusCollider;
    private Vector3 sphereCenter;
    private float sphereRadius;

    public float attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        float scaleX = transform.parent.GetComponentInChildren<FillEffect>().finalScaleX;
        transform.localScale = new(scaleX, scaleX, scaleX);

        radiusCollider = GetComponent<SphereCollider>();

        // Get the terrain height at the object's current position (X, Z)
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

    public override void Damage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, sphereRadius);

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
