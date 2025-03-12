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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();
        
        isMudArmorHash = Animator.StringToHash("isMudarmor");
        maxCharges = 1;
        cooldown = 40;
        skillCode = 2;
    }

    IEnumerator EnterMudStance()
    {
        player = GameObject.FindWithTag("Player");
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
        yield return new WaitForSeconds(1f); //Skill animation
        PlayerStats.Instance.hasMudArmor = true;
        yield return new WaitForSeconds(20);
        PlayerStats.Instance.hasMudArmor = false;
        exitMudStance = StartCoroutine(ExitMudStance());
    }
}
