using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MCSkillsUIPanel : MonoBehaviour
{
    //Purchased bools
    private bool Skill1Purchased;
    private bool Skill2Purchased;
    private bool Skill3Purchased;
    private bool Skill4Purchased;
    private bool Skill5Purchased;

    //Text for purchase buttons
    private TextMeshProUGUI Skill1Text;
    private TextMeshProUGUI Skill2Text;
    private TextMeshProUGUI Skill3Text;
    private TextMeshProUGUI Skill4Text;
    private TextMeshProUGUI Skill5Text;

    private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();

    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        Skill1Purchased = false;
        Skill2Purchased = false;
        Skill3Purchased = false;
        Skill4Purchased = false;
        Skill5Purchased = false;

        Skill1Text = transform.Find("RightPanel/Skill 1/Skill 1 Purchase Button")
            .GetComponentInChildren<TextMeshProUGUI>();
        Skill2Text = transform.Find("RightPanel/Skill 2/Skill 2 Purchase Button")
            .GetComponentInChildren<TextMeshProUGUI>();
        Skill3Text = transform.Find("RightPanel/Skill 3/Skill 3 Purchase Button")
            .GetComponentInChildren<TextMeshProUGUI>();
        Skill4Text = transform.Find("RightPanel/Skill 4/Skill 4 Purchase Button")
            .GetComponentInChildren<TextMeshProUGUI>();
        Skill5Text = transform.Find("RightPanel/Skill 5/Skill 5 Purchase Button")
            .GetComponentInChildren<TextMeshProUGUI>();

        textList.Add(Skill1Text);
        textList.Add(Skill2Text);
        textList.Add(Skill3Text);
        textList.Add(Skill4Text);
        textList.Add(Skill5Text);

        playerStats = PlayerStats.Instance;
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

    public void DisplaySkill()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        Transform panel = transform.Find("RightPanel");
        foreach (Transform child in panel)
        {
            if (child.name == clickedObject.name)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void UnequipAll()
    {
        foreach (TextMeshProUGUI text in textList)
        {
            if (text.text == "Equipped")
            {
                text.text = "Equip";
            }
        }
    }
    public void Skill1Upgrade()
    {
        if (!Skill1Purchased)
        {
            if (playerStats.kapreCigars < 5)
            {
                //message that not enough cigars
                return;
            }
            playerStats.AddKapreCigars(-5);
            Skill1Purchased = true;
        }

        UnequipAll();
        Skill1Text.text = "Equipped";
        FirstAid firstAid = PlayerSkills.Instance.GetComponentInChildren<FirstAid>();
        PlayerSkills.Instance.MainCharacterSkillChange(firstAid);
    }
}
