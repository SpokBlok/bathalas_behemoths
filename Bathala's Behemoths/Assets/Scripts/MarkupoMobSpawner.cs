using UnityEngine;

public class MarkupoMobSpawner : MonoBehaviour
{
    public GameObject kaprePlayerPrefab;
    public Transform encounterCenter;
    public float minSpawnRadius = 15f;
    public float maxSpawnRadius = 35f;
    public float terrainOffset = 1.2f;
    public int initialSpawnCount = 5;
    public int respawnCount = 1;

    private float timer;
    public float spawnGap;

    // Start is called before the first frame update
    void Start()
    {
        if (encounterCenter == null)
        {
            GameObject markupo = GameObject.FindGameObjectWithTag("Markupo");
            if (markupo != null)
            {
                encounterCenter = markupo.transform;
            }
        }

        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnKapre();
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
            for (int i = 0; i < respawnCount; i++)
            {
                SpawnKapre();
            }
        }
    }

    void SpawnKapre()
    {
        if (kaprePlayerPrefab == null)
        {
            return;
        }

        Vector3 center = encounterCenter != null ? encounterCenter.position : transform.position;
        Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnRadius, maxSpawnRadius);
        Vector3 spawnPosition = new(center.x + randomCircle.x, center.y, center.z + randomCircle.y);

        if (Terrain.activeTerrain != null)
        {
            spawnPosition.y = Terrain.activeTerrain.SampleHeight(spawnPosition) + terrainOffset;
        }

        Instantiate(kaprePlayerPrefab, spawnPosition, Quaternion.Euler(0f, 90f, 0f), transform);
    }
}
