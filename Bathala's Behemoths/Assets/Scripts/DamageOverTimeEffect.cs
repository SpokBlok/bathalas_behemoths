using System.Collections;
using UnityEngine;

public class DamageOverTimeEffect : MonoBehaviour
{
    private Coroutine damageOverTimeCoroutine;

    public void Apply(EnemyMob enemy, float tickDamage, int tickCount, float tickInterval)
    {
        if (damageOverTimeCoroutine != null)
        {
            StopCoroutine(damageOverTimeCoroutine);
        }

        damageOverTimeCoroutine = StartCoroutine(ApplyRoutine(enemy, tickDamage, tickCount, tickInterval));
    }

    private IEnumerator ApplyRoutine(EnemyMob enemy, float tickDamage, int tickCount, float tickInterval)
    {
        for (int currentTick = 0; currentTick < tickCount; currentTick++)
        {
            yield return new WaitForSeconds(tickInterval);

            if (enemy == null || enemy.health <= 0f)
            {
                damageOverTimeCoroutine = null;
                yield break;
            }

            enemy.TakeDamage(tickDamage);
        }

        damageOverTimeCoroutine = null;
    }
}