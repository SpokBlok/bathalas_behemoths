using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public BaseSkill mainCharacterSkill;
    public bool mainCharacterSkillIsEquipped = false;
    public int mainCharacterSkillCharges;
    public bool mainCharacterSkillCharging;
    public float mainCharacterSkillChargeTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MainCharacterSkillChargeTimer();
    }

    public static PlayerSkills Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    public void MainCharacterSkillChange(BaseSkill newSkill)
    {
        mainCharacterSkill = newSkill;
        mainCharacterSkillChargeTimer = newSkill.cooldown;
        mainCharacterSkillCharges = newSkill.maxCharges;
    }

    public void MainCharacterSkillChargeTimer()
    {
        if (!mainCharacterSkillIsEquipped)
        {
            return;
        }
        if (mainCharacterSkillCharges < mainCharacterSkill.maxCharges)
        {
            mainCharacterSkillCharging = true;
        }

        if (mainCharacterSkillCharging)
        {
            if (mainCharacterSkillChargeTimer <= 0)
            {
                mainCharacterSkillCharges++;
                mainCharacterSkillChargeTimer = mainCharacterSkill.cooldown;
                if (mainCharacterSkillCharges == mainCharacterSkill.maxCharges)
                {
                    mainCharacterSkillCharging = false;
                }
            }
            else
            {
                mainCharacterSkillChargeTimer -= Time.deltaTime;
            }
        }
    }

    public void RunMainCharacterSkill()
    {
        if (mainCharacterSkillCharges > 0)
        {
            mainCharacterSkillCharges--;
            StartCoroutine(mainCharacterSkill.RunSkill());
        }
    }
}
