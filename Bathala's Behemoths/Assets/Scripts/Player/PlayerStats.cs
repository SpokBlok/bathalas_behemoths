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
    public float speedMultiplier;
    public float basicAttackDamage;
    public float currentHealth;
    public float maxHealth;

    public bool hasMudArmor;

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

    public bool tammyFound;
    public bool markyFound;

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
        outdoorsScene = true;
        ruinsScene = false;
        introDone = false;
        tutorialDone = false;

        // Set default values
        if(ruinsScene && introDone)
        {
            speed = 15;
        }
        else if(outdoorsScene && introDone == false)
        {
            speed = 15;
        }
        else
        {
            speed = 45;
        }
        speedMultiplier = 1;
        basicAttackDamage = 25;
        maxHealth = 50;
        currentHealth = maxHealth;

        hasMudArmor = false;

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

    public void AddKapreCigars(float addedCigars)
    {
        kapreCigars += addedCigars;
    }

    public bool IsMaxHealth()
    {
        return (currentHealth == maxHealth);
    }
}
