using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Protect : BaseSkill
{
    public Animator animator;
    int isProtectHash;

    float ProtectStanceValue = 0.0f;
    public Coroutine enterProtectStance;
    public Coroutine exitProtectStance;
    [FormerlySerializedAs("mannyBody")]
    public SkinnedMeshRenderer tambanokanoBody;

    public Material normalManny;
    public Material armoredManny;
    public AudioClip armoredGruntSound;
    public AudioClip releaseArmorSound;

    private Color originalTambanokanoColor;
    private Color protectStanceColor;
    [SerializeField]
    private float protectDarkenFactor = 0.5f;

    private bool TryResolveTambanokanoBody()
    {
        if (tambanokanoBody != null)
        {
            return true;
        }

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (player == null)
        {
            return false;
        }

        TSAnimController tambanokanoModel = player.GetComponentInChildren<TSAnimController>(true);
        if (tambanokanoModel == null)
        {
            tambanokanoModel = FindAnyObjectByType<TSAnimController>(FindObjectsInactive.Include);
        }

        if (tambanokanoModel == null)
        {
            return false;
        }

        SkinnedMeshRenderer[] renderers = tambanokanoModel.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            if (renderer != null && renderer.enabled)
            {
                tambanokanoBody = renderer;
                return true;
            }
        }

        if (renderers.Length > 0)
        {
            tambanokanoBody = renderers[0];
        }

        return tambanokanoBody != null;
    }

    private void CacheProtectStanceColors()
    {
        if (!TryResolveTambanokanoBody())
        {
            return;
        }

        originalTambanokanoColor = tambanokanoBody.material.color;
        protectStanceColor = new Color(
            originalTambanokanoColor.r * protectDarkenFactor,
            originalTambanokanoColor.g * protectDarkenFactor,
            originalTambanokanoColor.b * protectDarkenFactor,
            originalTambanokanoColor.a);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            animator = player.GetComponentInChildren<Animator>();
        }

        CacheProtectStanceColors();

        isProtectHash = Animator.StringToHash("isProtect");
        maxCharges = 1;
        cooldown = 25;
        skillCode = 6;
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

        CacheProtectStanceColors();

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            if (tambanokanoBody != null)
            {
                tambanokanoBody.material.color = Color.Lerp(originalTambanokanoColor, protectStanceColor, t);
            }
            yield return null;
        }

        if (tambanokanoBody != null)
        {
            tambanokanoBody.material.color = protectStanceColor;
        }
    }

    IEnumerator ExitProtectStance()
    {
        TryResolveTambanokanoBody();

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            if (tambanokanoBody != null)
            {
                tambanokanoBody.material.color = Color.Lerp(protectStanceColor, originalTambanokanoColor, t);
            }
            yield return null;
        }

        if (tambanokanoBody != null)
        {
            tambanokanoBody.material.color = originalTambanokanoColor;
        }
    }

    public override IEnumerator RunSkill()
    {
        enterProtectStance = StartCoroutine(EnterProtectStance());
        player = GameObject.FindWithTag("Player");
        
        AudioSource.PlayClipAtPoint(armoredGruntSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        
        yield return new WaitForSeconds(1f); //Skill animation
        PlayerStats.Instance.hasProtect = true;
        PlayerStats.Instance.speedMultiplier -= 0.2f;
        yield return new WaitForSeconds(10);
        
        AudioSource.PlayClipAtPoint(releaseArmorSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        
        PlayerStats.Instance.hasProtect = false;
        PlayerStats.Instance.speedMultiplier += 0.2f;
        exitProtectStance = StartCoroutine(ExitProtectStance());
    }
}
