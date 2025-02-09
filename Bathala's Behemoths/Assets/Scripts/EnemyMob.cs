using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMob : MonoBehaviour
{
    public abstract void TakeDamage(float damage);
    public abstract IEnumerator Stun(float duration);

    public float health;
}
