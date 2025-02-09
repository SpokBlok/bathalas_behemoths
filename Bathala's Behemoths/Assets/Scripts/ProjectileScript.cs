using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public CharacterController charControl;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        charControl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Move(Vector3 direction)
    {
        float attackDuration = 0.5f;
        float elapsedTime = 0f;
        float projectileSpeed = 50f;
        while ((elapsedTime < attackDuration))
        {
            TerrainGravity();
            charControl.Move(direction * projectileSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        Destroy(gameObject);
        yield break;
    }

    public void TerrainGravity()
    {
        Vector3 position = charControl.transform.position;

        // Get the terrain height at the character's current position (X, Z)
        float terrainHeight = Terrain.activeTerrain.SampleHeight(position);

        // Set the character's Y position to match the terrain height + 1, more if berserk
        position.y = terrainHeight + 1.2f;

        charControl.transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Tambanokano"))
        {
            EnemyMob enemy = other.GetComponent<EnemyMob>();
            if (enemy.health - PlayerStats.Instance.basicAttackDamage > 0)
            {
                enemy.StartCoroutine(other.gameObject.GetComponent<EnemyMob>().Stun(2));
            }
            enemy.TakeDamage(PlayerStats.Instance.basicAttackDamage);
        }
    }
}
