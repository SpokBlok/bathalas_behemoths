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

    public BaseSkill behemothSkill1;
    public bool behemothSkill1IsEquipped = false;
    public int behemothSkill1Charges;
    public bool behemothSkill1Charging;
    public float behemothSkill1ChargeTimer;

    public BaseSkill behemothSkill2;
    public bool behemothSkill2IsEquipped = false;
    public int behemothSkill2Charges;
    public bool behemothSkill2Charging;
    public float behemothSkill2ChargeTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MainCharacterSkillChargeTimer();
        BehemothSkill1ChargeTimer();
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
        mainCharacterSkillIsEquipped = true;
    }

    public void BehemothSkill1Change(BaseSkill newSkill)
    {
        behemothSkill1 = newSkill;
        behemothSkill1ChargeTimer = newSkill.cooldown;
        behemothSkill1Charges = newSkill.maxCharges;
        behemothSkill1IsEquipped = true;
    }

    public void BehemothSkill2Change(BaseSkill newSkill)
    {
        behemothSkill2 = newSkill;
        behemothSkill2ChargeTimer = newSkill.cooldown;
        behemothSkill2Charges = newSkill.maxCharges;
        behemothSkill2IsEquipped = true;
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

    public void BehemothSkill1ChargeTimer()
    {
        if (!behemothSkill1IsEquipped)
        {
            return;
        }
        if (behemothSkill1Charges < behemothSkill1.maxCharges)
        {
            behemothSkill1Charging = true;
        }

        if (behemothSkill1Charging)
        {
            if (behemothSkill1ChargeTimer <= 0)
            {
                behemothSkill1Charges++;
                behemothSkill1ChargeTimer = behemothSkill1.cooldown;
                if (behemothSkill1Charges == behemothSkill1.maxCharges)
                {
                    behemothSkill1Charging = false;
                }
            }
            else
            {
                behemothSkill1ChargeTimer -= Time.deltaTime;
            }
        }
    }

    public void BehemothSkill2ChargeTimer()
    {
        if (!behemothSkill2IsEquipped)
        {
            return;
        }
        if (behemothSkill2Charges < behemothSkill2.maxCharges)
        {
            behemothSkill2Charging = true;
        }

        if (behemothSkill2Charging)
        {
            if (behemothSkill2ChargeTimer <= 0)
            {
                behemothSkill2Charges++;
                behemothSkill2ChargeTimer = behemothSkill2.cooldown;
                if (behemothSkill2Charges == behemothSkill2.maxCharges)
                {
                    behemothSkill2Charging = false;
                }
            }
            else
            {
                behemothSkill2ChargeTimer -= Time.deltaTime;
            }
        }
    }

    public void RunMainCharacterSkill()
    {
        if (mainCharacterSkillIsEquipped && (mainCharacterSkillCharges > 0 
            || mainCharacterSkill.oneTimeUseAvailable))
        {
            mainCharacterSkillCharges--;
            mainCharacterSkill.oneTimeUseAvailable = false;
            StartCoroutine(mainCharacterSkill.RunSkill());
        }
    }

    public void RunBehemothSkill1()
    {
        if (behemothSkill1IsEquipped && (behemothSkill1Charges > 0
            || behemothSkill1.oneTimeUseAvailable))
        {
            behemothSkill1Charges--;
            behemothSkill1.oneTimeUseAvailable = false;
            StartCoroutine(behemothSkill1.RunSkill());
        }
    }

    public void RunBehemothSkill2()
    {
        if (behemothSkill2IsEquipped && (behemothSkill2Charges > 0
            || behemothSkill2.oneTimeUseAvailable))
        {
            behemothSkill2Charges--;
            behemothSkill2.oneTimeUseAvailable = false;
            StartCoroutine(behemothSkill2.RunSkill());
        }
    }
}
