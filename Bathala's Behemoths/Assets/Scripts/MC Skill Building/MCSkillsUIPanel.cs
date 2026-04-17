using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MCSkillsUIPanel : MonoBehaviour
{
    public GameObject HPRegenPrefab;

    public TextMeshProUGUI purchaseFlute;

    //Purchased bools
    public bool FirstAidPurchased;
    public bool HpUpPurchased;
    public bool FlutePurchased;
    public bool RegenPurchased;
    public bool BBPurchased;

    [SerializeField]
    private Transform rightPanel;

    //Text for purchase buttons
    [SerializeField]
    private TextMeshProUGUI Skill1Text;
    [SerializeField]
    private TextMeshProUGUI Skill2Text;
    [SerializeField]
    private TextMeshProUGUI Skill3Text;
    [SerializeField]
    private TextMeshProUGUI Skill4Text;
    [SerializeField]
    private TextMeshProUGUI Skill5Text;

    private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();

    private PlayerStats playerStats;
    private PlayerSkills playerSkills;

    public GameObject hud;
    public Vector3 originalHUDPos;
    public bool obtainedOGPos;

    private bool initialized;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void EnablePanel()
    {
        Initialize();

        gameObject.SetActive(true);
        QuestState.Instance.pausedForDialogue = true;
        HUDHider.Hide();
        EventManager.Instance.InvokeOnEnteringUpgradeScreen();

        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        if(PlayerStats.Instance.clue6)
        {
            purchaseFlute.text = "Equip";
        }

        // Auto-update UI for already purchased skills based on flags
        if (FirstAidPurchased && Skill1Text != null && Skill1Text.text == "Purchase")
        {
            // Skill 1 is purchased; make it available to equip
            Skill1Text.text = "Equip";
        }

        if (HpUpPurchased && Skill2Text != null)
        {
            // Skill 2 is a passive upgrade; ensure it shows as purchased
            Skill2Text.text = "Purchased";
        }

        if (FlutePurchased && Skill3Text != null && Skill3Text.text == "Purchase")
        {
            // Skill 3 is purchased; make it available to equip
            Skill3Text.text = "Equip";
        }

        if (RegenPurchased && Skill4Text != null)
        {
            // Skill 4 is a passive upgrade; ensure it shows as purchased
            Skill4Text.text = "Purchased";
        }

        if (BBPurchased && Skill5Text != null && Skill5Text.text == "Purchase")
        {
            // Skill 5 is purchased; make it available to equip
            Skill5Text.text = "Equip";
        }
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
        QuestState.Instance.pausedForDialogue = false;

        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        HUDHider.Show();
        EventManager.Instance.InvokeOnExitingUpgradeScreen();
        Transform panel = rightPanel != null ? rightPanel : transform.Find("RightPanel");
        if (panel == null)
        {
            Debug.LogError("MCSkillsUIPanel: RightPanel not found when disabling panel.");
            return;
        }
        foreach (Transform child in panel)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void DisplaySkill()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        Transform panel = rightPanel != null ? rightPanel : transform.Find("RightPanel");
        if (panel == null)
        {
            Debug.LogError("MCSkillsUIPanel: RightPanel not found when displaying skill.");
            return;
        }
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
            if (text != null && text.text == "Equipped")
            {
                text.text = "Equip";
            }
        }
    }

    private void Initialize()
    {
        if (initialized)
        {
            return;
        }

        FirstAidPurchased = PlayerStats.Instance.FirstAidPurchased;
        HpUpPurchased = PlayerStats.Instance.HpUpPurchased;
        FlutePurchased = PlayerStats.Instance.FlutePurchased;
        RegenPurchased = PlayerStats.Instance.RegenPurchased;
        BBPurchased = PlayerStats.Instance.BBPurchased;

        if (rightPanel == null)
        {
            rightPanel = transform.Find("RightPanel");
            if (rightPanel == null)
            {
                Debug.LogError("MCSkillsUIPanel: RightPanel not found. Please assign it in the inspector.");
            }
        }

        Skill1Text = ResolveTextIfNull(Skill1Text, "Skill 1/Skill 1 Purchase Button", nameof(Skill1Text));
        Skill2Text = ResolveTextIfNull(Skill2Text, "Skill 2/Skill 2 Purchase Button", nameof(Skill2Text));
        Skill3Text = ResolveTextIfNull(Skill3Text, "Skill 3/Skill 3 Purchase Button", nameof(Skill3Text));
        Skill4Text = ResolveTextIfNull(Skill4Text, "Skill 4/Skill 4 Purchase Button", nameof(Skill4Text));
        Skill5Text = ResolveTextIfNull(Skill5Text, "Skill 5/Skill 5 Purchase Button", nameof(Skill5Text));

        if (textList.Count == 0)
        {
            if (Skill1Text != null) textList.Add(Skill1Text);
            if (Skill2Text != null) textList.Add(Skill2Text);
            if (Skill3Text != null) textList.Add(Skill3Text);
            if (Skill4Text != null) textList.Add(Skill4Text);
            if (Skill5Text != null) textList.Add(Skill5Text);
        }

        playerStats = PlayerStats.Instance;
        playerSkills = PlayerSkills.Instance;

        hud = GameObject.FindGameObjectWithTag("HUD");

        initialized = true;
    }

    private TextMeshProUGUI ResolveTextIfNull(TextMeshProUGUI field, string relativePathFromRightPanel, string fieldName)
    {
        if (field != null)
        {
            return field;
        }

        Transform root = rightPanel != null ? rightPanel : transform;
        string fullPath = rightPanel != null ? relativePathFromRightPanel : "RightPanel/" + relativePathFromRightPanel;

        Transform target = root.Find(fullPath);
        if (target == null)
        {
            Debug.LogError($"MCSkillsUIPanel: Could not find TextMeshProUGUI for {fieldName} at path '{fullPath}'. Please assign it in the inspector.");
            return field;
        }

        TextMeshProUGUI result = target.GetComponentInChildren<TextMeshProUGUI>();
        if (result == null)
        {
            Debug.LogError($"MCSkillsUIPanel: No TextMeshProUGUI component found in children for {fieldName} at path '{fullPath}'.");
        }

        return result;
    }

    public void Skill1Upgrade()
    {
        if (!FirstAidPurchased)
        {
            if (playerStats.kapreCigars < 5)
            {
                //message that not enough cigars
                return;
            }
            playerStats.AddKapreCigars(-5);
            FirstAidPurchased = true;
            PlayerStats.Instance.FirstAidPurchased = true;
        }

        UnequipAll();
        Skill1Text.text = "Equipped";
        FirstAid firstAid = playerSkills.GetComponentInChildren<FirstAid>();
        playerSkills.MainCharacterSkillChange(firstAid);
    }

    public void Skill2Upgrade()
    {
        if (!HpUpPurchased)
        {
            if (playerStats.kapreCigars < 5)
            {
                //message that not enough cigars
                return;
            }
            playerStats.AddKapreCigars(-5);
            HpUpPurchased = true;
            PlayerStats.Instance.HpUpPurchased = true;
            Skill2Text.text = "Purchased";
            playerStats.maxHealth *= 1.25f;
            playerStats.currentHealth = playerStats.maxHealth;
            EventManager.Instance.InvokeOnFullHealth();
        }
    }

    public void Skill3Upgrade()
    {
        if (!FlutePurchased)
        {
            if (playerStats.kapreCigars < 5)
            {
                //message that not enough cigars
                return;
            }
            playerStats.AddKapreCigars(-5);
            FlutePurchased = true;
            PlayerStats.Instance.FlutePurchased = true;
        }

        UnequipAll();
        Skill3Text.text = "Equipped";
        MusicalFlute musicalFlute = playerSkills.GetComponentInChildren<MusicalFlute>();
        playerSkills.MainCharacterSkillChange(musicalFlute);
    }

    public void Skill4Upgrade()
    {
        if (!RegenPurchased)
        {
            if (playerStats.kapreCigars < 10)
            {
                //message that not enough cigars
                return;
            }
            playerStats.AddKapreCigars(-10);
            RegenPurchased = true;
            PlayerStats.Instance.RegenPurchased = true;
            Skill4Text.text = "Purchased";
            GameObject instance = Instantiate(HPRegenPrefab, PlayerSkills.Instance.transform);
            instance.name = HPRegenPrefab.name;
        }
    }

    public void Skill5Upgrade()
    {
        if (!BBPurchased)
        {
            if (playerStats.kapreCigars < 10)
            {
                //message that not enough cigars
                return;
            }
            playerStats.AddKapreCigars(-10);
            BBPurchased = true;
            PlayerStats.Instance.BBPurchased = true;
        }

        UnequipAll();
        Skill5Text.text = "Equipped";
        BathalasBlessing bathalasBlessing = playerSkills.GetComponentInChildren<BathalasBlessing>();
        playerSkills.MainCharacterSkillChange(bathalasBlessing);
    }
}
