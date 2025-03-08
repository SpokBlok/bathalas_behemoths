using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MarkupoMobSpawner : MonoBehaviour
{
    public GameObject kaprePlayerPrefab;

    private float timer;
    public float spawnGap;



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject kapre = Instantiate(kaprePlayerPrefab, new(Random.Range(200f, 300f), 50f, Random.Range(200f, 300f)), Quaternion.Euler(0f, 90f, 0f), transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnGap)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            for (int i = 0; i < 1; i++)
            {
                GameObject kapre = Instantiate(kaprePlayerPrefab, new(Random.Range(200f, 300f), 50f, Random.Range(200f, 300f)), Quaternion.Euler(0f, 90f, 0f));
                kapre.transform.parent = transform;
            }
        }
    }
}
