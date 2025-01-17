using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillEffect : MonoBehaviour
{
    private Transform fillCircle;
    private float timer;
    private Vector3 fillVector;

    void Start()
    {
        fillCircle = transform.Find("Inside Circle").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 5f)
        {
            timer += Time.deltaTime;
            fillCircle.localScale = new (timer + 1, timer + 1, timer*3);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
