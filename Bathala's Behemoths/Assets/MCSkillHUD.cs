using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MCSkillHUD : MonoBehaviour
{
    public Image bathBless;
    public Image firstAid;
    public Image musicalFlute;
    public Image blank;

    public AudioClip bbSound;
    public AudioClip firstAidSound;
    public AudioClip musicalFluteSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerSkills.Instance.skillMCBeingUnequipped)
        {
            ClearMCSkillHUD();
            PlayerSkills.Instance.skillMCBeingUnequipped = false;
        }

        if(PlayerSkills.Instance.mainCharacterSkill != null && PlayerSkills.Instance.mainCharacterSkill.skillCode == 1)
        {
            SetHUDToBathBless();
            PlayerStats.Instance.mcSkillSound = bbSound;
        }
        else if(PlayerSkills.Instance.mainCharacterSkill != null && PlayerSkills.Instance.mainCharacterSkill.skillCode == 2)
        {
            SetHUDToFirstAid();
            PlayerStats.Instance.mcSkillSound = firstAidSound;
        }
        else if(PlayerSkills.Instance.mainCharacterSkill != null && PlayerSkills.Instance.mainCharacterSkill.skillCode == 3)
        {
            SetHUDToMusicalFlute();
            PlayerStats.Instance.mcSkillSound = musicalFluteSound;
        }
    }

    public void ClearMCSkillHUD()
    {
        blank.gameObject.SetActive(false);
        bathBless.gameObject.SetActive(false);
        firstAid.gameObject.SetActive(false);
        musicalFlute.gameObject.SetActive(false);
    }

    public void SetHUDToBathBless()
    {
        ClearMCSkillHUD();
        bathBless.gameObject.SetActive(true);
        PlayerSkills.Instance.skillMCBeingEquipped = false;
    }

    public void SetHUDToFirstAid()
    {
        ClearMCSkillHUD();
        firstAid.gameObject.SetActive(true);
        PlayerSkills.Instance.skillMCBeingEquipped = false;
    }

    public void SetHUDToMusicalFlute()
    {
        ClearMCSkillHUD();
        musicalFlute.gameObject.SetActive(true);
        PlayerSkills.Instance.skillMCBeingEquipped = false;
    }
}
