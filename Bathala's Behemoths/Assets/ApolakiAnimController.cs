using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApolakiAnimController : MonoBehaviour
{
    public Animator animator;
    int isMeleeAttackingHash;
    int isMeteorShowerHash;
    int isUltimateHash;
    int isCrossSlashHash;
    int isStunnedHash;
    public Apolaki apolaki;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isMeleeAttackingHash = Animator.StringToHash("isMeleeAttacking");
        isMeteorShowerHash = Animator.StringToHash("isMeteorShower");
        isUltimateHash = Animator.StringToHash("isUltimate");
        isCrossSlashHash = Animator.StringToHash("isCrossSlash");
        isStunnedHash = Animator.StringToHash("isStunned");
        apolaki = GameObject.FindGameObjectWithTag("Apolaki").GetComponent<Apolaki>();
    }

    // Update is called once per frame
    void Update()
    {
        if(apolaki.isMeleeAttacking)
        {
            animator.SetBool(isMeleeAttackingHash, true);
        }
        else
        {
            animator.SetBool(isMeleeAttackingHash, false);
        }

        if(apolaki.isMeteorShower)
        {
            animator.SetBool(isMeteorShowerHash, true);
        }
        else
        {
            animator.SetBool(isMeteorShowerHash, false);
        }

        if(apolaki.isUltimate)
        {
            animator.SetBool(isUltimateHash, true);
        }
        else
        {
            animator.SetBool(isUltimateHash, false);
        }

        if(apolaki.isCrossSlash)
        {
            animator.SetBool(isCrossSlashHash, true);
        }
        else
        {
            animator.SetBool(isCrossSlashHash, false);
        }

        if(apolaki.isStunned)
        {
            animator.SetBool(isStunnedHash, true);
        }
        else
        {
            animator.SetBool(isStunnedHash, false);
        }
    }
}
