using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailingLightningStrike : MonoBehaviour
{
    public FillEffect lightningPrefab;
    private GameObject player;
    private float timer = 0;
    private float attacksLeft = 15;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 1.5f)
            {
            timer += Time.deltaTime;
        }
        else if (attacksLeft > 0)
        {
            Instantiate(lightningPrefab, player.transform.position, Quaternion.identity);
            timer = 0f;
            attacksLeft--;
        } else
        {
            Destroy(gameObject);
        }
    }
}
