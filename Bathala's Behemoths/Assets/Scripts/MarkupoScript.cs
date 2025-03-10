using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MarkupoScript : MonoBehaviour
{
    private GameObject player;
    public Transform target;

    public GameObject tailSwipePrefab;
    public GameObject poisonSprayPrefab;

    public float distanceFromTarget;

    private Coroutine randomAttackCoroutine;

    public float health;
    private GameObject bulletHit;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        health = 2500f;

        StartCoroutine(PoisonSpray());

        //Start off on terrain height
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        Vector3 newPosition = transform.position;
        newPosition.y = terrainHeight + 1.2f;
        transform.position = newPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Trigger win cutscene
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bulletHit != other.gameObject && other.gameObject.CompareTag("Projectile"))
        {
            bulletHit = other.gameObject;
            TakeDamage(PlayerStats.Instance.basicAttackDamage);
        }
    }

    private IEnumerator TailSwipe()
    {
        GameObject tailSwipe = Instantiate(tailSwipePrefab, gameObject.transform.position, Quaternion.identity);
        tailSwipe.transform.parent = transform;
        yield return null;
    }

    private IEnumerator PoisonSpray()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 directionToTarget = (target.position - gameObject.transform.position).normalized;

        Vector3 spawnPosition = gameObject.transform.position + (directionToTarget * distanceFromTarget);

        GameObject poisonSpray = Instantiate(poisonSprayPrefab, 
            spawnPosition, Quaternion.LookRotation(directionToTarget));
        poisonSpray.transform.parent = transform;

        yield return null;
    }
}
