using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MCSkillCD : MonoBehaviour
{
    public Image img;
    public Coroutine coolingDown;
    public float cdTime;

    // Start is called before the first frame update
    void Start()
    {
        img.fillAmount = 1.0f;
        cdTime = PlayerSkills.Instance.mainCharacterSkillChargeTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if(cdTime != PlayerSkills.Instance.mainCharacterSkillChargeTimer)
        {
            // Debug.Log("cdTime setup");
            cdTime = PlayerSkills.Instance.mainCharacterSkillChargeTimer;
        }

        if(PlayerSkills.Instance.skillMCBeingEquipped)
        {
            // Debug.Log("MC skill change to new skill");
            img.fillAmount = 1.0f;
            cdTime = PlayerSkills.Instance.mainCharacterSkillChargeTimer;
            PlayerSkills.Instance.skillMCCDStart = false;
        }

        if(PlayerSkills.Instance.mainCharacterSkillCharges != null && PlayerSkills.Instance.mainCharacterSkill != null)
        {   
            // Debug.Log("Calling MC Skill stuff not null");
            if(PlayerSkills.Instance.skillMCCDStart)
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
        // Debug.Log("MC Skill Cooling Down");

        float duration = cdTime;
        float elapsedTime = 0f;
        float startValue = 0.0f;
        float endValue = 1.0f;
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
