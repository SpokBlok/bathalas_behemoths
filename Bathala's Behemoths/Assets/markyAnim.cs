using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markyAnim : MonoBehaviour
{
    public Animator animator;
    int isPoisonSprayingHash;
    int isTailSpinningHash;
    int isNormStunnedHash;
    int isFluteStunnedHash;
    public MarkupoScript marky;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isPoisonSprayingHash = Animator.StringToHash("isPoisonSpraying");
        isTailSpinningHash = Animator.StringToHash("isTailSpinning");
        isNormStunnedHash = Animator.StringToHash("NormStunnedg");
        isFluteStunnedHash = Animator.StringToHash("isFluteStunned");
        marky = GameObject.FindGameObjectWithTag("Markupo").GetComponent<MarkupoScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(marky.isSpraying)
        {
            animator.SetBool(isPoisonSprayingHash, true);
        }
        else if(marky.isSpinning)
        {
            animator.SetBool(isTailSpinningHash, true);
        }
        else if(marky.isFluteStunned)
        {
            animator.SetBool(isFluteStunnedHash, true);
        }
        else if(marky.isNormStunned)
        {
            animator.SetBool(isNormStunnedHash, true);
        }
        else
        {
            animator.SetBool(isPoisonSprayingHash, false);
            animator.SetBool(isTailSpinningHash, false);
            animator.SetBool(isNormStunnedHash, false);
            animator.SetBool(isFluteStunnedHash, false);
        }
    }
}
