using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TammyAnimController : MonoBehaviour
{
    public Animator animator;
    int isClawSwipeHash;
    public Tambanokano tammy;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isClawSwipeHash = Animator.StringToHash("isClawSwiping");
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
    }
}
