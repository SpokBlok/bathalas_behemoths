using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tambanokano : EnemyMob
{

    private GameObject player;

    //Attack prefabs
    public GameObject clawSwipePrefab;
    public GameObject trailingLightningPrefab;
    public GameObject arenaWideLightingPrefab;

    private BoxCollider clawRange;

    private bool isLightingStriking;

    public float health;

    private int minutesPassed;
    private int halfMinutesPassed;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        clawRange = GetComponentInChildren<BoxCollider>();
        health = 1000f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override IEnumerator Stun(float duration)
    {
        if (isLightingStriking)
        {
            //cancel lightning strikes
        }
        yield return new WaitForSeconds(duration);
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //trigger winning cutscene
        }
    }

    private IEnumerator ClawSwipe()
    {
        Instantiate(clawSwipePrefab, new(Random.Range(250f, 650f), 170f, 359f), Quaternion.Euler(0f, 90f, 0f));
        yield return new WaitForSeconds(1);
    }

    private IEnumerator TrailingLightning()
    {
        Instantiate(trailingLightningPrefab, new(0f, 0f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(3);  
    }

    private IEnumerator ArenaWideLightning()
    {
        Instantiate(arenaWideLightingPrefab, player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(8);
    }
}
