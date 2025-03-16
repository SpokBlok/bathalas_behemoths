using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MarkupoScript : EnemyMob
{
    private GameObject player;
    public Transform target;

    public GameObject tailSwipePrefab;
    public GameObject poisonSprayPrefab;
    public GameObject HUD;
    public GameObject markyHPBar;
    public bool isSpraying;
    public bool isSpinning;
    public bool isNormStunned;
    public bool isFluteStunned;

    public float distanceFromTarget;

    private bool stunned;
    private bool isAlive;

    private Coroutine randomAttackCoroutine;

    private GameObject bulletHit;
    public SkinnedMeshRenderer[] modelRenderer;
    public markyAnim markupoModel;
    public Coroutine takingDamage;
    public GameObject endDialogue;
    private Coroutine getStunned;

    // Start is called before the first frame update
    void Start()
    {
        if (modelRenderer == null)
        {
            modelRenderer = markupoModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        player = GameObject.FindWithTag("Player");
        PlayerStats.Instance.markyScene = true;
        
        HUD = GameObject.FindWithTag("HUD");
        if (HUD != null)
        {
            // Search for markyHPBarBG inside the HUD parent
            Transform hpBarTransform = HUD.transform.Find("MarkyHPBarBG");

                if (hpBarTransform != null)
                {
                    // Activate the markyHPBarBG GameObject
                    hpBarTransform.gameObject.SetActive(true);
                    Debug.Log("MarkyHPBarBG activated!");
                }
        }

        health = 1500f;

        stunned = false;

        //Start off on terrain height
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        Vector3 newPosition = transform.position;
        newPosition.y = terrainHeight + 1.2f;
        transform.position = newPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestState.Instance.pausedForDialogue) { return; }
        if (randomAttackCoroutine == null && !stunned)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            int index = Random.Range(0, 2);

            switch (index)
            {
                case 0:
                    randomAttackCoroutine = StartCoroutine(TailSwipe());
                    break;

                case 1:
                    randomAttackCoroutine = StartCoroutine(PoisonSpray());
                    break;
            }
        }
    }

    public void GetFluteStunned()
    {
        isFluteStunned = true;
    }

    public override IEnumerator Stun(float duration)
    {       
        stunned = true;
        if(!isFluteStunned)
        {
            isNormStunned = true;
        }
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        StopAllCoroutines();
        randomAttackCoroutine = null;
        
        // stun animation
        // stunMoving = StartCoroutine(StunMovement());
        yield return new WaitForSeconds(duration);
        isNormStunned = false;
        isFluteStunned = false;
        stunned = false;
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

    private IEnumerator TailSwipe()
    {
        GameObject tailSwipe = Instantiate(tailSwipePrefab, gameObject.transform.position, Quaternion.identity);
        tailSwipe.transform.parent = transform;
        yield return new WaitForSeconds(5f); // Let the attack charge before playing animation

        isSpinning = true;
        yield return new WaitForSeconds(2f);
        isSpinning = false;

        yield return new WaitForSeconds(3f);

        randomAttackCoroutine = null;
    }

    private IEnumerator PoisonSpray()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 directionToTarget = (target.position - gameObject.transform.position).normalized;

        Vector3 spawnPosition = gameObject.transform.position + (directionToTarget * distanceFromTarget);

        GameObject poisonSpray = Instantiate(poisonSprayPrefab, 
            spawnPosition, Quaternion.LookRotation(directionToTarget));
        poisonSpray.transform.parent = transform;
        yield return new WaitForSeconds(5f); // Let the attack charge before playing animation

        isSpraying = true;
        yield return new WaitForSeconds(2f);
        isSpraying = false;

        yield return new WaitForSeconds(3f);

        randomAttackCoroutine = null;
    }
}
