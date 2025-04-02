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
    public Image blankReady;

    public AudioClip dashSound;
    public AudioClip mudArmorSound;
    public AudioClip mudFlingSound;
    public AudioClip tornadoPunchSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerSkills.Instance.behemothSkillECharges > 0)
        {
            blankReady.gameObject.SetActive(true);
        }
        else
        {
            blankReady.gameObject.SetActive(false);
        }

        if(PlayerSkills.Instance.skillEBeingUnequipped)
        {
            ClearSkillEHUD();
            PlayerSkills.Instance.skillEBeingUnequipped = false;
        }

        if(PlayerSkills.Instance.behemothSkillE != null && PlayerSkills.Instance.behemothSkillE.skillCode == 1)
        {
            SetHUDToDash();
            PlayerStats.Instance.skillESound = dashSound;
        }
        else if(PlayerSkills.Instance.behemothSkillE != null && PlayerSkills.Instance.behemothSkillE.skillCode == 2)
        {
            SetHUDToMudArmor();
            PlayerStats.Instance.skillESound = mudArmorSound;
        }
        else if(PlayerSkills.Instance.behemothSkillE != null && PlayerSkills.Instance.behemothSkillE.skillCode == 3)
        {
            SetHUDToMudfling();
            PlayerStats.Instance.skillESound = mudFlingSound;
        }
        else if(PlayerSkills.Instance.behemothSkillE != null && PlayerSkills.Instance.behemothSkillE.skillCode == 4)
        {
            SetHUDToTornadoPunch();
            PlayerStats.Instance.skillESound = tornadoPunchSound;
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
        PlayerSkills.Instance.skillEBeingEquipped = false;
    }

    public void SetHUDToMudArmor()
    {
        ClearSkillEHUD();
        mudArmor.gameObject.SetActive(true);
        PlayerSkills.Instance.skillEBeingEquipped = false;
    }

    public void SetHUDToMudfling()
    {
        ClearSkillEHUD();
        mudfling.gameObject.SetActive(true);
        PlayerSkills.Instance.skillEBeingEquipped = false;
    }
    
    public void SetHUDToTornadoPunch()
    {
        ClearSkillEHUD();
        tornadoPunch.gameObject.SetActive(true);
        PlayerSkills.Instance.skillEBeingEquipped = false;
    }
}
