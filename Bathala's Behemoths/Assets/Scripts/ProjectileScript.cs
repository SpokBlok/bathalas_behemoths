using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public CharacterController charControl;

    // Start is called before the first frame update
    void Start()
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
        float projectileSpeed = 20f;
        while (elapsedTime < attackDuration)
        {
            charControl.Move(direction * projectileSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        Destroy(gameObject);
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")){
            Destroy(gameObject);
        }
    }
}
