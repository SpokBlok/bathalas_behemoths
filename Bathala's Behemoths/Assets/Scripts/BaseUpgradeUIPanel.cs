using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUpgradeUIPanel : MonoBehaviour
{
    public BaseUpgradeUIPanel[] uiList;
    public BaseUpgradeUIPanel baseUI;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnablePanel()
    {
        gameObject.SetActive(true);
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
        Transform panel = transform.Find("RightPanel");
        foreach (Transform child in panel)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void DisplaySkill1()
    {
        Transform panel = transform.Find("RightPanel");
        foreach (Transform child in panel)
        {
            if (child.name == "Skill 1")
            {
                child.gameObject.SetActive(true);
            }
            else { 
                child.gameObject.SetActive(false);
            }
        }
    }

    public void Skill1Upgrade()
    {
        PlayerStats.Instance.dashSkillEquipped = true;
        PlayerStats.Instance.rangedSkillEquipped = false;
        PlayerStats.Instance.noSkillEquipped = false;
    }

    public void DisplaySkill2()
    {
        Transform panel = transform.Find("RightPanel");
        foreach (Transform child in panel)
        {
            if (child.name == "Skill 2")
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void Skill2Upgrade()
    {
        PlayerStats.Instance.dashSkillEquipped = false;
        PlayerStats.Instance.rangedSkillEquipped = true;
        PlayerStats.Instance.noSkillEquipped = false;
    }

    public void DisplayUlt1()
    {
        Transform panel = transform.Find("RightPanel");
        foreach (Transform child in panel)
        {
            if (child.name == "Ult 1")
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void Ult1Upgrade()
    {
        PlayerStats.Instance.rangedUltEquipped = true;
        PlayerStats.Instance.berserkUltEquipped = false;
        PlayerStats.Instance.noSkillEquipped = false;
    }

    public void DisplayUlt2()
    {
        Transform panel = transform.Find("RightPanel");
        foreach (Transform child in panel)
        {
            if (child.name == "Ult 2")
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void Ult2Upgrade()
    {
        PlayerStats.Instance.rangedUltEquipped = false;
        PlayerStats.Instance.berserkUltEquipped = true;
        PlayerStats.Instance.noSkillEquipped = false;
    }
}
