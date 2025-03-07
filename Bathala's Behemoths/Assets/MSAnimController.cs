using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSAnimController : MonoBehaviour
{
    public Animator animator;
    float velocity = 0.0f;
    public float acceleration = 50.0f;
    public float deceleration = 100.0f;
    int velocityHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
        bool moving = false;
        float moveHor = Input.GetAxis("Horizontal");
        float moveVert = Input.GetAxis("Vertical");

        animator.SetFloat("InputX",moveHor);
        animator.SetFloat("InputY",moveVert);

        if(moveHor != 0 || moveVert != 0)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        if((moving && shiftPressed) && velocity < 15.0f)
        {
            velocity += Time.deltaTime * acceleration;
        }
        else if(moving && velocity < 5.0f)
        {
            velocity += Time.deltaTime * acceleration;
        }
        else if((moving && !shiftPressed) && velocity > 5.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }
        else if(!moving && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        if(!moving && velocity < 0.0f)
        {
            velocity = 0.0f;
        }

        animator.SetFloat(velocityHash, velocity);
        PlayerStats.Instance.SetSpeed((int)velocity);
    }
}
