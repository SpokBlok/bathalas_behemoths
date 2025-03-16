using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KapreAnimController : MonoBehaviour
{
    public Animator animator;
    public bool moving;
    public bool attacking;
    public bool stunned;
    int isMovingHash;
    int isAttackingHash;
    int isStunnedHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isMovingHash = Animator.StringToHash("isMoving");
        isAttackingHash = Animator.StringToHash("isAttacking");
        isStunnedHash = Animator.StringToHash("isStunned");
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            animator.SetBool(isMovingHash, true);
        }
        else
        {
            animator.SetBool(isMovingHash, false);
        }

        if(attacking)
        {
            animator.SetBool(isAttackingHash, true);
        }
        else
        {
            animator.SetBool(isAttackingHash, false);
        }
        if(stunned)
        {
            animator.SetBool(isStunnedHash, true);
        }
        else
        {
            animator.SetBool(isStunnedHash, false);
        }
    }

    public void setAttacking(bool attackStatus)
    {
        attacking = attackStatus;
    }

    public void setMoving(bool moveStatus)
    {
        moving = moveStatus;
    }

    public void setStunned(bool stunStatus)
    {
        stunned = stunStatus;
    }
}
