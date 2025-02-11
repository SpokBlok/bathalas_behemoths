using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tambanokano : EnemyMob
{

    private GameObject player;
    public GameObject endDialogue;

    //Attack prefabs
    public GameObject clawSwipePrefab;
    public GameObject trailingLightningPrefab;
    public GameObject arenaWideLightingPrefab;
    public GameObject massiveAOEPrefab;

    private Coroutine randomAttackCoroutine;
    private bool isAlive;
    private bool isUlting;

    private bool isLightingStriking;
    private bool stunned;

    public int attacksPassed;

    public float duration;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        health = 2500f;

        isLightingStriking = false;
        stunned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (randomAttackCoroutine == null && !isUlting && !stunned)
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
            stunned = true;
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            StopAllCoroutines();
            randomAttackCoroutine = null;
            isLightingStriking = false;
            isUlting = false;
        }
        //stun animation
        yield return new WaitForSeconds(5);
        stunned = false;
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //trigger winning cutscene
            isAlive = false;
            
            if(isAlive == false)
            {
                StopAllCoroutines();
                endDialogue.SetActive(true);
            }
        }
    }

    private IEnumerator SingleClawSwipe()
    {
        //attack animation
        GameObject claw = Instantiate(clawSwipePrefab, new Vector3(Random.Range(400f, 500f), 170f, Random.Range(285f, 425f)), Quaternion.Euler(0f, 90f, 0f));
        claw.transform.parent = transform;
        yield return new WaitForSeconds(claw.GetComponent<FillEffect>().attackDuration);
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator ClawSwipe()
    {
        //attack animation
        GameObject claw = Instantiate(clawSwipePrefab, new Vector3(Random.Range(400f, 500f), 170f, Random.Range(285f, 425f)), Quaternion.Euler(0f, 90f, 0f));
        claw.transform.parent = transform;
        yield return new WaitForSeconds(claw.GetComponent<FillEffect>().attackDuration);
        yield return new WaitForSeconds(2f);

        //attack animation
        claw = Instantiate(clawSwipePrefab, new Vector3(Random.Range(250f, 650f), 170f, 370f), Quaternion.Euler(0f, 90f, 0f));
        yield return new WaitForSeconds(claw.GetComponent<FillEffect>().attackDuration);
        yield return new WaitForSeconds(2f);

        randomAttackCoroutine = null;
    }

    private IEnumerator TrailingLightning()
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

        duration = lightningComponent.attackGapDuration * (lightningComponent.attacksLeft);
        
        yield return new WaitForSeconds(duration + 7f);

        randomAttackCoroutine = null;
    }

    private IEnumerator ArenaWideLightning()
    {
        isLightingStriking = true;

        GameObject lightning = Instantiate(arenaWideLightingPrefab, 
            new Vector3(Random.Range(380f, 420f), 300f, Random.Range(260f, 360f)),
            Quaternion.Euler(0f, Random.Range(0f, 90f), 0f));
        lightning.transform.parent = transform;
        yield return new WaitForSeconds(6);
        yield return new WaitForSeconds(2f);

        randomAttackCoroutine = null;
    }

    private IEnumerator UltimateAttack()
    {
        isUlting = true;
        StartCoroutine(TrailingLightning());
        StartCoroutine(ArenaWideLightning());

        yield return new WaitForSeconds(7);
        StartCoroutine(ClawSwipe());

        yield return new WaitForSeconds(1.25f);
        StartCoroutine(TrailingLightning());

        yield return new WaitForSeconds(6);
        StartCoroutine(ArenaWideLightning());

        yield return new WaitForSeconds(9);
        GameObject lightning = Instantiate(massiveAOEPrefab, new Vector3(440f, 160f, 240f), Quaternion.identity);
        lightning.transform.parent = transform;

        yield return new WaitForSeconds(14);

        isUlting = false;
        randomAttackCoroutine = null;
    }
}
