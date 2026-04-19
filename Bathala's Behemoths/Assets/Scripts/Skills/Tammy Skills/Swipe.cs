using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : BaseSkill
{
    public Animator animator;
    int isSwipeHash;
    public AudioClip punchSound;
    public AudioClip blowLands;

    [SerializeField] private float castDuration = 2.0f;
    [SerializeField] private float attackRadius = 12.0f;
    [SerializeField] private float forwardOffset = 8.0f;
    [SerializeField, Range(0f, 180f)] private float attackAngle = 140.0f;

    private PlayerMovement playerMovement;
    private TSAnimController tambanokanoModel;
    private SkinnedMeshRenderer tambanokanoBody;
    private bool movementLocked;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = player != null ? player.GetComponentInChildren<Animator>() : null;
        playerMovement = player != null ? player.GetComponent<PlayerMovement>() : null;
        // isSwipeHash = Animator.StringToHash("isSwipe");
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
        
        // animator.SetBool(isSwipeHash, true);
        LockMovement();
        PlayClip(punchSound);
        yield return new WaitForSeconds(castDuration);
        PlayClip(blowLands);
        DamageEnemiesInArc();
        ReleaseMovementLock();
        // animator.SetBool(isSwipeHash, false);
    }

    private void DamageEnemiesInArc()
    {
        Vector3 origin = ResolveAttackOrigin();
        Vector3 forward = player.transform.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude <= Mathf.Epsilon)
        {
            forward = Vector3.forward;
        }

        forward.Normalize();

        Vector3 attackCenter = origin + forward * forwardOffset;
        float halfAttackAngle = attackAngle * 0.5f;
        HashSet<EnemyMob> hitEnemies = new HashSet<EnemyMob>();
        Collider[] colliders = Physics.OverlapSphere(attackCenter, attackRadius);

        foreach (Collider collider in colliders)
        {
            EnemyMob mob = collider.GetComponent<EnemyMob>();
            if (mob == null)
            {
                mob = collider.GetComponentInParent<EnemyMob>();
            }

            if (mob == null || !hitEnemies.Add(mob))
            {
                continue;
            }

            Vector3 targetPoint = collider.bounds.center;
            Vector3 directionToTarget = targetPoint - origin;
            directionToTarget.y = 0f;

            if (directionToTarget.sqrMagnitude <= Mathf.Epsilon)
            {
                continue;
            }

            if (Vector3.Angle(forward, directionToTarget.normalized) > halfAttackAngle)
            {
                continue;
            }

            mob.TakeDamage(PlayerStats.Instance.basicAttackDamage * 3f);
        }
    }

    private Vector3 ResolveAttackOrigin()
    {
        if (TryResolveTambanokanoBody())
        {
            Vector3 bodyCenter = tambanokanoBody.bounds.center;
            bodyCenter.y = player.transform.position.y;
            return bodyCenter;
        }

        if (TryResolveTambanokanoModel())
        {
            Vector3 modelPosition = tambanokanoModel.transform.position;
            modelPosition.y = player.transform.position.y;
            return modelPosition;
        }

        return player.transform.position;
    }

    private bool TryResolveTambanokanoModel()
    {
        if (tambanokanoModel != null)
        {
            return true;
        }

        tambanokanoModel = player != null ? player.GetComponentInChildren<TSAnimController>(true) : null;
        if (tambanokanoModel == null)
        {
            tambanokanoModel = FindAnyObjectByType<TSAnimController>(FindObjectsInactive.Include);
        }

        return tambanokanoModel != null;
    }

    private bool TryResolveTambanokanoBody()
    {
        if (tambanokanoBody != null)
        {
            return true;
        }

        if (!TryResolveTambanokanoModel())
        {
            return false;
        }

        SkinnedMeshRenderer[] renderers = tambanokanoModel.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            if (renderer != null && renderer.enabled)
            {
                tambanokanoBody = renderer;
                return true;
            }
        }

        if (renderers.Length > 0)
        {
            tambanokanoBody = renderers[0];
        }

        return tambanokanoBody != null;
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
