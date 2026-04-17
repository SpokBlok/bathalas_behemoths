using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int initSpeed;
    public float speed;
    public float speedMultiplier;
    public float basicAttackDamage;
    public float currentHealth;
    public float maxHealth;

    public bool dead;
    public bool hasMudArmor;
    public bool hasProtect;

    public bool introDone;
    public bool tutorialDone;
    public bool ruinsScene;
    public bool outdoorsScene;
    public bool questComp;

    public float kapreCigars;
    public bool clue1;
    public bool clue2;
    public bool clue3;
    public bool clue4;
    public bool clue5;
    public bool clue6;

    public bool activeQuest1;
    public bool activeQuest2;
    public bool activeQuest3;
    public bool activeQuest4;
    public bool activeQuest5;
    public bool activeQuest6;

    public bool tammyFound;
    public bool markyFound;
    public bool apolakiFound;
    public bool tammyScene;
    public bool markyScene;
    public bool apolakiScene;
    public bool apolakiUnlocked;
    public bool ruinsVisitedOnce;

    public AudioClip skillESound;
    public AudioClip skillQSound;
    public AudioClip mcSkillSound;

    // Skills Purchased Booleans
    public bool DashPurchased;
    public bool MudflingPurchased;
    public bool AtkUpPurchased;
    public bool TornadoPurchased;
    public bool MudArmorPurchased;
    public bool FirstAidPurchased;
    public bool HpUpPurchased;
    public bool FlutePurchased;
    public bool RegenPurchased;
    public bool BBPurchased;
    public bool RockyShellPurchased;

    public GameObject player;
    public Vector3 playerSavePosition;

    public int playerModelIndex = 1; // 1 for Manny, 2 for Tammy, 3 for Marky

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
            InitializePlayerStats();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    private void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void InitializePlayerStats()
    {
        outdoorsScene = true; // Set to true for correct build - false for testing Manny+Steve Model
        ruinsScene = false; // set to false by default
        introDone = false; // set to false by default
        tutorialDone = true; // set to false by default
        ruinsVisitedOnce = false; // set to false by default
        dead = false; // duh

        // Set default values
        if(ruinsScene && introDone)
        {
            initSpeed = 0;
        }
        else if(outdoorsScene && introDone == false)
        {
            initSpeed = 0;
        }
        else
        {
            initSpeed = 5;
        }

        if(tammyScene || markyScene || apolakiScene) 
        {
            speedMultiplier = 1.5f;
        }
        else
        {
            speedMultiplier = 1.0f;
        }

        activeQuest1 = false;
        activeQuest2 = false;
        activeQuest3 = false;
        activeQuest4 = false;
        activeQuest5 = false;
        activeQuest6 = false;

        basicAttackDamage = 25;
        maxHealth = 50;
        currentHealth = maxHealth;

        hasMudArmor = false;
        hasProtect = false;

        kapreCigars = 0;
}

    public void AddBasicAttackDamage(int damageIncrease)
    {
        basicAttackDamage += damageIncrease;
    }

    public void AddSpeed(int speedIncrease)
    {
        speed += speedIncrease;
    }

    public void ReduceSpeed(int speedDecrease)
    {
        speed -= speedDecrease;
    }

    public void SetSpeed(int velocity)
    {
        speed = (initSpeed + velocity) * speedMultiplier;
    }

    public void AddKapreCigars(float addedCigars)
    {
        kapreCigars += addedCigars;
    }

    public bool IsMaxHealth()
    {
        return (currentHealth == maxHealth);
    }

    public void ActiveQuestReset()
    {
        activeQuest1 = false;
        activeQuest2 = false;
        activeQuest3 = false;
        activeQuest4 = false;
        activeQuest5 = false;
        activeQuest6 = false;
    }

    public void Save(ref PlayerStatsSaveData data)
    {
        data.initSpeed = initSpeed;
        data.speed = speed;
        data.speedMultiplier = speedMultiplier;
        data.basicAttackDamage = basicAttackDamage;
        data.currentHealth = currentHealth;
        data.maxHealth = maxHealth;

        data.dead = dead;
        data.hasMudArmor = hasMudArmor;
        data.hasProtect = hasProtect;


        data.introDone = introDone;
        data.tutorialDone = tutorialDone;
        data.ruinsScene = ruinsScene;
        data.outdoorsScene = outdoorsScene;
        data.questComp = questComp;

        data.kapreCigars = kapreCigars;
        data.clue1 = clue1;
        data.clue2 = clue2;
        data.clue3 = clue3;
        data.clue4 = clue4;
        data.clue5 = clue5;
        data.clue6 = clue6;

        data.activeQuest1 = activeQuest1;
        data.activeQuest2 = activeQuest2;
        data.activeQuest3 = activeQuest3;
        data.activeQuest4 = activeQuest4;
        data.activeQuest5 = activeQuest5;
        data.activeQuest6 = activeQuest6;

        data.tammyFound = tammyFound;
        data.markyFound = markyFound;
        data.apolakiFound = apolakiFound;
        data.tammyScene = tammyScene;
        data.markyScene = markyScene;
        data.apolakiScene = apolakiScene;
        data.apolakiUnlocked = apolakiUnlocked;
        data.ruinsVisitedOnce = ruinsVisitedOnce;

        data.skillESound = skillESound;
        data.skillQSound = skillQSound;
        data.mcSkillSound = mcSkillSound;

        data.DashPurchased = DashPurchased;
        data.MudflingPurchased = MudflingPurchased;
        data.AtkUpPurchased = AtkUpPurchased;
        data.RockyShellPurchased = RockyShellPurchased;
        data.TornadoPurchased = TornadoPurchased;
        data.MudArmorPurchased = MudArmorPurchased;
        data.FirstAidPurchased = FirstAidPurchased;
        data.HpUpPurchased = HpUpPurchased;
        data.FlutePurchased = FlutePurchased;
        data.RegenPurchased = RegenPurchased;
        data.BBPurchased = BBPurchased;

        data.player = player;
        data.playerSavePosition = player != null ? player.transform.position : playerSavePosition;

        data.playerModelIndex = playerModelIndex;
    }

    public void Load(PlayerStatsSaveData data)
    {
        initSpeed = data.initSpeed;
        speed = data.speed;
        speedMultiplier = data.speedMultiplier;
        basicAttackDamage = data.basicAttackDamage;
        currentHealth = data.currentHealth;
        maxHealth = data.maxHealth;

        dead = data.dead;
        hasMudArmor = data.hasMudArmor;
        hasProtect = data.hasProtect;

        introDone = data.introDone;
        tutorialDone = data.tutorialDone;
        ruinsScene = data.ruinsScene;
        outdoorsScene = data.outdoorsScene;
        questComp = data.questComp;

        kapreCigars = data.kapreCigars;
        clue1 = data.clue1;
        clue2 = data.clue2;
        clue3 = data.clue3;
        clue4 = data.clue4;
        clue5 = data.clue5;
        clue6 = data.clue6;

        activeQuest1 = data.activeQuest1;
        activeQuest2 = data.activeQuest2;
        activeQuest3 = data.activeQuest3;
        activeQuest4 = data.activeQuest4;
        activeQuest5 = data.activeQuest5;
        activeQuest6 = data.activeQuest6;

        tammyFound = data.tammyFound;
        markyFound = data.markyFound;
        apolakiFound = data.apolakiFound;
        tammyScene = data.tammyScene;
        markyScene = data.markyScene;
        apolakiScene = data.apolakiScene;
        apolakiUnlocked = data.apolakiUnlocked;
        ruinsVisitedOnce = data.ruinsVisitedOnce;

        skillESound = data.skillESound;
        skillQSound = data.skillQSound;
        mcSkillSound = data.mcSkillSound;

        DashPurchased = data.DashPurchased;
        MudflingPurchased = data.MudflingPurchased;
        AtkUpPurchased = data.AtkUpPurchased;
        RockyShellPurchased = data.RockyShellPurchased;
        TornadoPurchased = data.TornadoPurchased;
        MudArmorPurchased = data.MudArmorPurchased;
        FirstAidPurchased = data.FirstAidPurchased;
        HpUpPurchased = data.HpUpPurchased;
        FlutePurchased = data.FlutePurchased;
        RegenPurchased = data.RegenPurchased;
        BBPurchased = data.BBPurchased;

        player = data.player != null ? data.player : player;
        playerSavePosition = data.playerSavePosition;
        if (player != null)
        {
            player.transform.position = playerSavePosition;
        }

        playerModelIndex = data.playerModelIndex;
    }

    public void ClearSave()
    {
        // Core scene/progression flags
        outdoorsScene = true;
        ruinsScene = false;
        introDone = false;
        tutorialDone = true; // matches InitializePlayerStats
        ruinsVisitedOnce = false;
        questComp = false;
        dead = false;
        hasMudArmor = false;
        hasProtect = false;

        // Stat values
        if (ruinsScene && introDone)
        {
            initSpeed = 0;
        }
        else if (outdoorsScene && introDone == false)
        {
            initSpeed = 0;
        }
        else
        {
            initSpeed = 5;
        }

        if (tammyScene || markyScene || apolakiScene)
        {
            speedMultiplier = 1.5f;
        }
        else
        {
            speedMultiplier = 1.0f;
        }

        speed = 0f;
        basicAttackDamage = 25f;
        maxHealth = 50f;
        currentHealth = maxHealth;

        // Inventory / collectibles
        kapreCigars = 0f;

        // Clues
        clue1 = clue2 = clue3 = clue4 = clue5 = clue6 = false;

        // Active quests
        activeQuest1 = activeQuest2 = activeQuest3 = false;
        activeQuest4 = activeQuest5 = activeQuest6 = false;

        // Character discoveries / scenes
        tammyFound = false;
        markyFound = false;
        apolakiFound = false;
        tammyScene = false;
        markyScene = false;
        apolakiScene = false;
        apolakiUnlocked = false;

        // Skill purchases
        DashPurchased = false;
        MudflingPurchased = false;
        AtkUpPurchased = false;
        RockyShellPurchased = false;
        TornadoPurchased = false;
        MudArmorPurchased = false;
        FirstAidPurchased = false;
        HpUpPurchased = false;
        FlutePurchased = false;
        RegenPurchased = false;
        BBPurchased = false;

        // Player model / saved position
        playerModelIndex = 1;
        playerSavePosition = Vector3.zero;
    }

    public void SetScenePosition()
    {
        // Positioning for cutscenes and starting positions when entering a scene based on QuestState flags
        if (tammyScene && !outdoorsScene)
        {
            playerSavePosition = new Vector3(999.1f, 187.2f, 390f);
        }
        else if (markyScene && !outdoorsScene)
        {
            playerSavePosition = new Vector3(978.8f, 60f, 263.1f);
        }
        else if (apolakiScene && !outdoorsScene)
        {
            playerSavePosition = new Vector3(999.1f, 187.2f, 390f);
        }
        else if (tammyScene && outdoorsScene)
        {
            playerSavePosition = new Vector3(676f, 62.5f, 1336.6f);
            tammyScene = false;
        }
        else if (markyScene && outdoorsScene)
        {
            playerSavePosition = new Vector3(997f, 70.2f, 276.5f);
            markyScene = false;
        }
        else if (introDone && outdoorsScene && !ruinsScene)
        {
            playerSavePosition = new Vector3(632.2f, 59.5f, 285.138f);
        }
        else if (outdoorsScene)
        {
            playerSavePosition = new Vector3(876.24f, 79.24f, 72.68f);
        }
        else if (ruinsScene)
        {
            playerSavePosition = new Vector3(154.1f, 9.2f, 102.7f);
        }
    }
}

[System.Serializable]
public struct PlayerStatsSaveData
{
    public int initSpeed;
    public float speed;
    public float speedMultiplier;
    public float basicAttackDamage;
    public float currentHealth;
    public float maxHealth;

    public bool dead;
    public bool hasMudArmor;
    public bool hasProtect;

    public bool introDone;
    public bool tutorialDone;
    public bool ruinsScene;
    public bool outdoorsScene;
    public bool questComp;

    public float kapreCigars;
    public bool clue1;
    public bool clue2;
    public bool clue3;
    public bool clue4;
    public bool clue5;
    public bool clue6;

    public bool activeQuest1;
    public bool activeQuest2;
    public bool activeQuest3;
    public bool activeQuest4;
    public bool activeQuest5;
    public bool activeQuest6;

    public bool tammyFound;
    public bool markyFound;
    public bool apolakiFound;
    public bool tammyScene;
    public bool markyScene;
    public bool apolakiScene;
    public bool apolakiUnlocked;
    public bool ruinsVisitedOnce;

    public AudioClip skillESound;
    public AudioClip skillQSound;
    public AudioClip mcSkillSound;

    public bool DashPurchased;
    public bool MudflingPurchased;
    public bool AtkUpPurchased;
    public bool RockyShellPurchased;
    public bool TornadoPurchased;
    public bool MudArmorPurchased;
    public bool FirstAidPurchased;
    public bool HpUpPurchased;
    public bool FlutePurchased;
    public bool RegenPurchased;
    public bool BBPurchased;

    public GameObject player;
    public Vector3 playerSavePosition;

    public int playerModelIndex;
}

