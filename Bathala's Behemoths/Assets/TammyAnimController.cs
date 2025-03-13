using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TammyAnimController : MonoBehaviour
{
    public Animator animator;
    int isClawSwipeHash;
    int isStunnedHash;
    public Tambanokano tammy;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isClawSwipeHash = Animator.StringToHash("isClawSwiping");
        isStunnedHash = Animator.StringToHash("isStunned");
        tammy = GameObject.FindGameObjectWithTag("Tambanokano").GetComponent<Tambanokano>();
    }

    // Update is called once per frame
    void Update()
    {
        if(tammy.isClawSwiping)
        {
            animator.SetBool(isClawSwipeHash, true);
        }
        else
        {
            animator.SetBool(isClawSwipeHash, false);
        }

        if(tammy.stunned)
        {
            animator.SetBool(isStunnedHash, true);
        }
        else
        {
            animator.SetBool(isStunnedHash, false);
        }
    }
}
