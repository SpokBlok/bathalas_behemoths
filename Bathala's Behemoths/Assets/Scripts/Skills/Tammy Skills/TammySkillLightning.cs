using UnityEngine;

public class TammySkillLightning : AOEAttackRadius
{
    public SphereCollider radiusCollider;
    private Vector3 sphereCenter;
    private float sphereRadius;

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

        sphereCenter = radiusCollider.transform.position + radiusCollider.center;
        sphereRadius = radiusCollider.radius * Mathf.Max(
            radiusCollider.transform.lossyScale.x,
            radiusCollider.transform.lossyScale.y,
            radiusCollider.transform.lossyScale.z
        );
    }

    public override void Damage()
    {
        if (radiusCollider == null)
        {
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, sphereRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (!hitCollider.TryGetComponent<EnemyMob>(out var mob))
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