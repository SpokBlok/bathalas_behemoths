using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KapreAnimController : MonoBehaviour
{
    public Animator animator;
    public bool moving;
    public bool attacking;
    int isMovingHash;
    int isAttackingHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isMovingHash = Animator.StringToHash("isMoving");
        isAttackingHash = Animator.StringToHash("isAttacking");
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
    }

    public void setAttacking(bool attackStatus)
    {
        attacking = attackStatus;
    }

    public void setMoving(bool moveStatus)
    {
        moving = moveStatus;
    }
}
