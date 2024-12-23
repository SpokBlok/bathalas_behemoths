using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static PlayerStats Instance { get; private set; }

    public int speed;
    public float basicAttackDamage;

    public bool dashSkillEquipped;
    public bool rangedSkillEquipped;
    public bool noSkillEquipped;

    public bool rangedUltEquipped;
    public bool berserkUltEquipped;
    public bool noUltEquipped;
    public bool introDone;
    public bool outdoorsScene;
    public bool questComp;

    private void Awake()
    {
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

    private void InitializePlayerStats()
    {
        // Set default values
        speed = 10;
        basicAttackDamage = 25;

        dashSkillEquipped = false;
        rangedSkillEquipped = true;
        noSkillEquipped = false;

        rangedUltEquipped = true;
        berserkUltEquipped = false;
        noUltEquipped = false;
        introDone = false;
        outdoorsScene = true;
}

    public void AddBasicAttackDamage(int damageIncrease)
    {
        basicAttackDamage += damageIncrease;
    }

    public void AddSpeed(int speedIncrease)
    {
        speed += speedIncrease;
    }
}
