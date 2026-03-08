using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillEffect : MonoBehaviour
{
    private Transform fillCircle;
    private float timer;
    public float attackDuration;
    public float finalScaleX;
    public float finalScaleY;

    private AOEAttackRadius attackRadius;
    public AudioClip thunderSound;
    public AudioClip overlaySound;

    public GameObject lightning;
    public bool lightningSpawned = false;
    public bool audioSpawned = false;

    private GameObject spawnedLightning;

    void Start()
    {
        timer = 0;
        fillCircle = transform.Find("Inside Section").GetComponent<Transform>();
        attackRadius = GetComponentInChildren<AOEAttackRadius>();
    }

    // Update is called once per frame
    void Update()
    {
        if(QuestState.Instance.pausedForDialogue) {return;} // Prevents continued function while taming dialogue is active

        if (timer <= attackDuration)
        {
            timer += Time.deltaTime;
            float currentPercentage = timer / attackDuration;
            float currentScaleX = currentPercentage * finalScaleX;
            float currentScaleY = currentPercentage * finalScaleY;
            fillCircle.localScale = new (currentScaleX, currentScaleY, 15);
        }
        else
        {
            if (!lightningSpawned && lightning != null)
            {
                spawnedLightning = Instantiate(lightning, transform.position + new Vector3(0f, 0f, -1f), Quaternion.identity);
                spawnedLightning.transform.localScale = new Vector3(15.0f, 30.0f, 15.0f);
                lightningSpawned = true;
            }

            attackRadius.Damage();
            if (!audioSpawned)
            {
                AudioSource.PlayClipAtPoint(thunderSound, transform.position, 1f);
                AudioSource.PlayClipAtPoint(overlaySound, transform.position, 1f);
                audioSpawned = true;
            }

            Destroy(spawnedLightning, 1.0f); // Destroys the lightning effect after 0.5 seconds
            Destroy(gameObject); // Destroys the fill effect after 0.5 seconds
        }
    }

    public void SetSFX(AudioClip clip1, AudioClip clip2)
    {
        thunderSound = clip1;
        overlaySound = clip2;
    }
}
