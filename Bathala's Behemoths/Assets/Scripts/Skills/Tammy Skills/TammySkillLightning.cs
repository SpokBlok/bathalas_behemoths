using System.Collections.Generic;
using UnityEngine;

public class TammySkillLightning : AOEAttackRadius
{
    public SphereCollider radiusCollider;
    public float attackDamage;
    public float stunDuration;

    void Start()
    {
        SkillLightningFill fill = GetComponentInParent<SkillLightningFill>();
        if (fill != null)
        {
            float scaleX = fill.finalScaleX;
            transform.localScale = new Vector3(scaleX, scaleX, scaleX);
        }

        radiusCollider = GetComponent<SphereCollider>();
        if (radiusCollider == null)
        {
            return;
        }

        if (Terrain.activeTerrain != null)
        {
            float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
            Vector3 position = transform.position;
            transform.position = new Vector3(position.x, terrainHeight, position.z);
        }

    }

    public override void Damage()
    {
        if (radiusCollider == null)
        {
            return;
        }

        Bounds radiusBounds = radiusCollider.bounds;
        float sphereRadius = Mathf.Max(radiusBounds.extents.x, radiusBounds.extents.y, radiusBounds.extents.z);
        Collider[] hitColliders = Physics.OverlapSphere(radiusBounds.center, sphereRadius);
        HashSet<EnemyMob> hitEnemies = new HashSet<EnemyMob>();

        foreach (Collider hitCollider in hitColliders)
        {
            EnemyMob mob = hitCollider.GetComponent<EnemyMob>();
            if (mob == null)
            {
                mob = hitCollider.GetComponentInParent<EnemyMob>();
            }

            if (mob == null || !hitEnemies.Add(mob))
            {
                continue;
            }

            mob.TakeDamage(attackDamage);
            if (stunDuration > 0f)
            {
                mob.StartCoroutine(mob.Stun(stunDuration));
            }
        }
    }
}