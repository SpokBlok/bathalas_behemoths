using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailSlap : BaseSkill
{
    public Animator animator;
    public AudioClip punchSound;
    public AudioClip blowLands;

    [SerializeField] private float castDuration = 2.0f;
    [SerializeField] private float coneRange = 15.0f;
    [SerializeField, Range(0f, 180f)] private float coneAngle = 90.0f;

    private PlayerMovement playerMovement;
    private bool movementLocked;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player != null ? player.GetComponentInChildren<Animator>() : null;
        playerMovement = player != null ? player.GetComponent<PlayerMovement>() : null;

        maxCharges = 1;
        cooldown = 10;
        skillCode = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null || PlayerStats.Instance == null)
        {
            yield break;
        }

        animator = player.GetComponentInChildren<Animator>();
        playerMovement = player.GetComponent<PlayerMovement>();

        if (playerMovement == null)
        {
            yield break;
        }

        if (playerMovement.basicAttackCoroutine != null)
        {
            StopCoroutine(playerMovement.basicAttackCoroutine);
            playerMovement.basicAttackCoroutine = null;
        }

        LockMovement();
        PlayClip(punchSound);

        yield return new WaitForSeconds(castDuration);

        PlayClip(blowLands);
        DamageEnemiesInCone();
        ReleaseMovementLock();
    }

    private void DamageEnemiesInCone()
    {
        HashSet<EnemyMob> hitEnemies = new HashSet<EnemyMob>();
        Vector3 origin = player.transform.position;
        Vector3 forward = player.transform.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude <= Mathf.Epsilon)
        {
            forward = Vector3.forward;
        }

        float halfConeAngle = coneAngle * 0.5f;
        float damage = PlayerStats.Instance.basicAttackDamage * 3f;
        Collider[] colliders = Physics.OverlapSphere(origin, coneRange);

        foreach (Collider collider in colliders)
        {
            if (!collider.TryGetComponent<EnemyMob>(out var mob) || hitEnemies.Contains(mob))
            {
                continue;
            }

            Vector3 directionToTarget = collider.bounds.center - origin;
            directionToTarget.y = 0f;

            if (directionToTarget.sqrMagnitude <= Mathf.Epsilon)
            {
                continue;
            }

            if (Vector3.Angle(forward, directionToTarget.normalized) > halfConeAngle)
            {
                continue;
            }

            hitEnemies.Add(mob);
            mob.TakeDamage(damage);
        }
    }

    private void LockMovement()
    {
        movementLocked = true;
        playerMovement.isSkillingOrUlting = true;
        playerMovement.isAttacking = true;
        playerMovement.currentVelocity = Vector3.zero;
    }

    private void ReleaseMovementLock()
    {
        if (!movementLocked)
        {
            return;
        }

        if (playerMovement != null)
        {
            playerMovement.isSkillingOrUlting = false;
            playerMovement.isAttacking = false;
            playerMovement.currentVelocity = Vector3.zero;
            playerMovement.StateCheck();
        }

        movementLocked = false;
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null || Camera.main == null)
        {
            return;
        }

        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position + Camera.main.transform.forward * 2f, 1f);
    }

    private void OnDisable()
    {
        ReleaseMovementLock();
    }
}
