using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Apolaki : EnemyMob
{
    private GameObject player;
    public GameObject endDialogue;

    //Attack prefabs
    public GameObject meleeAttackPrefab; // Formerly clawSwipePrefab
    public GameObject meteorShowerPrefab; // Formerly trailingLightningPrefab
    public GameObject crossSlashPrefab; // Formerly arenaWideLightingPrefab
    public GameObject ultimatePrefab; // Formerly massiveAOEPrefab
    public GameObject HUD; // CHANGE to apolakiHPBar Canvas

    private Coroutine randomAttackCoroutine;
    private Coroutine getStunned;
    private Coroutine stunMoving;
    private Coroutine blinking;
    private bool isAlive;
    public bool stunned;

    public int attacksPassed;

    public float duration;

    public GameObject appy;
    public GameObject stunSymbol;
    public bool isMeleeAttacking;
    public bool isMeteorShower;
    public bool isCrossSlash;
    public bool isUltimate;
    public bool isStunned;

    public SkinnedMeshRenderer[] modelRenderer;
    public ApolakiAnimController apolakiModel;
    public Coroutine takingDamage;

    // Start is called before the first frame update
    void Start()
    {
        if (modelRenderer == null)
        {
            modelRenderer = apolakiModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        player = GameObject.FindWithTag("Player");
        appy = GameObject.FindWithTag("ApolakiModel");

        HUD = GameObject.FindWithTag("HUD"); // Gets cut off if the apolakiModel from above is not assigned (like a break())
        if (HUD != null)
        {
            // Search for apolakiHPBarBG inside the HUD parent
            Transform hpBarTransform = HUD.transform.Find("ApolakiHPBarBG");

                if (hpBarTransform != null)
                {
                    // Activate the apolakiHPBarBG GameObject
                    hpBarTransform.gameObject.SetActive(true);
                    Debug.Log("ApolakiHPBarBG activated!");
                }
        }

        health = 6000f;
        // Special QuestState flag/s to make Apolaki fight easier
        if(QuestState.Instance.tambanokanoDefeated && QuestState.Instance.markupoDefeated)
        {
            health = 3500f;
            QuestState.Instance.goodEnding = true;
        }
        else
        {
            health = 6000f;
            QuestState.Instance.goodEnding = false;
        }

        isMeteorShower = false;
        isAlive = true;
        isCrossSlash = false;
        isMeleeAttacking = false;
        stunned = false;
        isUltimate = false;
        isStunned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(QuestState.Instance.pausedForDialogue) {return;}
        if(!PlayerStats.Instance.apolakiScene)
        {
            PlayerStats.Instance.apolakiScene = true;
        }
        if (randomAttackCoroutine == null && !isUltimate && !stunned)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            isMeteorShower = false;
            isCrossSlash = false;
            int index = Random.Range(0, 3);
            if (attacksPassed >= 10)
            {
                index = 3;
            }
            attacksPassed++;

            switch(index)
            {
                case 0:
                    randomAttackCoroutine = StartCoroutine(MeleeAttack());
                    break;

                case 1:
                    randomAttackCoroutine = StartCoroutine(MeteorShower());
                    isMeteorShower = true;
                    break;

                case 2:
                    randomAttackCoroutine = StartCoroutine(CrossSlash());
                    break;

                case 3:
                    randomAttackCoroutine = StartCoroutine(UltimateAttack());
                    attacksPassed = 0;
                    break;
            }
        }
    }

    public void GetMudStunned()
    {
        getStunned = StartCoroutine(Stun(3));
    }

    public override IEnumerator Stun(float duration)
    {
        stunSymbol.SetActive(true);
        isStunned = true;
        if (isMeteorShower)
        {
            stunned = true;
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            StopAllCoroutines();
            randomAttackCoroutine = null;
            isMeteorShower = false;
            isUltimate = false;
        }
        //stun animation
        stunMoving = StartCoroutine(StunMovement());
        yield return new WaitForSeconds(duration);
        stunned = false;
        isStunned = false;
        
        stunSymbol.SetActive(false);
    }

    IEnumerator StunMovement()
    {
        float degreeRot = 0.0f;
        float duration = 0.4f;
        float elapsedTime = 0.0f;
        float startValue = 0.0f;
        float endValue = -10.0f;
        Debug.Log("Apolaki Stun Movement Start");

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            degreeRot = Mathf.Lerp(startValue, endValue, t);
            appy.transform.rotation = Quaternion.Euler(degreeRot, 180, 0);
            yield return null;
        }
        
        degreeRot = endValue; // Ensure it fully reaches the target
        appy.transform.rotation = Quaternion.Euler(degreeRot, 180, 0);
        elapsedTime = 0.0f;
        yield return new WaitForSeconds(2.2f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            degreeRot = Mathf.Lerp(endValue, startValue, t);
            appy.transform.rotation = Quaternion.Euler(degreeRot, 180, 0);
            yield return null;
        }
        
        degreeRot = startValue; // Ensure it fully reaches the target
        appy.transform.rotation = Quaternion.Euler(degreeRot, 180, 0);
    }

    IEnumerator SwitchToDamagedTex()
    {
        foreach(SkinnedMeshRenderer bodypart in modelRenderer)
        {
            bodypart.material.color = Color.red; // Change color to red
        }
        
        yield return new WaitForSeconds(0.1f); // Wait

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
            QuestState.Instance.apolakiDefeated = true;

            if(isAlive == false)
            {
                StopAllCoroutines();
                if(QuestState.Instance.tambanokanoDefeated && QuestState.Instance.markupoDefeated)
                {
                    SceneManager.LoadScene("ApolakiDefeatScene");
                }
                else
                {
                    SceneManager.LoadScene("ApolakiDefeatBadEndScene");
                }
                // endDialogue.SetActive(true);
            }
        }
    }

    private IEnumerator SingleMeleeAttack()
    {
        //attack animation
        GameObject spear = Instantiate(meleeAttackPrefab, new Vector3(Random.Range(400f, 500f), 170f, Random.Range(285f, 425f)), Quaternion.Euler(0f, 90f, 0f));
        spear.transform.parent = transform;
        yield return new WaitForSeconds(spear.GetComponent<FillEffect>().attackDuration);
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator PlayMeleeAttackAnimation()
    {
        isMeleeAttacking = true;
        yield return new WaitForSeconds(0.5f);
        isMeleeAttacking = false;
    }

    private IEnumerator MoveInFrontOfPlayer(float distance, float dashDuration)
    {
        if (player == null)
        {
            yield break;
        }

        Vector3 playerPosition = player.transform.position;
        Vector3 playerForward = player.transform.forward;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        Vector3 targetPosition = playerPosition - playerForward * distance;
        targetPosition.y = startPosition.y;

        // Face the player only on the horizontal plane (no vertical tilt)
        Vector3 toPlayer = playerPosition - targetPosition;
        toPlayer.y = 0f;
        Quaternion targetRotation = transform.rotation;
        if (toPlayer.sqrMagnitude > 0.0001f)
        {
            targetRotation = Quaternion.LookRotation(toPlayer, Vector3.up);
        }

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dashDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            
            appy.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
        appy.transform.position = targetPosition;
        appy.transform.rotation = targetRotation;
    }

    private IEnumerator MeleeAttack()
    {
        // Quickly move Apolaki in front of the player before attacking
        yield return StartCoroutine(MoveInFrontOfPlayer(3f, 1.0f));

        //attack animation
        GameObject spear = Instantiate(meleeAttackPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0f, 90f, 0f));
        spear.transform.parent = transform;
        yield return new WaitForSeconds(spear.GetComponent<FillEffect>().attackDuration);
        StartCoroutine(PlayMeleeAttackAnimation());

        yield return StartCoroutine(MoveInFrontOfPlayer(3f, 1.0f));
        //attack animation
        spear = Instantiate(meleeAttackPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0f, 90f, 0f));
        yield return new WaitForSeconds(spear.GetComponent<FillEffect>().attackDuration);
        StartCoroutine(PlayMeleeAttackAnimation());
        yield return new WaitForSeconds(2f);

        randomAttackCoroutine = null;
    }

    private IEnumerator MeteorShower()
    {
        isMeteorShower = true;
        
        GameObject meteor = Instantiate(meteorShowerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        meteor.transform.parent = transform;
        TrailingLightningStrike meteorComponent = meteor.GetComponent<TrailingLightningStrike>();
        if (meteorComponent == null)
        {
            Debug.LogError("TrailingLightningStrike component not found on Meteor Shower prefab.");
            yield break; // Exit the coroutine if the component is not found
        }

        duration = meteorComponent.attackGapDuration * (meteorComponent.attacksLeft);
        
        yield return new WaitForSeconds(duration + 7f);

        isMeteorShower = false;
        randomAttackCoroutine = null;
    }

    private IEnumerator CrossSlash()
    {
        isCrossSlash = true;

        GameObject slash = Instantiate(crossSlashPrefab, 
            new Vector3(Random.Range(500f, 800f), 1000f, Random.Range(400f, 800f)),
            Quaternion.Euler(0f, Random.Range(0f, 90f), 0f));
        slash.transform.parent = transform;
        yield return new WaitForSeconds(6f);

        randomAttackCoroutine = null;
    }

    private IEnumerator UltimateAttack()
    {
        isUltimate = true;
        StartCoroutine(MeteorShower());
        StartCoroutine(CrossSlash());

        yield return new WaitForSeconds(7);
        StartCoroutine(MeleeAttack());

        yield return new WaitForSeconds(1.25f);
        StartCoroutine(MeteorShower());

        yield return new WaitForSeconds(6);
        StartCoroutine(CrossSlash());

        yield return new WaitForSeconds(9);
        GameObject ultimate = Instantiate(ultimatePrefab, new Vector3(1000f, 160f, 700f), Quaternion.identity);
        ultimate.transform.parent = transform;

        yield return new WaitForSeconds(14);

        isUltimate = false;
        randomAttackCoroutine = null;
    }
    
}


