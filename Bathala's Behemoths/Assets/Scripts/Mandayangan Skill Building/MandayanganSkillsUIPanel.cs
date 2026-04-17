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

    public GameObject purchaseMudfling;
    public GameObject equip1Mudfling;
    public GameObject equip2Mudfling;


    //Purchased bools
    private bool DashPurchased;
    private bool MudflingPurchased;
    private bool AtkUpPurchased;
    private bool TornadoPurchased;
    private bool MudArmorPurchased;

    public Transform skill1;
    public Transform skill2;
    public Transform skill4;
    public Transform skill5;
    public Transform purchaseButton;

    [SerializeField]
    private Transform rightPanel;

    //Texts for purchase buttons
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

    private List<TextMeshProUGUI> textListQ = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> textListE = new List<TextMeshProUGUI>();

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

        if (PlayerStats.Instance.clue1)
        {
            purchaseMudfling.SetActive(false);
            equip1Mudfling.SetActive(true);
            equip2Mudfling.SetActive(true);
        }

        // Auto-unlock any skills that are already purchased
        if (DashPurchased)
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
                Debug.LogError("MandayanganSkillsUIPanel: Skill 1 container not found. Please assign 'skill1' in the inspector.");
            }
        }

        if (MudflingPurchased)
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
                Debug.LogError("MandayanganSkillsUIPanel: Skill 2 container not found. Please assign 'skill2' in the inspector.");
            }
        }

        if (AtkUpPurchased)
        {
            Transform skill3Purchase = rightPanel != null ?
                rightPanel.Find("Skill 3/Skill 3 Purchase Button") :
                transform.Find("RightPanel/Skill 3/Skill 3 Purchase Button");
            if (skill3Purchase != null)
            {
                TextMeshProUGUI text = skill3Purchase.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    text.text = "Purchased";
                }
            }
            else
            {
                Debug.LogError("MandayanganSkillsUIPanel: Skill 3 purchase button not found.");
            }
        }

        if (TornadoPurchased)
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
                Debug.LogError("MandayanganSkillsUIPanel: Skill 4 container not found. Please assign 'skill4' in the inspector.");
            }
        }

        if (MudArmorPurchased)
        {
            if (skill5 == null)
            {
                skill5 = rightPanel != null ? rightPanel.Find("Skill 5") : transform.Find("RightPanel/Skill 5");
            }
            if (skill5 != null)
            {
                EnableAllButtons(skill5);
                purchaseButton = skill5.Find("Skill 5 Purchase Button");
                if (purchaseButton != null)
                {
                    purchaseButton.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("MandayanganSkillsUIPanel: Skill 5 container not found. Please assign 'skill5' in the inspector.");
            }
        }
    }

    private void Initialize()
    {
        if (initialized)
        {
            return;
        }

        DashPurchased = PlayerStats.Instance.DashPurchased;
        MudflingPurchased = PlayerStats.Instance.MudflingPurchased;
        AtkUpPurchased = PlayerStats.Instance.AtkUpPurchased;
        TornadoPurchased = PlayerStats.Instance.TornadoPurchased;
        MudArmorPurchased = PlayerStats.Instance.MudArmorPurchased;

        if (rightPanel == null)
        {
            rightPanel = transform.Find("RightPanel");
            if (rightPanel == null)
            {
                Debug.LogError("MandayanganSkillsUIPanel: RightPanel not found. Please assign it in the inspector.");
            }
        }

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
            textListQ.Add(Skill1TextQ);
            textListE.Add(Skill1TextE);
            textListQ.Add(Skill2TextQ);
            textListE.Add(Skill2TextE);
            textListQ.Add(Skill4TextQ);
            textListE.Add(Skill4TextE);
            textListQ.Add(Skill5TextQ);
            textListE.Add(Skill5TextE);
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
            Debug.LogError("MandayanganSkillsUIPanel: RightPanel not found when disabling panel.");
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
            Debug.LogError("MandayanganSkillsUIPanel: RightPanel not found when displaying skill.");
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
        Transform parent = selectedButton.GetComponent<Transform>().parent;
        EnableAllButtons(parent);
        selectedButton.SetActive(false);

        // Track which skill was purchased based on the parent container's name
        switch (parent.name)
        {
            case "Skill 1":
                DashPurchased = true;
                PlayerStats.Instance.DashPurchased = true;
                break;
            case "Skill 2":
                MudflingPurchased = true;
                PlayerStats.Instance.MudflingPurchased = true;
                break;
            case "Skill 4":
                TornadoPurchased = true;
                PlayerStats.Instance.TornadoPurchased = true;
                break;
            case "Skill 5":
                MudArmorPurchased = true;
                PlayerStats.Instance.MudArmorPurchased = true;
                break;
        }
    }

    public void Skill3Purchase()
    {
        if (!AtkUpPurchased)
        {
            if (playerStats.kapreCigars < 5)
            {
                //message that not enough cigars
                return;
            }
            playerStats.AddKapreCigars(-5);
            AtkUpPurchased = true;
            TextMeshProUGUI text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Purchased";
            playerStats.basicAttackDamage *= 1.25f;
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
            Debug.LogError($"MandayanganSkillsUIPanel: Could not find TextMeshProUGUI for {fieldName} at path '{fullPath}'. Please assign it in the inspector.");
            return field;
        }

        TextMeshProUGUI result = target.GetComponentInChildren<TextMeshProUGUI>();
        if (result == null)
        {
            Debug.LogError($"MandayanganSkillsUIPanel: No TextMeshProUGUI component found in children for {fieldName} at path '{fullPath}'.");
        }

        return result;
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
