using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MCSkillsUIPanel : MonoBehaviour
{
    public GameObject HPRegenPrefab;

    public GameObject purchaseFlute;
    public GameObject equip1Flute;
    public GameObject equip2Flute;

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
    private PlayerSkills playerSkills;

    public GameObject hud;

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
        playerSkills = PlayerSkills.Instance;
    }

    public void EnablePanel()
    {
        hud = GameObject.FindGameObjectWithTag("HUD");
        gameObject.SetActive(true);
        hud.SetActive(false);
        EventManager.Instance.InvokeOnEnteringUpgradeScreen();

        if(PlayerStats.Instance.clue1)
        {
            purchaseFlute.SetActive(false);
            equip1Flute.SetActive(true);
            equip2Flute.SetActive(true);
        }
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
        hud.SetActive(true);
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
        FirstAid firstAid = playerSkills.GetComponentInChildren<FirstAid>();
        playerSkills.MainCharacterSkillChange(firstAid);
    }

    public void Skill2Upgrade()
    {
        if (!Skill2Purchased)
        {
            if (playerStats.kapreCigars < 5)
            {
                //message that not enough cigars
                return;
            }
            playerStats.AddKapreCigars(-5);
            Skill2Purchased = true;
            Skill2Text.text = "Purchased";
            playerStats.maxHealth *= 1.25f;
            playerStats.currentHealth = playerStats.maxHealth;
            EventManager.Instance.InvokeOnFullHealth();
        }
    }

    public void Skill3Upgrade()
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
        }

        UnequipAll();
        Skill3Text.text = "Equipped";
        MusicalFlute musicalFlute = playerSkills.GetComponentInChildren<MusicalFlute>();
        playerSkills.MainCharacterSkillChange(musicalFlute);
    }

    public void Skill4Upgrade()
    {
        if (!Skill4Purchased)
        {
            if (playerStats.kapreCigars < 10)
            {
                //message that not enough cigars
                return;
            }
            playerStats.AddKapreCigars(-10);
            Skill4Purchased = true;
            Skill4Text.text = "Purchased";
            GameObject instance = Instantiate(HPRegenPrefab, PlayerSkills.Instance.transform);
            instance.name = HPRegenPrefab.name;
        }
    }

    public void Skill5Upgrade()
    {
        if (!Skill5Purchased)
        {
            if (playerStats.kapreCigars < 10)
            {
                //message that not enough cigars
                return;
            }
            playerStats.AddKapreCigars(-10);
            Skill5Purchased = true;
        }

        UnequipAll();
        Skill5Text.text = "Equipped";
        BathalasBlessing bathalasBlessing = playerSkills.GetComponentInChildren<BathalasBlessing>();
        playerSkills.MainCharacterSkillChange(bathalasBlessing);
    }
}
