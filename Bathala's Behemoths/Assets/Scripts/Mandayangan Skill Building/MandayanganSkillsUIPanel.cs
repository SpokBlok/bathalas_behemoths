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
    private bool Skill3Purchased;

    //Texts for purchase buttons
    private TextMeshProUGUI Skill1TextQ;
    private TextMeshProUGUI Skill1TextE;
    private TextMeshProUGUI Skill2TextQ;
    private TextMeshProUGUI Skill2TextE;
    private TextMeshProUGUI Skill4TextQ;
    private TextMeshProUGUI Skill4TextE;
    private TextMeshProUGUI Skill5TextQ;
    private TextMeshProUGUI Skill5TextE;

    private List<TextMeshProUGUI> textListQ = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> textListE = new List<TextMeshProUGUI>();

    private PlayerStats playerStats;
    private PlayerSkills playerSkills;

    // Start is called before the first frame update
    void Start()
    {
        Skill3Purchased = false;

        Skill1TextQ = transform.Find("RightPanel/Skill 1/Skill 1 Equip 1")
            .GetComponentInChildren<TextMeshProUGUI>();
        Skill1TextE = transform.Find("RightPanel/Skill 1/Skill 1 Equip 2")
            .GetComponentInChildren<TextMeshProUGUI>();

        Skill2TextQ = transform.Find("RightPanel/Skill 2/Skill 2 Equip 1")
            .GetComponentInChildren<TextMeshProUGUI>();
        Skill2TextE = transform.Find("RightPanel/Skill 2/Skill 2 Equip 2")
            .GetComponentInChildren<TextMeshProUGUI>();

        Skill4TextQ = transform.Find("RightPanel/Skill 4/Skill 4 Equip 1")
            .GetComponentInChildren<TextMeshProUGUI>();
        Skill4TextE = transform.Find("RightPanel/Skill 4/Skill 4 Equip 2")
            .GetComponentInChildren<TextMeshProUGUI>();

        Skill5TextQ = transform.Find("RightPanel/Skill 5/Skill 5 Equip 1")
            .GetComponentInChildren<TextMeshProUGUI>();
        Skill5TextE = transform.Find("RightPanel/Skill 5/Skill 5 Equip 2")
            .GetComponentInChildren<TextMeshProUGUI>();

        textListQ.Add(Skill1TextQ);
        textListE.Add(Skill1TextE);
        textListQ.Add(Skill2TextQ);
        textListE.Add(Skill2TextE);
        textListQ.Add(Skill4TextQ);
        textListE.Add(Skill4TextE);
        textListQ.Add(Skill5TextQ);
        textListE.Add(Skill5TextE);

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

    public void SkillWithEquipPurchase()
    {
        if (playerStats.kapreCigars < 5)
        {
            //message that not enough cigars
            return;
        }
        playerStats.AddKapreCigars(-5);

        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        EnableAllButtons(selectedButton.GetComponent<Transform>().parent);
        selectedButton.SetActive(false);
    }

    public void Skill3Purchase()
    {
        if (!Skill3Purchased)
        {
            if (playerStats.kapreCigars < 5)
            {
                //message that not enough cigars
                return;
            }
            playerStats.AddKapreCigars(-5);
            Skill3Purchased = true;
            TextMeshProUGUI text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Purchased";
            playerStats.basicAttackDamage *= 1.25f;
        }
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

    public void Skill2Equip()
    {
        if (EventSystem.current.currentSelectedGameObject.CompareTag("Q Button"))
        {
            Mudfling mudFling = playerSkills.GetComponentInChildren<Mudfling>();
            playerSkills.BehemothSkillQChange(mudFling);
            UnequipAllSkillQ();
            Skill2TextQ.text = "Equipped";
            if (Skill2TextE.text == "Equipped")
            {
                Skill2TextE.text = "Equip";
                playerSkills.RemoveBehemothSkillE();
            }
        }
        else
        {
            Mudfling mudFling = playerSkills.GetComponentInChildren<Mudfling>();
            playerSkills.BehemothSkillEChange(mudFling);
            UnequipAllSkillE();
            Skill2TextE.text = "Equipped";
            if (Skill2TextQ.text == "Equipped")
            {
                Skill2TextQ.text = "Equip";
                playerSkills.RemoveBehemothSkillQ();
            }
        }
    }

    public void Skill4Equip()
    {
        if (EventSystem.current.currentSelectedGameObject.CompareTag("Q Button"))
        {
            TornadoPunch tornadoPunch = playerSkills.GetComponentInChildren<TornadoPunch>();
            playerSkills.BehemothSkillQChange(tornadoPunch);
            UnequipAllSkillQ();
            Skill4TextQ.text = "Equipped";
            if (Skill4TextE.text == "Equipped")
            {
                Skill4TextE.text = "Equip";
                playerSkills.RemoveBehemothSkillE();
            }
        }
        else
        {
            TornadoPunch tornadoPunch = playerSkills.GetComponentInChildren<TornadoPunch>();
            playerSkills.BehemothSkillEChange(tornadoPunch);
            UnequipAllSkillE();
            Skill4TextE.text = "Equipped";
            if (Skill4TextQ.text == "Equipped")
            {
                Skill4TextQ.text = "Equip";
                playerSkills.RemoveBehemothSkillQ();
            }
        }
    }

    public void Skill5Equip()
    {
        if (EventSystem.current.currentSelectedGameObject.CompareTag("Q Button"))
        {
            MudArmor mudArmor = playerSkills.GetComponentInChildren<MudArmor>();
            playerSkills.BehemothSkillQChange(mudArmor);
            UnequipAllSkillQ();
            Skill5TextQ.text = "Equipped";
            if (Skill5TextE.text == "Equipped")
            {
                Skill5TextE.text = "Equip";
                playerSkills.RemoveBehemothSkillE();
            }
        }
        else
        {
            MudArmor mudArmor = playerSkills.GetComponentInChildren<MudArmor>();
            playerSkills.BehemothSkillEChange(mudArmor);
            UnequipAllSkillE();
            Skill5TextE.text = "Equipped";
            if (Skill5TextQ.text == "Equipped")
            {
                Skill5TextQ.text = "Equip";
                playerSkills.RemoveBehemothSkillQ();
            }
        }
    }
}
