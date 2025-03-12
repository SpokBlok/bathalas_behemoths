using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillQHUD : MonoBehaviour
{
    public Image dash;
    public Image mudArmor;
    public Image mudfling;
    public Image tornadoPunch;
    public Image blank;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerSkills.Instance.skillQBeingUnequipped)
        {
            ClearSkillQHUD();
            PlayerSkills.Instance.skillQBeingUnequipped = false;
        }

        if(PlayerSkills.Instance.behemothSkillQ != null && PlayerSkills.Instance.behemothSkillQ.skillCode == 1)
        {
            SetHUDToDash();
        }
        else if(PlayerSkills.Instance.behemothSkillQ != null && PlayerSkills.Instance.behemothSkillQ.skillCode == 2)
        {
            SetHUDToMudArmor();
        }
        else if(PlayerSkills.Instance.behemothSkillQ != null && PlayerSkills.Instance.behemothSkillQ.skillCode == 3)
        {
            SetHUDToMudfling();
        }
        else if(PlayerSkills.Instance.behemothSkillQ != null && PlayerSkills.Instance.behemothSkillQ.skillCode == 4)
        {
            SetHUDToTornadoPunch();
        }
    }

    public void ClearSkillQHUD()
    {
        blank.gameObject.SetActive(false);
        dash.gameObject.SetActive(false);
        mudArmor.gameObject.SetActive(false);
        mudfling.gameObject.SetActive(false);
        tornadoPunch.gameObject.SetActive(false);
    }

    public void SetHUDToDash()
    {
        ClearSkillQHUD();
        dash.gameObject.SetActive(true);
        PlayerSkills.Instance.skillBeingEquipped = false;
    }

    public void SetHUDToMudArmor()
    {
        ClearSkillQHUD();
        mudArmor.gameObject.SetActive(true);
        PlayerSkills.Instance.skillBeingEquipped = false;
    }

    public void SetHUDToMudfling()
    {
        ClearSkillQHUD();
        mudfling.gameObject.SetActive(true);
        PlayerSkills.Instance.skillBeingEquipped = false;
    }
    
    public void SetHUDToTornadoPunch()
    {
        ClearSkillQHUD();
        tornadoPunch.gameObject.SetActive(true);
        PlayerSkills.Instance.skillBeingEquipped = false;
    }
}