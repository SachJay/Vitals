using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public delegate void PlayerAttackEvent();
    public PlayerAttackEvent OnAttackStarted;
    public PlayerAttackEvent OnAttackEnded;
    public PlayerAttackEvent OnEnemyKilled;

    public bool IsAttacking { get; private set; }

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem enemyDeathParticlesPrefab;

    [Header("Attack Variables")]
    [SerializeField] private float maxAttackCount = 1;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float maxAttackDistance = 7;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private AttackIndicator[] attackIndicators;
    [SerializeField] private CircleCollider2D attackHitbox;

    private Vector2 attackDestination = Vector2.zero;
    private float currentAttackCount = 1;
    private float elapsedTime = 0;

    private void Awake()
    {
        if (player == null)
            LogExtension.LogMissingVariable(name, nameof(player));

        if (enemyDeathParticlesPrefab == null)
            LogExtension.LogMissingVariable(name, nameof(enemyDeathParticlesPrefab));

        if (attackHitbox == null)
            LogExtension.LogMissingVariable(name, nameof(attackHitbox));
    }

    private void Start()
    {
        player.PlayerInputHandler.OnAttackInputStarted += PlayerInput_OnAttackStarted;
        OnEnemyKilled += PlayerAttack_OnEnemyKilled;

        attackHitbox.enabled = false;
        SetTrailRenderer(false);
        UpdateAttackCount();
    }

    private void FixedUpdate()
    {
        if (!player.IsOwned)
            return;

        if (!IsAttacking)
            return;

        Attack();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsAttacking)
        {
            // TODO: This needs to be fixed for multiplayer later
            //if (other.gameObject.TryGetComponent(out Enemy enemy) && enemy.transitionOnDeath)
            //    StartCoroutine(LoadNewScene());

            HandleDeathParticles(other.gameObject);
            Destroy(other.gameObject);

            OnEnemyKilled?.Invoke();
        }
    }

    // TODO: Remove this when fixing above OnTriggerEnter2D function
    private IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(2);
        //player.LoadNextScene();
    }

    public void AddAttack()
    {
        currentAttackCount++;
    }

    private void Attack()
    {
        elapsedTime += Time.fixedDeltaTime;

        float percentComplete = elapsedTime / attackDuration;

        player.transform.position = Vector2.Lerp(player.transform.position, attackDestination, percentComplete);

        if (Vector2.Distance(player.transform.position, attackDestination) < 0.2f)
        {
            player.transform.position = attackDestination;
            EndAttack();
        }
    }

    private void PlayerInput_OnAttackStarted()
    {
        if (!CanAttack())
            return;

        currentAttackCount--;

        IsAttacking = true;
        OnAttackStarted?.Invoke();

        elapsedTime = 0;

        GetAttackLocation();

        attackHitbox.enabled = true;
        SetTrailRenderer(true);

        foreach (AttackIndicator attackIndicator in attackIndicators)
        {
            if (!attackIndicator.IsStarted)
            {
                attackIndicator.StartTimer(attackCooldown);
                break;
            }
        }
    }

    private void PlayerAttack_OnEnemyKilled()
    {
        EndAttack();
        for (int i = 0; i < maxAttackCount; i++)
        {
            attackIndicators[i].ResetTimer();
        }
    }

    private void HandleDeathParticles(GameObject enemy)
    {
        // TODO: Add enemy death particles prefab
        if (enemyDeathParticlesPrefab == null)
            return;

        ParticleSystem deathParticles = Instantiate(enemyDeathParticlesPrefab, player.transform.position, Quaternion.identity);

        Vector3 difference = player.transform.position - enemy.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, -difference.x) * Mathf.Rad2Deg;
        deathParticles.transform.SetPositionAndRotation(enemy.transform.position, Quaternion.Euler(rotationZ, 90.0f, 0));
    }

    private bool CanAttack()
    {
        if (!player.PlayerStats.IsAlive || currentAttackCount <= 0 || IsAttacking)
            return false;
        return true;
    }

    private void GetAttackLocation()
    {
        attackDestination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance((Vector2)player.transform.position, attackDestination) > maxAttackDistance)
        {
            Vector2 maxAttackDistVec = (attackDestination - (Vector2)player.transform.position).normalized;

            attackDestination = (Vector2)player.transform.position + maxAttackDistVec * maxAttackDistance;
        }
    }

    private void EndAttack()
    {
        attackHitbox.enabled = false;
        SetTrailRenderer(false);

        IsAttacking = false;
        OnAttackEnded?.Invoke();
    }

    private void UpdateAttackCount()
    {
        // Attacks
        currentAttackCount = maxAttackCount;

        foreach (AttackIndicator ind in attackIndicators)
        {
            ind.gameObject.SetActive(false);

            // TODO: Fix this to have a general timer and not be controlled by a UI
            ind.player = player;
        }

        for (int i = 0; i < maxAttackCount; i++)
        {
            attackIndicators[i].gameObject.SetActive(true);
        }
    }

    // TODO: Move out of dashing to its own script
    public void SetTrailRenderer(bool newState)
    {
        trailRenderer.emitting = newState;
    }
}
