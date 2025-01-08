using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseUpgradeUIPanel : MonoBehaviour
{
    public BaseUpgradeUIPanel[] uiList;
    public BaseUpgradeUIPanel baseUI;

    //Purchased bools
    private bool Skill1Purchased;
    private bool Skill2Purchased;
    private bool Ult1Purchased;
    private bool Ult2Purchased;

    //Texts for purchase buttons
    private TextMeshProUGUI Skill1Text;
    private TextMeshProUGUI Skill2Text;
    private TextMeshProUGUI Ult1Text;
    private TextMeshProUGUI Ult2Text;

    // Start is called before the first frame update
    void Start()
    {
        Skill1Purchased = false;
        Skill2Purchased = false;
        Ult1Purchased = false;
        Ult2Purchased = false;

        Skill1Text = transform.Find("RightPanel/Skill 1/Skill 1 Purchase Button")
            .GetComponentInChildren<TextMeshProUGUI>();
        Skill2Text = transform.Find("RightPanel/Skill 2/Skill 2 Purchase Button")
            .GetComponentInChildren<TextMeshProUGUI>();
        Ult1Text = transform.Find("RightPanel/Ult 1/Ult 1 Purchase Button")
                .GetComponentInChildren<TextMeshProUGUI>();
        Ult2Text = transform.Find("RightPanel/Ult 2/Ult 2 Purchase Button")
                .GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnablePanel()
    {
        gameObject.SetActive(true);
        EventManager.Instance.InvokeOnEnteringUpgradeScreen();
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
        EventManager.Instance.InvokeOnExitingUpgradeScreen();
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
        if (Skill1Purchased)
        {
            Skill1Text.text = "Equipped";
            PlayerStats.Instance.dashSkillEquipped = true;
            PlayerStats.Instance.rangedSkillEquipped = false;
            PlayerStats.Instance.noSkillEquipped = false;
            if (Skill2Purchased)
            {
                Skill2Text.text = "Equip";
            }
        } 
        else if (PlayerStats.Instance.kapreCigars > 5 && !Skill1Purchased)
        {
            Skill1Purchased = true;
            PlayerStats.Instance.AddKapreCigars(-5);
            Skill1Text.text = "Equipped";
            PlayerStats.Instance.dashSkillEquipped = true;
            PlayerStats.Instance.rangedSkillEquipped = false;
            PlayerStats.Instance.noSkillEquipped = false;
            if (Skill2Purchased)
            {
                Skill2Text.text = "Equip";
            }
        }
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
        if (Skill2Purchased)
        {
            Skill2Text.text = "Equipped";
            PlayerStats.Instance.dashSkillEquipped = false;
            PlayerStats.Instance.rangedSkillEquipped = true;
            PlayerStats.Instance.noSkillEquipped = false;
            if (Skill1Purchased)
            {
                Skill1Text.text = "Equip";
            }
        }
        else if (PlayerStats.Instance.kapreCigars > 5 && !Skill2Purchased)
        {
            Skill2Purchased = true;
            PlayerStats.Instance.AddKapreCigars(-5);
            Skill2Text.text = "Equipped";
            PlayerStats.Instance.dashSkillEquipped = false;
            PlayerStats.Instance.rangedSkillEquipped = true;
            PlayerStats.Instance.noSkillEquipped = false;
            if (Skill1Purchased)
            {
                Skill1Text.text = "Equip";
            }
        }
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
        if (Ult1Purchased)
        {
            Ult1Text.text = "Equipped";
            PlayerStats.Instance.rangedUltEquipped = true;
            PlayerStats.Instance.berserkUltEquipped = false;
            PlayerStats.Instance.noSkillEquipped = false;
            if (Ult2Purchased)
            {
                Ult2Text.text = "Equip";
            }
        }
        else if (PlayerStats.Instance.kapreCigars > 5 && !Ult1Purchased)
        {
            Ult1Purchased = true;
            PlayerStats.Instance.AddKapreCigars(-5);
            Ult1Text.text = "Equipped";
            PlayerStats.Instance.rangedUltEquipped = true;
            PlayerStats.Instance.berserkUltEquipped = false;
            PlayerStats.Instance.noSkillEquipped = false;
            if (Ult2Purchased)
            {
                Ult2Text.text = "Equip";
            }
        }
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
        if (Ult2Purchased)
        {
            Ult2Text.text = "Equipped";
            PlayerStats.Instance.rangedUltEquipped = false;
            PlayerStats.Instance.berserkUltEquipped = true;
            PlayerStats.Instance.noSkillEquipped = false;
            if (Ult1Purchased)
            {
                Ult1Text.text = "Equip";
            }
        }
        else if (PlayerStats.Instance.kapreCigars > 5 && !Ult2Purchased)
        {
            Ult2Purchased = true;
            PlayerStats.Instance.AddKapreCigars(-5);
            Ult2Text.text = "Equipped";
            PlayerStats.Instance.rangedUltEquipped = false;
            PlayerStats.Instance.berserkUltEquipped = true;
            PlayerStats.Instance.noSkillEquipped = false;
            if (Ult1Purchased)
            {
                Ult1Text.text = "Equip";
            }
        }
    }
}
