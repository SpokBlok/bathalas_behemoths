using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TammyAnimController : MonoBehaviour
{
    public Animator animator;
    int isClawSwipeHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isClawSwipeHash = Animator.StringToHash("isClawSwiping");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
