using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEHUD : MonoBehaviour
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
        if(PlayerSkills.Instance.skillEBeingUnequipped)
        {
            ClearSkillEHUD();
            PlayerSkills.Instance.skillEBeingUnequipped = false;
        }

        if(PlayerSkills.Instance.behemothSkillE != null && PlayerSkills.Instance.behemothSkillE.skillCode == 1)
        {
            SetHUDToDash();
        }
        else if(PlayerSkills.Instance.behemothSkillE != null && PlayerSkills.Instance.behemothSkillE.skillCode == 2)
        {
            SetHUDToMudArmor();
        }
        else if(PlayerSkills.Instance.behemothSkillE != null && PlayerSkills.Instance.behemothSkillE.skillCode == 3)
        {
            SetHUDToMudfling();
        }
        else if(PlayerSkills.Instance.behemothSkillE != null && PlayerSkills.Instance.behemothSkillE.skillCode == 4)
        {
            SetHUDToTornadoPunch();
        }
    }

    public void ClearSkillEHUD()
    {
        blank.gameObject.SetActive(false);
        dash.gameObject.SetActive(false);
        mudArmor.gameObject.SetActive(false);
        mudfling.gameObject.SetActive(false);
        tornadoPunch.gameObject.SetActive(false);
    }

    public void SetHUDToDash()
    {
        ClearSkillEHUD();
        dash.gameObject.SetActive(true);
        PlayerSkills.Instance.skillBeingEquipped = false;
    }

    public void SetHUDToMudArmor()
    {
        ClearSkillEHUD();
        mudArmor.gameObject.SetActive(true);
        PlayerSkills.Instance.skillBeingEquipped = false;
    }

    public void SetHUDToMudfling()
    {
        ClearSkillEHUD();
        mudfling.gameObject.SetActive(true);
        PlayerSkills.Instance.skillBeingEquipped = false;
    }
    
    public void SetHUDToTornadoPunch()
    {
        ClearSkillEHUD();
        tornadoPunch.gameObject.SetActive(true);
        PlayerSkills.Instance.skillBeingEquipped = false;
    }
}
