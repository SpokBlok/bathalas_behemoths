using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public abstract void RunSkill();
    protected float cooldown;
    protected int charges;

}
