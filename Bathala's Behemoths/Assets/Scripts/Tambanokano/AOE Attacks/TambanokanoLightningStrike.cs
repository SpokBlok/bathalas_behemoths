using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TambanokanoLightningStrike : AOEAttackRadius
{
    public SphereCollider radiusCollider;
    private Vector3 sphereCenter;
    private float sphereRadius;

    public float attackDamage;
    public float stunDuration;

    private bool hasDamagedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        hasDamagedPlayer = false;
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
            if (hitCollider.CompareTag("Player") && !hasDamagedPlayer)
            {
                hitCollider.GetComponent<PlayerMovement>().TakeDamage(attackDamage);
                hasDamagedPlayer = true;
            }
            else if (hitCollider.CompareTag("Enemy"))
            {
                if (hitCollider.TryGetComponent<EnemyMob>(out var mob))
                {
                    mob.TakeDamage(attackDamage);
                    if (stunDuration > 0f)
                    {
                        mob.StartCoroutine(mob.Stun(stunDuration));
                    }
                }
            }
        }
    }
}
