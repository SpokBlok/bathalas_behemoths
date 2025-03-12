using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public abstract IEnumerator RunSkill();
    protected GameObject player;
    public float cooldown;
    public int maxCharges;
    public bool oneTimeUseAvailable;
    public int skillCode;

}
