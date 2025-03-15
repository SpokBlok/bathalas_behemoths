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
    public GameObject HUD;
    public TammyHPBarCanvas tammyHPBar;

    private Coroutine randomAttackCoroutine;
    private Coroutine getStunned;
    private Coroutine stunMoving;
    private Coroutine blinking;
    private bool isAlive;
    private bool isUlting;

    private bool isLightingStriking;
    public bool stunned;

    public int attacksPassed;

    public float duration;

    public Material eyesOpenMat;
    public Material eyesClosedMat;
    public Renderer tamRend;
    public GameObject tammy;
    public bool isClawSwiping;

    public SkinnedMeshRenderer[] modelRenderer;
    public TammyAnimController tammyModel;
    public Coroutine takingDamage;

    // Start is called before the first frame update
    void Start()
    {
        if (modelRenderer == null)
        {
            modelRenderer = tammyModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        player = GameObject.FindWithTag("Player");
        tamRend = GameObject.FindWithTag("TambanokanoBody").GetComponent<Renderer>();
        tammy = GameObject.FindWithTag("TammyModel");

        HUD = GameObject.FindWithTag("HUD");
        if (HUD != null)
        {
            // Search for tammyHPBarBG inside the HUD parent
            Transform hpBarTransform = HUD.transform.Find("TammyHPBarBG");

                if (hpBarTransform != null)
                {
                    // Activate the tammyHPBarBG GameObject
                    hpBarTransform.gameObject.SetActive(true);
                    Debug.Log("TammyHPBarBG activated!");
                }
        }
        
        health = 3500f;

        isLightingStriking = false;
        stunned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(QuestState.Instance.pausedForDialogue) {return;}
        if(!PlayerStats.Instance.tammyScene)
        {
            PlayerStats.Instance.tammyScene = true;
        }
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

    public void BlinkOnce()
    {
        blinking = StartCoroutine(blink(0.5f));
    }

    public void GetMudStunned()
    {
        getStunned = StartCoroutine(Stun(3));
        blinking = StartCoroutine(blink(3f));
    }

    public IEnumerator blink(float duration)
    {
        if (tamRend != null && eyesClosedMat != null && eyesOpenMat != null)
        {
            tamRend.material = eyesClosedMat;
            yield return new WaitForSeconds(duration);
            tamRend.material = eyesOpenMat;
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
        stunMoving = StartCoroutine(StunMovement());
        yield return new WaitForSeconds(duration);
        stunned = false;
    }

    IEnumerator StunMovement()
    {
        float degreeRot = 0.0f;
        float duration = 0.4f;
        float elapsedTime = 0.0f;
        float startValue = 0.0f;
        float endValue = -10.0f;
        Debug.Log("Tammy Stun Movement Start");

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            degreeRot = Mathf.Lerp(startValue, endValue, t);
            tammy.transform.rotation = Quaternion.Euler(degreeRot, 180, 0);
            yield return null;
        }
        
        degreeRot = endValue; // Ensure it fully reaches the target
        tammy.transform.rotation = Quaternion.Euler(degreeRot, 180, 0);
        elapsedTime = 0.0f;
        yield return new WaitForSeconds(2.2f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            degreeRot = Mathf.Lerp(endValue, startValue, t);
            tammy.transform.rotation = Quaternion.Euler(degreeRot, 180, 0);
            yield return null;
        }
        
        degreeRot = startValue; // Ensure it fully reaches the target
        tammy.transform.rotation = Quaternion.Euler(degreeRot, 180, 0);
    }

    IEnumerator SwitchToDamagedTex()
    {
        foreach(SkinnedMeshRenderer bodypart in modelRenderer)
        {
            bodypart.material.color = Color.red; // Change color to red
        }
        
        yield return new WaitForSeconds(0.2f); // Wait

        foreach(SkinnedMeshRenderer bodypart in modelRenderer)
        {
            bodypart.material.color = Color.white; // Restore original color
        }
    }

    public override void TakeDamage(float damage)
    {
        if(takingDamage == null)
        {
            takingDamage = StartCoroutine(SwitchToDamagedTex());
        }
        else if(takingDamage != null)
        {
            StopCoroutine(takingDamage);
            takingDamage = StartCoroutine(SwitchToDamagedTex());
        }

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

    private IEnumerator PlayClawSwipeAnimation()
    {
        isClawSwiping = true;
        yield return new WaitForSeconds(0.5f);
        isClawSwiping = false;
    }

    private IEnumerator ClawSwipe()
    {
        //attack animation
        GameObject claw = Instantiate(clawSwipePrefab, new Vector3(Random.Range(400f, 500f), 170f, Random.Range(350f, 400f)), Quaternion.Euler(0f, 90f, 0f));
        claw.transform.parent = transform;
        yield return new WaitForSeconds(claw.GetComponent<FillEffect>().attackDuration);
        StartCoroutine(PlayClawSwipeAnimation());
        yield return new WaitForSeconds(2f);

        //attack animation
        claw = Instantiate(clawSwipePrefab, new Vector3(Random.Range(400f, 500f), 170f, Random.Range(350f, 400f)), Quaternion.Euler(0f, 90f, 0f));
        yield return new WaitForSeconds(claw.GetComponent<FillEffect>().attackDuration);
        StartCoroutine(PlayClawSwipeAnimation());
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
