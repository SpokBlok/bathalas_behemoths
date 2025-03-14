using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSAnimController : MonoBehaviour
{
    public Animator animator;
    float velocity = 0.0f;
    public float accelerationTime = 0.5f;
    int velocityHash;
    public bool shiftPressed = false;
    private Coroutine acceleration;

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
        if(QuestState.Instance.pausedForDialogue) 
        {   
            animator.SetFloat(velocityHash, 0f);
            return; 
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            shiftPressed = !shiftPressed;
        }
        
        bool moving = false;
        float moveHor = Input.GetAxis("Horizontal");
        float moveVert = Input.GetAxis("Vertical");

        animator.SetFloat("InputX",moveHor);
        animator.SetFloat("InputY",moveVert);

        moving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if((moving && shiftPressed) && velocity <= 25.0f)
        {
            if(acceleration == null && velocity != 25.0f)
            {
                acceleration = StartCoroutine(Accelerate(velocity, 25.0f));
            }
        }
        else if(moving && velocity <= 5.0f)
        {
            if(acceleration == null && velocity != 5.0f)
            {
                acceleration = StartCoroutine(Accelerate(velocity, 5.0f));
            }
        }
        else if(moving && velocity > 5.0f)
        {
            if(acceleration == null)
            {
                acceleration = StartCoroutine(Accelerate(velocity, 5.0f));
            }
        }
        else if(!moving && velocity > 0.0f)
        {
            if(acceleration == null)
            {
                acceleration = StartCoroutine(Accelerate(velocity, 0.0f));
            }
        }

        if(!moving && velocity < 0.0f)
        {
            velocity = 0.0f;
        }

        animator.SetFloat(velocityHash, velocity);
        PlayerStats.Instance.SetSpeed((int)velocity);
    }

    IEnumerator Accelerate(float startValue, float endValue)
    {
        float duration = accelerationTime;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            velocity = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }
        
        velocity = endValue; // Ensure it fully reaches the target
        acceleration = null;
    }
}
