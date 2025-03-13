using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markyAnim : MonoBehaviour
{
    public Animator animator;
    int isPoisonSprayingHash;
    int isTailSpinningHash;
    public MarkupoScript marky;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isPoisonSprayingHash = Animator.StringToHash("isPoisonSpraying");
        isTailSpinningHash = Animator.StringToHash("isTailSpinning");
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
        else
        {
            animator.SetBool(isPoisonSprayingHash, false);
            animator.SetBool(isTailSpinningHash, false);
        }
    }
}
