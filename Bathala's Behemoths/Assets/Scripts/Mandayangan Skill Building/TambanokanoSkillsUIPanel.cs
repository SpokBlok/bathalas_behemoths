using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TambanokanoSkillsUIPanel : MonoBehaviour
{
    public TambanokanoSkillsUIPanel[] uiList;
    public TambanokanoSkillsUIPanel baseUI;

    public GameObject purchaseMudfling;
    public GameObject equip1Mudfling;
    public GameObject equip2Mudfling;

    private bool SwipePurchased;
    private bool LightningPurchased;
    private bool AtkUpPurchased;
    private bool RockyShellPurchased;
    private bool ProtectPurchased;

    public Transform skill1;
    public Transform skill2;
    public Transform skill4;
    public Transform skill5;
    public Transform purchaseButton;

    [SerializeField]
    private Transform rightPanel;

    [SerializeField]
    private TextMeshProUGUI Skill1TextQ;
    [SerializeField]
    private TextMeshProUGUI Skill1TextE;
    [SerializeField]
    private TextMeshProUGUI Skill2TextQ;
    [SerializeField]
    private TextMeshProUGUI Skill2TextE;
    [SerializeField]
    private TextMeshProUGUI Skill4TextQ;
    [SerializeField]
    private TextMeshProUGUI Skill4TextE;
    [SerializeField]
    private TextMeshProUGUI Skill5TextQ;
    [SerializeField]
    private TextMeshProUGUI Skill5TextE;

    private readonly List<TextMeshProUGUI> textListQ = new List<TextMeshProUGUI>();
    private readonly List<TextMeshProUGUI> textListE = new List<TextMeshProUGUI>();

    private PlayerStats playerStats;
    private PlayerSkills playerSkills;

    public GameObject hud;
    public Vector3 originalHUDPos;
    public bool obtainedOGPos;

    private bool initialized;

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

        if (PlayerStats.Instance.clue1)
        {
            LightningPurchased = true;
            PlayerStats.Instance.TammyLightningPurchased = true;
            purchaseMudfling.SetActive(false);
            equip1Mudfling.SetActive(true);
            equip2Mudfling.SetActive(true);
        }

        if (SwipePurchased)
        {
            if (skill1 == null)
            {
                skill1 = rightPanel != null ? rightPanel.Find("Skill 1") : transform.Find("RightPanel/Skill 1");
            }
            if (skill1 != null)
            {
                EnableAllButtons(skill1);
                purchaseButton = skill1.Find("Skill 1 Purchase Button");
                if (purchaseButton != null)
                {
                    purchaseButton.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("TambanokanoSkillsUIPanel: Skill 1 container not found. Please assign 'skill1' in the inspector.");
            }
        }

        if (LightningPurchased)
        {
            if (skill2 == null)
            {
                skill2 = rightPanel != null ? rightPanel.Find("Skill 2") : transform.Find("RightPanel/Skill 2");
            }
            if (skill2 != null)
            {
                EnableAllButtons(skill2);
                purchaseButton = skill2.Find("Skill 2 Purchase Button");
                if (purchaseButton != null)
                {
                    purchaseButton.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("TambanokanoSkillsUIPanel: Skill 2 container not found. Please assign 'skill2' in the inspector.");
            }
        }

        if (AtkUpPurchased)
        {
            MarkPassiveSkillPurchased("Skill 3");
        }

        if (ProtectPurchased)
        {
            if (skill4 == null)
            {
                skill4 = rightPanel != null ? rightPanel.Find("Skill 4") : transform.Find("RightPanel/Skill 4");
            }
            if (skill4 != null)
            {
                EnableAllButtons(skill4);
                purchaseButton = skill4.Find("Skill 4 Purchase Button");
                if (purchaseButton != null)
                {
                    purchaseButton.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("TambanokanoSkillsUIPanel: Skill 4 container not found. Please assign 'skill4' in the inspector.");
            }
        }

        if (RockyShellPurchased)
        {
            MarkPassiveSkillPurchased("Skill 5");
        }
    }

    private void Initialize()
    {
        if (initialized)
        {
            return;
        }

        SwipePurchased = PlayerStats.Instance.TammySwipePurchased;
        LightningPurchased = PlayerStats.Instance.TammyLightningPurchased;
        AtkUpPurchased = PlayerStats.Instance.AtkUpPurchased;
        RockyShellPurchased = PlayerStats.Instance.RockyShellPurchased;
        ProtectPurchased = PlayerStats.Instance.TammyProtectPurchased;

        if (rightPanel == null)
        {
            rightPanel = transform.Find("RightPanel");
            if (rightPanel == null)
            {
                Debug.LogError("TambanokanoSkillsUIPanel: RightPanel not found. Please assign it in the inspector.");
            }
        }

        ResolveSkillContainers();
        ConfigurePassiveSkillPresentation(
            "Skill 3",
            "Attack Up",
            "Increase Tambanokano's base attack damage by 25%.\n\n\nCost: 5 Kapre Cigars");
        ConfigurePassiveSkillPresentation(
            "Skill 5",
            "Rocky Shell",
            "Harden yourself with a rocky shell to permanently reduce damage taken by 10%.\n\n\nCost: 5 Kapre Cigars");
        ConfigureRockyShellVisuals();

        Skill1TextQ = ResolveTextIfNull(Skill1TextQ, "Skill 1/Skill 1 Equip 1", nameof(Skill1TextQ));
        Skill1TextE = ResolveTextIfNull(Skill1TextE, "Skill 1/Skill 1 Equip 2", nameof(Skill1TextE));

        Skill2TextQ = ResolveTextIfNull(Skill2TextQ, "Skill 2/Skill 2 Equip 1", nameof(Skill2TextQ));
        Skill2TextE = ResolveTextIfNull(Skill2TextE, "Skill 2/Skill 2 Equip 2", nameof(Skill2TextE));

        Skill4TextQ = ResolveTextIfNull(Skill4TextQ, "Skill 4/Skill 4 Equip 1", nameof(Skill4TextQ));
        Skill4TextE = ResolveTextIfNull(Skill4TextE, "Skill 4/Skill 4 Equip 2", nameof(Skill4TextE));

        Skill5TextQ = ResolveTextIfNull(Skill5TextQ, "Skill 5/Skill 5 Equip 1", nameof(Skill5TextQ));
        Skill5TextE = ResolveTextIfNull(Skill5TextE, "Skill 5/Skill 5 Equip 2", nameof(Skill5TextE));

        if (textListQ.Count == 0)
        {
            AddTextIfFound(textListQ, Skill1TextQ);
            AddTextIfFound(textListE, Skill1TextE);
            AddTextIfFound(textListQ, Skill2TextQ);
            AddTextIfFound(textListE, Skill2TextE);
            AddTextIfFound(textListQ, Skill4TextQ);
            AddTextIfFound(textListE, Skill4TextE);
            AddTextIfFound(textListQ, Skill5TextQ);
            AddTextIfFound(textListE, Skill5TextE);
        }

        playerStats = PlayerStats.Instance;
        playerSkills = PlayerSkills.Instance;

        hud = GameObject.FindGameObjectWithTag("HUD");

        initialized = true;
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
            Debug.LogError("TambanokanoSkillsUIPanel: RightPanel not found when disabling panel.");
            return;
        }
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
        Transform panel = rightPanel != null ? rightPanel : transform.Find("RightPanel");
        if (panel == null)
        {
            Debug.LogError("TambanokanoSkillsUIPanel: RightPanel not found when displaying skill.");
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

    public void UnequipAllSkillQ()
    {
        foreach (TextMeshProUGUI text in textListQ)
        {
            if (text != null)
            {
                text.text = "Equip";
            }
        }
    }

    public void UnequipAllSkillE()
    {
        foreach (TextMeshProUGUI text in textListE)
        {
            if (text != null)
            {
                text.text = "Equip";
            }
        }
    }

    public void SkillWithEquipPurchase()
    {
        if (playerStats.kapreCigars < 5)
        {
            return;
        }

        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        Transform parent = selectedButton.GetComponent<Transform>().parent;

        switch (parent.name)
        {
            case "Skill 1":
                SwipePurchased = true;
                PlayerStats.Instance.TammySwipePurchased = true;
                break;
            case "Skill 2":
                LightningPurchased = true;
                PlayerStats.Instance.TammyLightningPurchased = true;
                break;
            case "Skill 4":
                ProtectPurchased = true;
                PlayerStats.Instance.TammyProtectPurchased = true;
                break;
            case "Skill 5":
                RockyShellPurchased = true;
                PlayerStats.Instance.RockyShellPurchased = true;
                break;
            default:
                return;
        }

        playerStats.AddKapreCigars(-5);

        if (parent.name == "Skill 5")
        {
            MarkPassiveSkillPurchased("Skill 5");
            return;
        }

        EnableAllButtons(parent);
        selectedButton.SetActive(false);
    }

    public void Skill3Purchase()
    {
        if (!AtkUpPurchased)
        {
            if (playerStats.kapreCigars < 5)
            {
                return;
            }

            playerStats.AddKapreCigars(-5);
            AtkUpPurchased = true;
            PlayerStats.Instance.AtkUpPurchased = true;
            TextMeshProUGUI text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Purchased";
            playerStats.basicAttackDamage *= 1.25f;
        }
    }

    private void MarkPassiveSkillPurchased(string skillName)
    {
        Transform skillRoot = rightPanel != null ? rightPanel.Find(skillName) : transform.Find($"RightPanel/{skillName}");
        if (skillRoot == null)
        {
            Debug.LogError($"TambanokanoSkillsUIPanel: {skillName} container not found.");
            return;
        }

        Transform passivePurchaseButton = skillRoot.Find($"{skillName} Purchase Button");
        if (passivePurchaseButton == null)
        {
            Debug.LogError($"TambanokanoSkillsUIPanel: {skillName} purchase button not found.");
            return;
        }

        Button button = passivePurchaseButton.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false;
        }

        TextMeshProUGUI text = passivePurchaseButton.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = "Purchased";
        }

        foreach (Transform child in skillRoot)
        {
            if (child == passivePurchaseButton)
            {
                continue;
            }

            if (child.GetComponent<Button>() != null)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void ResolveSkillContainers()
    {
        if (rightPanel == null)
        {
            return;
        }

        skill1 = rightPanel.Find("Skill 1");
        skill2 = rightPanel.Find("Skill 2");
        skill4 = rightPanel.Find("Skill 4");
        skill5 = rightPanel.Find("Skill 5");
    }

    private void ConfigurePassiveSkillPresentation(string skillName, string displayName, string description)
    {
        Transform skillRoot = rightPanel != null ? rightPanel.Find(skillName) : transform.Find($"RightPanel/{skillName}");
        if (skillRoot == null)
        {
            return;
        }

        SetTextOnChild(skillRoot, $"{skillName} Name", displayName);
        SetTextOnChild(skillRoot, $"{skillName} Info", description);
        HideButton(skillRoot.Find($"{skillName} Equip 1"));
        HideButton(skillRoot.Find($"{skillName} Equip 2"));
    }

    private void ConfigureRockyShellVisuals()
    {
        Transform skillRoot = rightPanel != null ? rightPanel.Find("Skill 5") : transform.Find("RightPanel/Skill 5");
        if (skillRoot == null)
        {
            return;
        }

        Transform rockyShellGraphic = FindDescendantByName(skillRoot, "RockyShell");
        if (rockyShellGraphic != null)
        {
            rockyShellGraphic.gameObject.SetActive(true);
        }

        Transform protectGraphic = FindDescendantByName(skillRoot, "Protect");
        if (protectGraphic != null)
        {
            protectGraphic.gameObject.SetActive(false);
        }
    }

    private void SetTextOnChild(Transform parent, string childName, string value)
    {
        Transform child = parent.Find(childName);
        if (child == null)
        {
            return;
        }

        TextMeshProUGUI text = child.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = value;
        }
    }

    private void HideButton(Transform buttonTransform)
    {
        if (buttonTransform != null)
        {
            buttonTransform.gameObject.SetActive(false);
        }
    }

    private Transform FindDescendantByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }

            Transform nested = FindDescendantByName(child, name);
            if (nested != null)
            {
                return nested;
            }
        }

        return null;
    }

    private void AddTextIfFound(List<TextMeshProUGUI> textList, TextMeshProUGUI text)
    {
        if (text != null)
        {
            textList.Add(text);
        }
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
            Debug.LogError($"TambanokanoSkillsUIPanel: Could not find TextMeshProUGUI for {fieldName} at path '{fullPath}'. Please assign it in the inspector.");
            return field;
        }

        TextMeshProUGUI result = target.GetComponentInChildren<TextMeshProUGUI>();
        if (result == null)
        {
            Debug.LogError($"TambanokanoSkillsUIPanel: No TextMeshProUGUI component found in children for {fieldName} at path '{fullPath}'.");
        }

        return result;
    }

    private T GetBehemothSkill<T>(string skillName) where T : BaseSkill
    {
        T skill = playerSkills.GetComponentInChildren<T>();
        if (skill == null)
        {
            Debug.LogError($"TambanokanoSkillsUIPanel: Could not find {skillName} on PlayerSkills.");
        }

        return skill;
    }

    public void Skill1Equip()
    {
        Swipe swipe = GetBehemothSkill<Swipe>(nameof(Swipe));
        if (swipe == null)
        {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject.CompareTag("Q Button"))
        {
            playerSkills.BehemothSkillQChange(swipe);
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
            playerSkills.BehemothSkillEChange(swipe);
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
        Lightning lightning = GetBehemothSkill<Lightning>(nameof(Lightning));
        if (lightning == null)
        {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject.CompareTag("Q Button"))
        {
            playerSkills.BehemothSkillQChange(lightning);
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
            playerSkills.BehemothSkillEChange(lightning);
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
        Protect protect = GetBehemothSkill<Protect>(nameof(Protect));
        if (protect == null)
        {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject.CompareTag("Q Button"))
        {
            playerSkills.BehemothSkillQChange(protect);
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
            playerSkills.BehemothSkillEChange(protect);
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
        Debug.Log("TambanokanoSkillsUIPanel: Rocky Shell is passive and cannot be equipped.");
    }
}
