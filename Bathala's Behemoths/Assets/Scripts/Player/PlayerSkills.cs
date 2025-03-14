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
    public bool skillMCCDStart = false;
    public Coroutine mainCharacterSkillCoroutine = null;

    public BaseSkill behemothSkillQ;
    public bool behemothSkillQIsEquipped = false;
    public int behemothSkillQCharges;
    public bool behemothSkillQCharging;
    public float behemothSkillQChargeTimer;
    public bool skillQCDStart = false;
    public Coroutine behemothSkillQCoroutine = null;

    public BaseSkill behemothSkillE;
    public bool behemothSkillEIsEquipped = false;
    public int behemothSkillECharges;
    public bool behemothSkillECharging;
    public float behemothSkillEChargeTimer;
    public bool skillECDStart = false;
    public Coroutine behemothSkillECoroutine = null;

    public bool skillMCBeingEquipped = false;
    public bool skillEBeingEquipped = false;
    public bool skillQBeingEquipped = false;
    
    public bool skillMCBeingUnequipped = false;
    public bool skillQBeingUnequipped = false;
    public bool skillEBeingUnequipped = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCharacterSkillIsEquipped)
        {
            MainCharacterSkillChargeTimer();
        }
        if (behemothSkillQIsEquipped)
        {
            BehemothSkillQChargeTimer();
        }
        if (behemothSkillEIsEquipped)
        {
            BehemothSkillEChargeTimer();
        }
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
        skillMCCDStart = false;
        skillMCBeingEquipped = true;
    }

    public void BehemothSkillQChange(BaseSkill newSkill)
    {
        behemothSkillQ = newSkill;
        behemothSkillQChargeTimer = newSkill.cooldown;
        behemothSkillQCharges = newSkill.maxCharges;
        behemothSkillQIsEquipped = true;
        skillQBeingEquipped = true;
    }

    public void BehemothSkillEChange(BaseSkill newSkill)
    {
        behemothSkillE = newSkill;
        behemothSkillEChargeTimer = newSkill.cooldown;
        behemothSkillECharges = newSkill.maxCharges;
        behemothSkillEIsEquipped = true;
        skillEBeingEquipped = true;
    }

    public void RemoveMainCharacterSkill()
    {
        mainCharacterSkill = null;
        mainCharacterSkillChargeTimer = 0;
        mainCharacterSkillCharges = 0;
        mainCharacterSkillIsEquipped = false;
        mainCharacterSkillCharging = false;
        skillMCBeingUnequipped = true;
    }

    public void RemoveBehemothSkillQ()
    {
        behemothSkillQ = null;
        behemothSkillQChargeTimer = 0;
        behemothSkillQCharges = 0;
        behemothSkillQIsEquipped = false;
        behemothSkillQCharging = false;
        skillQBeingUnequipped = true;
    }

    public void RemoveBehemothSkillE()
    {
        behemothSkillE = null;
        behemothSkillEChargeTimer = 0;
        behemothSkillECharges = 0;
        behemothSkillEIsEquipped = false;
        behemothSkillECharging = false;
        skillEBeingUnequipped = true;
    }

    private void SkillMCChargeTimer(ref bool isEquipped, ref bool isCharging, ref float chargeTimer, ref int charges, int maxCharges, float cooldown)
    {
        if (!isEquipped)
        {
            return;;
        }

        if (charges < maxCharges)
        {
            isCharging = true;
        }

        if (isCharging)
        {
            if (charges >= maxCharges)
            {
                isCharging = false;
                chargeTimer = cooldown;
                skillMCCDStart = false;
            }
            else if (chargeTimer <= 0)
            {
                charges++;
                chargeTimer = cooldown;
            }
            else
            {
                chargeTimer -= Time.deltaTime;
                skillMCCDStart = true;
            }
            // Debug.Log("skillMCCDStart value: " + skillMCCDStart);
        }
    }
    
    private void SkillEChargeTimer(ref bool isEquipped, ref bool isCharging, ref float chargeTimer, ref int charges, int maxCharges, float cooldown)
    {
        if (!isEquipped)
        {
            return;;
        }

        if (charges < maxCharges)
        {
            isCharging = true;
        }

        if (isCharging)
        {
            if (charges >= maxCharges)
            {
                isCharging = false;
                chargeTimer = cooldown;
                skillECDStart = false;
            }
            else if (chargeTimer <= 0)
            {
                charges++;
                chargeTimer = cooldown;
            }
            else
            {
                chargeTimer -= Time.deltaTime;
                skillECDStart = true;
            }
        }
    }

    private void SkillQChargeTimer(ref bool isEquipped, ref bool isCharging, ref float chargeTimer, ref int charges, int maxCharges, float cooldown)
    {
        if (!isEquipped)
        {
            return;
        }

        if (charges < maxCharges)
        {
            isCharging = true;
        }

        if (isCharging)
        {
            if (charges >= maxCharges)
            {
                isCharging = false;
                chargeTimer = cooldown;
                skillQCDStart = false;
            }
            else if (chargeTimer <= 0)
            {
                charges++;
                chargeTimer = cooldown;
            }
            else
            {
                chargeTimer -= Time.deltaTime;
                skillQCDStart = true;
            }
        }
    }

    public void MainCharacterSkillChargeTimer()
    {
        // Debug.Log("MCChargeTimer Called");
        SkillMCChargeTimer(
            ref mainCharacterSkillIsEquipped,
            ref mainCharacterSkillCharging,
            ref mainCharacterSkillChargeTimer,
            ref mainCharacterSkillCharges,
            mainCharacterSkill.maxCharges,
            mainCharacterSkill.cooldown
        );
    }

    public void BehemothSkillQChargeTimer()
    {
        // Debug.Log("QChargeTimer Called");
        SkillQChargeTimer(
            ref behemothSkillQIsEquipped,
            ref behemothSkillQCharging,
            ref behemothSkillQChargeTimer,
            ref behemothSkillQCharges,
            behemothSkillQ.maxCharges,
            behemothSkillQ.cooldown
        );
    }

    public void BehemothSkillEChargeTimer()
    {
        // Debug.Log("EChargeTimer Called");
        SkillEChargeTimer(
            ref behemothSkillEIsEquipped,
            ref behemothSkillECharging,
            ref behemothSkillEChargeTimer,
            ref behemothSkillECharges,
            behemothSkillE.maxCharges,
            behemothSkillE.cooldown
        );
    }

    public IEnumerator RunMainCharacterSkill()
    {
        // Debug.Log("RunMCSkill Parameters:" + mainCharacterSkillIsEquipped + (mainCharacterSkillCoroutine == null) + mainCharacterSkillCharges + mainCharacterSkill.oneTimeUseAvailable);
        if (mainCharacterSkillIsEquipped && mainCharacterSkillCoroutine == null && 
            (mainCharacterSkillCharges > 0 
            || mainCharacterSkill.oneTimeUseAvailable))
        {
            Debug.Log("Running Main Character Skill!");
            AudioSource.PlayClipAtPoint(PlayerStats.Instance.mcSkillSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        
            mainCharacterSkillCharges--;
            mainCharacterSkill.oneTimeUseAvailable = false;
            mainCharacterSkillCoroutine = StartCoroutine(mainCharacterSkill.RunSkill());
            yield return mainCharacterSkillCoroutine;
            mainCharacterSkillCoroutine = null;
        }
    }

    public IEnumerator RunBehemothSkillQ()
    {
        Debug.Log("RunQSkill Parameters:" + behemothSkillQIsEquipped + (behemothSkillQCoroutine == null) + behemothSkillQCharges + behemothSkillQ.oneTimeUseAvailable);
        if (behemothSkillQIsEquipped && behemothSkillQCoroutine == null &&
            (behemothSkillQCharges > 0
            || behemothSkillQ.oneTimeUseAvailable))
        {
            AudioSource.PlayClipAtPoint(PlayerStats.Instance.skillQSound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        
            behemothSkillQCharges--;
            behemothSkillQ.oneTimeUseAvailable = false;
            behemothSkillQCoroutine = StartCoroutine(behemothSkillQ.RunSkill());
            yield return behemothSkillQCoroutine;
            behemothSkillQCoroutine = null;
        }
    }

    public IEnumerator RunBehemothSkillE()
    {
        if (behemothSkillEIsEquipped && behemothSkillECoroutine == null &&
            (behemothSkillECharges > 0
            || behemothSkillE.oneTimeUseAvailable))
        {
            AudioSource.PlayClipAtPoint(PlayerStats.Instance.skillESound, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
        
            behemothSkillECharges--;
            behemothSkillE.oneTimeUseAvailable = false;
            behemothSkillECoroutine = StartCoroutine(behemothSkillE.RunSkill());
            yield return behemothSkillECoroutine;
            behemothSkillECoroutine = null;
        }
    }
}
