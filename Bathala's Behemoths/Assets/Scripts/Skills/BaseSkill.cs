using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public abstract IEnumerator RunSkill();
    protected float cooldown;
    protected int maxCharges;

}
