using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudArmor : BaseSkill
{
    public Animator animator;
    int isMudArmorHash;

    float mudStanceValue = 0.0f;
    public Coroutine enterMudStance;
    public Coroutine exitMudStance;
    public SkinnedMeshRenderer mannyBody;

    public Material normalManny;
    public Material armoredManny;
    public AudioClip armoredGruntSound;
    public AudioClip releaseArmorSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();
        mannyBody = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<MSAnimController>(true)[0].GetComponentsInChildren<SkinnedMeshRenderer>(true)[2];
        
        isMudArmorHash = Animator.StringToHash("isMudarmor");
        maxCharges = 1;
        cooldown = 40;
        skillCode = 2;
    }

    void Update()
    {
    }

    IEnumerator EnterMudStance()
    {
        player = GameObject.FindWithTag("Player");
        if(mannyBody == null)
        {
            mannyBody = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<MSAnimController>(true)[0].GetComponentsInChildren<SkinnedMeshRenderer>(true)[2];
        }
        mannyBody.material = armoredManny;
        animator = player.GetComponentInChildren<Animator>();
        
        float duration = 0.5f;
        float elapsedTime = 0f;
        float startValue = 0.0f;
        float endValue = 1.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            mudStanceValue = Mathf.Lerp(startValue, endValue, t);
            animator.SetFloat(isMudArmorHash, mudStanceValue);
            yield return null;
        }
        
        mudStanceValue = endValue; // Ensure it fully reaches the target
        animator.SetFloat(isMudArmorHash, mudStanceValue);
    }

    IEnumerator ExitMudStance()
    {
        if(mannyBody == null)
        {
            mannyBody = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<MSAnimController>(true)[0].GetComponentsInChildren<SkinnedMeshRenderer>(true)[2];
        }
        mannyBody.material = normalManny;

        float duration = 0.5f;
        float elapsedTime = 0f;
        float startValue = 1.0f;
        float endValue = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            mudStanceValue = Mathf.Lerp(startValue, endValue, t);
            animator.SetFloat(isMudArmorHash, mudStanceValue);
            yield return null;
        }
        
        mudStanceValue = endValue; // Ensure it fully reaches the target
        animator.SetFloat(isMudArmorHash, mudStanceValue);
    }

    public override IEnumerator RunSkill()
    {
        enterMudStance = StartCoroutine(EnterMudStance());
        animator.SetFloat(isMudArmorHash, 1.0f);
        player = GameObject.FindWithTag("Player");
        
        AudioSource.PlayClipAtPoint(armoredGruntSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        
        yield return new WaitForSeconds(1f); //Skill animation
        PlayerStats.Instance.hasMudArmor = true;
        yield return new WaitForSeconds(20);
        
        AudioSource.PlayClipAtPoint(releaseArmorSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        
        PlayerStats.Instance.hasMudArmor = false;
        exitMudStance = StartCoroutine(ExitMudStance());
    }
}
