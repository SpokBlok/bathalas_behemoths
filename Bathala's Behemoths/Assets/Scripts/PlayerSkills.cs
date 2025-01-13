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
    public Coroutine mainCharacterSkillCoroutine = null;

    public BaseSkill behemothSkillQ;
    public bool behemothSkillQIsEquipped = false;
    public int behemothSkillQCharges;
    public bool behemothSkillQCharging;
    public float behemothSkillQChargeTimer;
    public Coroutine behemothSkillQCoroutine = null;

    public BaseSkill behemothSkillE;
    public bool behemothSkillEIsEquipped = false;
    public int behemothSkillECharges;
    public bool behemothSkillECharging;
    public float behemothSkillEChargeTimer;
    public Coroutine behemothSkillECoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MainCharacterSkillChargeTimer();
        BehemothSkillQChargeTimer();
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
        RemoveMainCharacterSkill();
        mainCharacterSkill = newSkill;
        mainCharacterSkillChargeTimer = newSkill.cooldown;
        mainCharacterSkillCharges = newSkill.maxCharges;
        mainCharacterSkillIsEquipped = true;
    }

    public void BehemothSkillQChange(BaseSkill newSkill)
    {
        behemothSkillQ = newSkill;
        behemothSkillQChargeTimer = newSkill.cooldown;
        behemothSkillQCharges = newSkill.maxCharges;
        behemothSkillQIsEquipped = true;
    }

    public void BehemothSkillEChange(BaseSkill newSkill)
    {
        behemothSkillE = newSkill;
        behemothSkillEChargeTimer = newSkill.cooldown;
        behemothSkillECharges = newSkill.maxCharges;
        behemothSkillEIsEquipped = true;
    }

    public void RemoveMainCharacterSkill()
    {
        mainCharacterSkill = null;
        mainCharacterSkillChargeTimer = 0;
        mainCharacterSkillCharges = 0;
        mainCharacterSkillIsEquipped = false;
        mainCharacterSkillCharging = false;
    }

    public void RemoveBehemothSkillQ()
    {
        behemothSkillQ = null;
        behemothSkillQChargeTimer = 0;
        behemothSkillQCharges = 0;
        behemothSkillQIsEquipped = false;
        behemothSkillQCharging = false;
    }

    public void RemoveBehemothSkillE()
    {
        behemothSkillE = null;
        behemothSkillEChargeTimer = 0;
        behemothSkillECharges = 0;
        behemothSkillEIsEquipped = false;
        behemothSkillECharging = false;
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

    public void BehemothSkillQChargeTimer()
    {
        if (!behemothSkillQIsEquipped)
        {
            return;
        }
        if (behemothSkillQCharges < behemothSkillQ.maxCharges)
        {
            behemothSkillQCharging = true;
        }

        if (behemothSkillQCharging)
        {
            if (behemothSkillQChargeTimer <= 0)
            {
                behemothSkillQCharges++;
                behemothSkillQChargeTimer = behemothSkillQ.cooldown;
                if (behemothSkillQCharges == behemothSkillQ.maxCharges)
                {
                    behemothSkillQCharging = false;
                }
            }
            else
            {
                behemothSkillQChargeTimer -= Time.deltaTime;
            }
        }
    }

    public void BehemothSkillEChargeTimer()
    {
        if (!behemothSkillEIsEquipped)
        {
            return;
        }
        if (behemothSkillECharges < behemothSkillE.maxCharges)
        {
            behemothSkillECharging = true;
        }

        if (behemothSkillECharging)
        {
            if (behemothSkillEChargeTimer <= 0)
            {
                behemothSkillECharges++;
                behemothSkillEChargeTimer = behemothSkillE.cooldown;
                if (behemothSkillECharges == behemothSkillE.maxCharges)
                {
                    behemothSkillECharging = false;
                }
            }
            else
            {
                behemothSkillEChargeTimer -= Time.deltaTime;
            }
        }
    }

    public IEnumerator RunMainCharacterSkill()
    {
        if (mainCharacterSkillIsEquipped && mainCharacterSkillCoroutine == null && 
            (mainCharacterSkillCharges > 0 
            || mainCharacterSkill.oneTimeUseAvailable))
        {
            mainCharacterSkillCharges--;
            mainCharacterSkill.oneTimeUseAvailable = false;
            mainCharacterSkillCoroutine = StartCoroutine(mainCharacterSkill.RunSkill());
            yield return mainCharacterSkillCoroutine;
            mainCharacterSkillCoroutine = null;
        }
    }

    public IEnumerator RunBehemothSkillQ()
    {
        if (behemothSkillQIsEquipped && behemothSkillQCoroutine == null &&
            (behemothSkillQCharges > 0
            || behemothSkillQ.oneTimeUseAvailable))
        {
            behemothSkillQCharges--;
            behemothSkillQ.oneTimeUseAvailable = false;
            behemothSkillQCoroutine = StartCoroutine(behemothSkillQ.RunSkill());
            yield return behemothSkillQCoroutine;
            behemothSkillQCoroutine = null;
        }
    }

    public void RunBehemothSkillE()
    {
        if (behemothSkillEIsEquipped && behemothSkillECoroutine == null &&
            (behemothSkillECharges > 0
            || behemothSkillE.oneTimeUseAvailable))
        {
            behemothSkillECharges--;
            behemothSkillE.oneTimeUseAvailable = false;
            StartCoroutine(behemothSkillE.RunSkill());
        }
    }
}
