using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEHUD : MonoBehaviour
{
    public Image dash;
    public Image mudArmor;
    public Image mudfling;
    public Image tornadoPunch;
    public Image hypnotize;
    public Image poisonBreath;
    public Image slither;
    public Image tailSlap;
    public Image lightningStrike;
    public Image protect;
    public Image swipe;
    public Image rockyShell;
    public Image blank;
    public Image blankReady;

    public AudioClip dashSound;
    public AudioClip mudArmorSound;
    public AudioClip mudFlingSound;
    public AudioClip tornadoPunchSound;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSkillImages();
        ChangeSkill();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerSkills.Instance.behemothSkillECharges > 0)
        {
            blankReady.gameObject.SetActive(true);
        }
        else
        {
            blankReady.gameObject.SetActive(false);
        }

        if(PlayerSkills.Instance.skillEBeingUnequipped)
        {
            ClearSkillEHUD();
            PlayerSkills.Instance.skillEBeingUnequipped = false;
        }

        if (PlayerSkills.Instance.skillEBeingEquipped)
        {
            ChangeSkill(); // If skill is not in the process of being equipped, exit the method
        }
    }

    private void SetImageActive(Image image, bool isActive)
    {
        if (image != null)
        {
            image.gameObject.SetActive(isActive);
        }
    }

    private void InitializeSkillImages()
    {
        TryAssignSkillImage("Dash", ref dash);
        TryAssignSkillImage("MudArmor", ref mudArmor);
        TryAssignSkillImage("Mudfling", ref mudfling);
        TryAssignSkillImage("TornadoPunch", ref tornadoPunch);
        TryAssignSkillImage("Hypnotize", ref hypnotize);
        TryAssignSkillImage("PoisonBreath", ref poisonBreath);
        TryAssignSkillImage("Slither", ref slither);
        TryAssignSkillImage("TailSlap", ref tailSlap);
        TryAssignSkillImage("LightningStrike", ref lightningStrike);
        TryAssignSkillImage("Protect", ref protect);
        TryAssignSkillImage("Swipe", ref swipe);
        TryAssignSkillImage("RockyShell", ref rockyShell);

        RectTransform referenceRect = blank != null ? blank.rectTransform : GetComponent<RectTransform>();
        NormalizeSkillImage(dash, referenceRect);
        NormalizeSkillImage(mudArmor, referenceRect);
        NormalizeSkillImage(mudfling, referenceRect);
        NormalizeSkillImage(tornadoPunch, referenceRect);
        NormalizeSkillImage(hypnotize, referenceRect);
        NormalizeSkillImage(poisonBreath, referenceRect);
        NormalizeSkillImage(slither, referenceRect);
        NormalizeSkillImage(tailSlap, referenceRect);
        NormalizeSkillImage(lightningStrike, referenceRect);
        NormalizeSkillImage(protect, referenceRect);
        NormalizeSkillImage(swipe, referenceRect);
        NormalizeSkillImage(rockyShell, referenceRect);
    }

    private void TryAssignSkillImage(string skillName, ref Image imageField)
    {
        if (imageField == null)
        {
            Transform existingChild = transform.Find(skillName);
            if (existingChild != null)
            {
                imageField = existingChild.GetComponent<Image>();
            }
        }

        if (imageField != null)
        {
            return;
        }

        Image sourceImage = FindSceneImage(skillName);
        if (sourceImage == null)
        {
            return;
        }

        GameObject clone = Instantiate(sourceImage.gameObject, transform, false);
        clone.name = skillName;
        clone.SetActive(false);
        imageField = clone.GetComponent<Image>();
    }

    private Image FindSceneImage(string skillName)
    {
        Image[] sceneImages = Resources.FindObjectsOfTypeAll<Image>();

        foreach (Image candidate in sceneImages)
        {
            if (candidate == null || candidate.gameObject == null)
            {
                continue;
            }

            if (!candidate.gameObject.scene.IsValid())
            {
                continue;
            }

            if (candidate.transform.IsChildOf(transform))
            {
                continue;
            }

            if (candidate.gameObject.name != skillName || candidate.sprite == null)
            {
                continue;
            }

            return candidate;
        }

        return null;
    }

    private void NormalizeSkillImage(Image image, RectTransform referenceRect)
    {
        if (image == null || referenceRect == null)
        {
            return;
        }

        RectTransform imageRect = image.rectTransform;
        imageRect.anchorMin = referenceRect.anchorMin;
        imageRect.anchorMax = referenceRect.anchorMax;
        imageRect.pivot = referenceRect.pivot;
        imageRect.anchoredPosition = referenceRect.anchoredPosition;
        imageRect.localRotation = Quaternion.identity;
        imageRect.localScale = Vector3.one;
    }

    public void ClearSkillEHUD()
    {
        SetImageActive(blank, false);
        SetImageActive(dash, false);
        SetImageActive(mudArmor, false);
        SetImageActive(mudfling, false);
        SetImageActive(tornadoPunch, false);
        SetImageActive(hypnotize, false);
        SetImageActive(poisonBreath, false);
        SetImageActive(slither, false);
        SetImageActive(tailSlap, false);
        SetImageActive(lightningStrike, false);
        SetImageActive(protect, false);
        SetImageActive(swipe, false);
        SetImageActive(rockyShell, false);
    }

    public void ChangeSkill()
    {
        InitializeSkillImages();

        BaseSkill equippedSkill = PlayerSkills.Instance.behemothSkillE;

        if (equippedSkill == null)
        {
            ClearSkillEHUD();
            PlayerStats.Instance.skillESound = null;
            PlayerSkills.Instance.skillEBeingEquipped = false;
            return;
        }

        if (equippedSkill is Dash)
        {
            SetHUDToDash();
            PlayerStats.Instance.skillESound = dashSound;
        }
        else if (equippedSkill is MudArmor)
        {
            SetHUDToMudArmor();
            PlayerStats.Instance.skillESound = mudArmorSound;
        }
        else if (equippedSkill is Mudfling)
        {
            SetHUDToMudfling();
            PlayerStats.Instance.skillESound = mudFlingSound;
        }
        else if (equippedSkill is TornadoPunch)
        {
            SetHUDToTornadoPunch();
            PlayerStats.Instance.skillESound = tornadoPunchSound;
        }
        else if (equippedSkill is Hypnotize)
        {
            SetHUDToHypnotize();
            PlayerStats.Instance.skillESound = null;
            // PlayerStats.Instance.skillESound = hypnotizeSound;
        }
        else if (equippedSkill is Poisonbreath)
        {
            SetHUDToPoisonBreath();
            PlayerStats.Instance.skillESound = null;
            // PlayerStats.Instance.skillESound = poisonBreathSound;
        }
        else if (equippedSkill is Slither)
        {
            SetHUDToSlither();
            PlayerStats.Instance.skillESound = null;
            // PlayerStats.Instance.skillESound = slitherSound;
        }
        else if (equippedSkill is TailSlap)
        {
            SetHUDToTailSlap();
            PlayerStats.Instance.skillESound = null;
            // PlayerStats.Instance.skillESound = tailSlapSound;
        }
        else if (equippedSkill is Lightning)
        {
            SetHUDToLightningStrike();
            PlayerStats.Instance.skillESound = null;
            // PlayerStats.Instance.skillESound = lightningStrikeSound;
        }
        else if (equippedSkill is Protect)
        {
            SetHUDToProtect();
            PlayerStats.Instance.skillESound = null;
            // PlayerStats.Instance.skillESound = protectSound;
        }
        else if (equippedSkill is Swipe)
        {
            SetHUDToSwipe();
            PlayerStats.Instance.skillESound = null;
            // PlayerStats.Instance.skillESound = swipeSound;
        }
        else
        {
            ClearSkillEHUD();
            PlayerStats.Instance.skillESound = null;
            PlayerSkills.Instance.skillEBeingEquipped = false;
        }
    }

    private void SetSkillEHUD(Image skillImage)
    {
        ClearSkillEHUD();
        SetImageActive(skillImage, true);
        PlayerSkills.Instance.skillEBeingEquipped = false;
    }

    public void SetHUDToDash()
    {
        SetSkillEHUD(dash);
    }

    public void SetHUDToMudArmor()
    {
        SetSkillEHUD(mudArmor);
    }

    public void SetHUDToMudfling()
    {
        SetSkillEHUD(mudfling);
    }
    
    public void SetHUDToTornadoPunch()
    {
        SetSkillEHUD(tornadoPunch);
    }

    public void SetHUDToHypnotize()
    {
        SetSkillEHUD(hypnotize);
    }

    public void SetHUDToPoisonBreath()
    {
        SetSkillEHUD(poisonBreath);
    }

    public void SetHUDToSlither()
    {
        SetSkillEHUD(slither);
    }

    public void SetHUDToTailSlap()
    {
        SetSkillEHUD(tailSlap);
    }

    public void SetHUDToLightningStrike()
    {
        SetSkillEHUD(lightningStrike);
    }

    public void SetHUDToProtect()
    {
        SetSkillEHUD(protect);
    }

    public void SetHUDToSwipe()
    {
        SetSkillEHUD(swipe);
    }

    public void SetHUDToRockyShell()
    {
        SetSkillEHUD(rockyShell);
    }
}
