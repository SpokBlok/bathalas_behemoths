using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrikeAOEAttackRadius : AOEAttackRadius
{
    public SphereCollider radiusCollider;
    private Vector3 sphereCenter;
    private float sphereRadius;

    public float attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        radiusCollider = GetComponent<SphereCollider>();
        sphereRadius = radiusCollider.radius * Mathf.Max(
            radiusCollider.transform.lossyScale.x,
            radiusCollider.transform.lossyScale.y,
            radiusCollider.transform.lossyScale.z
        );

        sphereCenter = radiusCollider.transform.position + radiusCollider.center;
    }

    public override void Damage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, sphereRadius);
        attackDamage = PlayerStats.Instance.basicAttackDamage * 0.7f;
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.GetComponent<EnemyMob>().TakeDamage(attackDamage);
            }
        }
    }
}
