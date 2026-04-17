using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protect : BaseSkill
{
    public Animator animator;
    int isProtectHash;

    float ProtectStanceValue = 0.0f;
    public Coroutine enterProtectStance;
    public Coroutine exitProtectStance;
    public SkinnedMeshRenderer mannyBody;

    public Material normalManny;
    public Material armoredManny;
    public AudioClip armoredGruntSound;
    public AudioClip releaseArmorSound;

    private Color originalMannyColor;
    private Color protectStanceColor;
    [SerializeField]
    private float protectDarkenFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();
        mannyBody = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<MSAnimController>(true)[0].GetComponentsInChildren<SkinnedMeshRenderer>(true)[2];
        
        if (mannyBody != null)
        {
            originalMannyColor = mannyBody.material.color;
            protectStanceColor = new Color(originalMannyColor.r * protectDarkenFactor, originalMannyColor.g * protectDarkenFactor, originalMannyColor.b * protectDarkenFactor, originalMannyColor.a);
        }

        isProtectHash = Animator.StringToHash("isProtect");
        maxCharges = 1;
        cooldown = 40;
        skillCode = 2;
    }

    void Update()
    {
    }

    IEnumerator EnterProtectStance()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (mannyBody == null)
        {
            mannyBody = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<MSAnimController>(true)[0].GetComponentsInChildren<SkinnedMeshRenderer>(true)[2];
        }

        if (mannyBody != null)
        {
            originalMannyColor = mannyBody.material.color;
            protectStanceColor = new Color(originalMannyColor.r * protectDarkenFactor, originalMannyColor.g * protectDarkenFactor, originalMannyColor.b * protectDarkenFactor, originalMannyColor.a);
        }

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            if (mannyBody != null)
            {
                mannyBody.material.color = Color.Lerp(originalMannyColor, protectStanceColor, t);
            }
            yield return null;
        }

        if (mannyBody != null)
        {
            mannyBody.material.color = protectStanceColor;
        }
    }

    IEnumerator ExitProtectStance()
    {
        if (mannyBody == null)
        {
            mannyBody = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<MSAnimController>(true)[0].GetComponentsInChildren<SkinnedMeshRenderer>(true)[2];
        }

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            if (mannyBody != null)
            {
                mannyBody.material.color = Color.Lerp(protectStanceColor, originalMannyColor, t);
            }
            yield return null;
        }

        if (mannyBody != null)
        {
            mannyBody.material.color = originalMannyColor;
        }
    }

    public override IEnumerator RunSkill()
    {
        enterProtectStance = StartCoroutine(EnterProtectStance());
        player = GameObject.FindWithTag("Player");
        
        AudioSource.PlayClipAtPoint(armoredGruntSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        
        yield return new WaitForSeconds(1f); //Skill animation
        PlayerStats.Instance.hasProtect = true;
        yield return new WaitForSeconds(20);
        
        AudioSource.PlayClipAtPoint(releaseArmorSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        
        PlayerStats.Instance.hasProtect = false;
        exitProtectStance = StartCoroutine(ExitProtectStance());
    }
}
