using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillQCD : MonoBehaviour
{
    public Image img;
    public Coroutine coolingDown;
    public bool skillInCooldown = false;
    public float cdTime;

    // Start is called before the first frame update
    void Start()
    {
        img.fillAmount = 0.0f;
        cdTime = PlayerSkills.Instance.behemothSkillQChargeTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerSkills.Instance.skillBeingEquipped || cdTime == 0)
        {
            cdTime = PlayerSkills.Instance.behemothSkillQChargeTimer;
        }
        
        if(PlayerSkills.Instance.skillCooldownStart != null && PlayerSkills.Instance.mainCharacterSkillCharges != null && PlayerSkills.Instance.mainCharacterSkill != null)
        {
            if((PlayerSkills.Instance.skillCooldownStart && !skillInCooldown) && (PlayerSkills.Instance.behemothSkillQCharges != PlayerSkills.Instance.behemothSkillQ.maxCharges))
            {
                CoolDownStart();
            }
        }
    }

    public void CoolDownStart()
    {
        if(coolingDown == null && !QuestState.Instance.pausedForDialogue)
        {
            coolingDown = StartCoroutine(CoolDown());
        }
        else if(coolingDown != null && !QuestState.Instance.pausedForDialogue)
        {
            StopCoroutine(coolingDown);
            coolingDown = StartCoroutine(CoolDown());
        }
    }

    IEnumerator CoolDown()
    {
        Debug.Log("Skill Q Cooling Down");
        skillInCooldown = true;

        float duration = cdTime;
        float elapsedTime = 0f;
        float startValue = 1.0f;
        float endValue = 0.0f;
        img.fillAmount = startValue;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            img.fillAmount = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }
        
        img.fillAmount = endValue; // Ensure it fully reaches the target
        skillInCooldown = false;
    }
}
