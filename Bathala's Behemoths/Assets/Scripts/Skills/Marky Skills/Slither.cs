using System.Collections;
using UnityEngine;

public class Slither : BaseSkill
{
    public Animator animator;
    int isSlitheringHash;

    [SerializeField] private float speedBoostAmount = 0.8f;
    [SerializeField] private float speedBoostDuration = 8f;
    [SerializeField] private float animationDuration = 0.4f;

    private bool speedBoostActive;
    private int speedBoostVersion;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player != null ? player.GetComponentInChildren<Animator>() : null;

        isSlitheringHash = Animator.StringToHash("isSlithering");

        maxCharges = 2;
        cooldown = 15;
        skillCode = 1;
    }

    public override IEnumerator RunSkill()
    {
        if (PlayerStats.Instance == null)
        {
            yield break;
        }

        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            animator = player.GetComponentInChildren<Animator>();
        }

        ApplySpeedBoost();

        if (animator != null)
        {
            animator.SetBool(isSlitheringHash, true);
            yield return new WaitForSeconds(animationDuration);
            animator.SetBool(isSlitheringHash, false);
        }

        int currentSpeedBoostVersion = speedBoostVersion;
        yield return new WaitForSeconds(speedBoostDuration);

        if (currentSpeedBoostVersion == speedBoostVersion)
        {
            RemoveSpeedBoost();
        }
    }

    private void ApplySpeedBoost()
    {
        if (speedBoostActive)
        {
            PlayerStats.Instance.speedMultiplier -= speedBoostAmount;
        }

        speedBoostVersion++;
        speedBoostActive = true;
        PlayerStats.Instance.speedMultiplier += speedBoostAmount;
    }

    private void RemoveSpeedBoost()
    {
        if (!speedBoostActive || PlayerStats.Instance == null)
        {
            speedBoostActive = false;
            return;
        }

        PlayerStats.Instance.speedMultiplier -= speedBoostAmount;
        speedBoostActive = false;
    }

    private void OnDisable()
    {
        if (animator != null)
        {
            animator.SetBool(isSlitheringHash, false);
        }

        RemoveSpeedBoost();
    }
}
