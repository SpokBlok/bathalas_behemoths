using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MandayanganSkillsUIPanel : MonoBehaviour
{
    public MandayanganSkillsUIPanel[] uiList;
    public MandayanganSkillsUIPanel baseUI;

    //Purchased bools
    private bool Skill1Purchased;
    private bool Skill2Purchased;
    private bool Skill3Purchased;
    private bool Skill4Purchased;
    private bool Skill5Purchased;

    //Texts for purchase buttons
    private TextMeshProUGUI Skill1TextQ;
    private TextMeshProUGUI Skill1TextE;
    private TextMeshProUGUI Skill2Text;
    private TextMeshProUGUI Skill3Text;
    private TextMeshProUGUI Skill4Text;
    private TextMeshProUGUI Skill5Text;

    private List<TextMeshProUGUI> textListQ = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> textListE = new List<TextMeshProUGUI>();

    private PlayerStats playerStats;
    private PlayerSkills playerSkills;

    // Start is called before the first frame update
    void Start()
    {
        Skill1Purchased = false;
        Skill2Purchased = false;
        Skill3Purchased = false;
        Skill4Purchased = false;
        Skill5Purchased = false;

        Skill1TextQ = transform.Find("RightPanel/Skill 1/Skill 1 Equip 1")
            .GetComponentInChildren<TextMeshProUGUI>();
        Skill1TextE = transform.Find("RightPanel/Skill 1/Skill 1 Equip 2")
            .GetComponentInChildren<TextMeshProUGUI>();
        Skill2Text = transform.Find("RightPanel/Skill 2/Skill 2 Purchase Button")
            .GetComponentInChildren<TextMeshProUGUI>();
        //Skill3Text = transform.Find("RightPanel/Skill 3/Skill 3 Purchase Button")
        //    .GetComponentInChildren<TextMeshProUGUI>();
        //Skill4Text = transform.Find("RightPanel/Skill 4/Skill 4 Purchase Button")
        //    .GetComponentInChildren<TextMeshProUGUI>();
        //Skill5Text = transform.Find("RightPanel/Skill 5/Skill 5 Purchase Button")
        //    .GetComponentInChildren<TextMeshProUGUI>();

        textListQ.Add(Skill1TextQ);
        textListE.Add(Skill1TextE);
        //textList.Add(Skill3Text);
        //textList.Add(Skill4Text);
        //textList.Add(Skill5Text);

        playerStats = PlayerStats.Instance;
        playerSkills = PlayerSkills.Instance;
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

    private void EnableAllButtons(Transform parentObject)
    {
        foreach (Transform child in parentObject)
        {
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                button.gameObject.SetActive(true);
            }
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

    public void UnequipAllSkillQ()
    {
        foreach (TextMeshProUGUI text in textListQ)
        {
            text.text = "Equip";
        }
    }

    public void UnequipAllSkillE()
    {
        foreach (TextMeshProUGUI text in textListE)
        {
            text.text = "Equip";
        }
    }

    public void Skill1Purchase()
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

        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        EnableAllButtons(selectedButton.GetComponent<Transform>().parent);
        selectedButton.SetActive(false);
    }

    public void Skill1Equip()
    {
        if (EventSystem.current.currentSelectedGameObject.CompareTag("Q Button"))
        {
            Dash dash = playerSkills.GetComponentInChildren<Dash>();
            playerSkills.BehemothSkillQChange(dash);
            UnequipAllSkillQ();
            Skill1TextQ.text = "Equipped";
            if (Skill1TextE.text == "Equipped")
            {
                Skill1TextE.text = "Equip";
                playerSkills.RemoveBehemothSkillE();
            }
        } 
        else
        {
            Dash dash = playerSkills.GetComponentInChildren<Dash>();
            playerSkills.BehemothSkillEChange(dash);
            UnequipAllSkillE();
            Skill1TextE.text = "Equipped";
            if (Skill1TextQ.text == "Equipped")
            {
                Skill1TextQ.text = "Equip";
                playerSkills.RemoveBehemothSkillQ();
            }
        }
    }
}
