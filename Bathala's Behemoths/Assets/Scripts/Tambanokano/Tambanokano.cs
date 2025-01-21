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
    public GameObject massiveAOEPrefab;

    private Coroutine randomAttackCoroutine;
    private bool isUlting;

    private bool isLightingStriking;
    private bool stunned;

    public float health;

    public int attacksPassed;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        health = 5000f;

        isLightingStriking = false;
        stunned = false;

        attacksPassed = 9;
    }

    // Update is called once per frame
    void Update()
    {
        if (randomAttackCoroutine == null && !isUlting)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            isLightingStriking = false;
            int index = Random.Range(0, 3);
            if (attacksPassed >= 10)
            {
                index = 3;
            }
            attacksPassed++;

            switch(index)
            {
                case 0:
                    randomAttackCoroutine = StartCoroutine(ClawSwipe());
                    break;

                case 1:
                    randomAttackCoroutine = StartCoroutine(TrailingLightning());
                    isLightingStriking = true;
                    break;

                case 2:
                    randomAttackCoroutine = StartCoroutine(ArenaWideLightning());
                    isLightingStriking = true;
                    break;

                case 3:
                    randomAttackCoroutine = StartCoroutine(UltimateAttack());
                    isLightingStriking = true;
                    attacksPassed = 0;
                    break;
            }
        }
    }

    public override IEnumerator Stun(float duration)
    {
        if (isLightingStriking)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            StopCoroutine(randomAttackCoroutine);
            randomAttackCoroutine = null;
            isLightingStriking = false;
            isUlting = false;
        }
        //stun animation
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

    private IEnumerator SingleClawSwipe()
    {
        //attack animation
        GameObject claw = Instantiate(clawSwipePrefab, new Vector3(Random.Range(250f, 650f), 170f, 365f), Quaternion.Euler(0f, 90f, 0f));
        claw.transform.parent = transform;
        yield return new WaitForSeconds(claw.GetComponent<FillEffect>().attackDuration);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator ClawSwipe() //6 second claw swipe, 14 second total
    {
        //attack animation
        GameObject claw = Instantiate(clawSwipePrefab, new Vector3(Random.Range(250f, 650f), 170f, 370f), Quaternion.Euler(0f, 90f, 0f));
        claw.transform.parent = transform;
        yield return new WaitForSeconds(claw.GetComponent<FillEffect>().attackDuration);
        yield return new WaitForSeconds(0.5f);

        //attack animation
        claw = Instantiate(clawSwipePrefab, new Vector3(Random.Range(250f, 650f), 170f, 370f), Quaternion.Euler(0f, 90f, 0f));
        yield return new WaitForSeconds(claw.GetComponent<FillEffect>().attackDuration);
        yield return new WaitForSeconds(1.5f);

        randomAttackCoroutine = null;
    }

    private IEnumerator TrailingLightning() //6 * 1.5 seconds = 9 seconds total
    {
        isLightingStriking = true;
        
        GameObject lightning = Instantiate(trailingLightningPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        lightning.transform.parent = transform;
        TrailingLightningStrike lightningComponent = lightning.GetComponent<TrailingLightningStrike>();
        if (lightningComponent == null)
        {
            Debug.LogError("TrailingLightningStrike component not found on lightning prefab.");
            yield break; // Exit the coroutine if the component is not found
        }

        float duration = lightningComponent.attackGapDuration * lightningComponent.attacksLeft;
        
        yield return new WaitForSeconds(duration);
        yield return new WaitForSeconds(1.5f);

        randomAttackCoroutine = null;
    }

    private IEnumerator ArenaWideLightning() //5 seconds
    {
        isLightingStriking = true;

        GameObject lightning = Instantiate(arenaWideLightingPrefab, player.transform.position, Quaternion.identity);
        lightning.transform.parent = transform;
        yield return new WaitForSeconds(5);
        yield return new WaitForSeconds(1.5f);

        randomAttackCoroutine = null;
    }

    private IEnumerator UltimateAttack()
    {
        isUlting = true;
        StartCoroutine(TrailingLightning()); // 6*1.5 = 9 seconds
        StartCoroutine(ArenaWideLightning()); //5 seconds
        StartCoroutine(ClawSwipe()); // 12.5 seconds

        yield return new WaitForSeconds(6);
        StartCoroutine(ArenaWideLightning());

        yield return new WaitForSeconds(4);
        StartCoroutine(TrailingLightning());

        yield return new WaitForSeconds(2);
        StartCoroutine(SingleClawSwipe());
        StartCoroutine(ArenaWideLightning());

        yield return new WaitForSeconds(7);
        GameObject lightning = Instantiate(massiveAOEPrefab, player.transform.position, Quaternion.identity);
        lightning.transform.parent = transform;

        yield return new WaitForSeconds(10);

        isUlting = false;
        randomAttackCoroutine = null;
    }
}
