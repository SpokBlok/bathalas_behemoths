using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillECD : MonoBehaviour
{
    public Image img;
    public Coroutine coolingDown;
    public float cdTime;

    // Start is called before the first frame update
    void Start()
    {
        img.fillAmount = 0.0f;
        cdTime = PlayerSkills.Instance.behemothSkillEChargeTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if(cdTime != PlayerSkills.Instance.behemothSkillEChargeTimer)
        {
            cdTime = PlayerSkills.Instance.behemothSkillEChargeTimer;
        }

        if(PlayerSkills.Instance.skillEBeingEquipped)
        {
            img.fillAmount = 0.0f;
            cdTime = PlayerSkills.Instance.behemothSkillEChargeTimer;
            PlayerSkills.Instance.skillECDStart = false;
        }

        if(PlayerSkills.Instance.behemothSkillECharges != null && PlayerSkills.Instance.behemothSkillE != null)
        {    
            if(PlayerSkills.Instance.skillECDStart)
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
    }

    IEnumerator CoolDown()
    {
        // Debug.Log("Skill E Cooling Down");

        if (!PlayerSkills.Instance.skillECDStart)
        {
            coolingDown = null;
            yield break; // Exit the coroutine if skillECDStart is false
        }

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
        coolingDown = null;
    }
}
