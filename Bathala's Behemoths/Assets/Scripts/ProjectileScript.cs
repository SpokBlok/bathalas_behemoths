using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public CharacterController charControl;
    public AudioClip ballHit;
    private Tambanokano tammy;

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
        float projectileSpeed = 200f;
        while ((elapsedTime < attackDuration))
        {
            charControl.Move(direction * projectileSpeed * Time.deltaTime);
            TerrainGravity();
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        Destroy(gameObject);
        yield break;
    }

    public void TerrainGravity()
    {
        Vector3 position = charControl.transform.position;
        Terrain myTerrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        // Get the terrain height at the character's current position (X, Z)
        float terrainHeight = myTerrain.SampleHeight(position);

        // Set the character's Y position to match the terrain height + 1, more if berserk
        position.y = terrainHeight + 1.2f;

        charControl.transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Tambanokano") || other.gameObject.CompareTag("Tambanokano"))
        {
            EnemyMob enemy = other.GetComponent<EnemyMob>();
            if(PlayerStats.Instance.tammyScene)
            {
                tammy = GameObject.FindWithTag("Tambanokano").GetComponent<Tambanokano>();
            }

            if (enemy.health - PlayerStats.Instance.basicAttackDamage > 0)
            {
                Debug.Log("Kapre Collided With, Stunning it!!!");
                enemy.StartCoroutine(other.gameObject.GetComponent<EnemyMob>().Stun(3));
                AudioSource.PlayClipAtPoint(ballHit, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
            }
            enemy.TakeDamage(PlayerStats.Instance.basicAttackDamage);

            if(tammy != null)
            {
                tammy.GetMudStunned();
            }
        }
    }
}
