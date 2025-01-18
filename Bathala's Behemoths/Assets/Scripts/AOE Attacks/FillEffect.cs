using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillEffect : MonoBehaviour
{
    private Transform fillCircle;
    private float timer;
    public float attackDuration;
    public float finalScale;

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
            float currentScale = currentPercentage * finalScale;
            fillCircle.localScale = new (currentScale, currentScale, 15);
        }
        else
        {
            attackRadius.Damage();
            Destroy(gameObject);
        }
    }
}
