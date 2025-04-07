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
            attackRadius.Damage();
            AudioSource.PlayClipAtPoint(thunderSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
            AudioSource.PlayClipAtPoint(overlaySound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
            
            Destroy(gameObject);
        }
    }

    public void SetSFX(AudioClip clip1, AudioClip clip2)
    {
        thunderSound = clip1;
        overlaySound = clip2;
    }
}
