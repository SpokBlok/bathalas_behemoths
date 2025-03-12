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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerSkills.Instance.mainCharacterSkill != null && PlayerSkills.Instance.mainCharacterSkill.skillCode == 1)
        {
            SetHUDToBathBless();
        }
        else if(PlayerSkills.Instance.mainCharacterSkill != null && PlayerSkills.Instance.mainCharacterSkill.skillCode == 2)
        {
            SetHUDToFirstAid();
        }
        else if(PlayerSkills.Instance.mainCharacterSkill != null && PlayerSkills.Instance.mainCharacterSkill.skillCode == 3)
        {
            SetHUDToMusicalFlute();
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
        PlayerSkills.Instance.skillBeingEquipped = false;
    }

    public void SetHUDToFirstAid()
    {
        ClearMCSkillHUD();
        firstAid.gameObject.SetActive(true);
        PlayerSkills.Instance.skillBeingEquipped = false;
    }

    public void SetHUDToMusicalFlute()
    {
        ClearMCSkillHUD();
        musicalFlute.gameObject.SetActive(true);
        PlayerSkills.Instance.skillBeingEquipped = false;
    }
}
