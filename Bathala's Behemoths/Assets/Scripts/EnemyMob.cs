using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMob : MonoBehaviour
{
    public abstract void TakeDamage(float damage);
    public abstract IEnumerator Stun(float duration);

    public float health;

    public void ApplyDamageOverTime(float tickDamage, int tickCount, float tickInterval)
    {
        if (tickDamage <= 0f || tickCount <= 0 || tickInterval <= 0f)
        {
            return;
        }

        DamageOverTimeEffect damageOverTimeEffect = GetComponent<DamageOverTimeEffect>();
        if (damageOverTimeEffect == null)
        {
            damageOverTimeEffect = gameObject.AddComponent<DamageOverTimeEffect>();
        }

        damageOverTimeEffect.Apply(this, tickDamage, tickCount, tickInterval);
    }
}
