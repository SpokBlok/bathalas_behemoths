using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillEffect : MonoBehaviour
{
    private Transform fillCircle;
    private float timer;
    public float attackDuration;
    public float finalScaleX;
    public float finalScaleY;

    private AOEAttackRadius attackRadius;

    void Start()
    {
        timer = 0;
        fillCircle = transform.Find("Inside Section").GetComponent<Transform>();
        attackRadius = GetComponentInChildren<AOEAttackRadius>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= attackDuration)
        {
            timer += Time.deltaTime;
            float currentPercentage = timer / attackDuration;
            float currentScaleX = currentPercentage * finalScaleX;
            float currentScaleY = currentPercentage * finalScaleY;
            fillCircle.localScale = new (currentScaleX, currentScaleY, 15);
        }
        else
        {
            attackRadius.Damage();
            Destroy(gameObject);
        }
    }
}
