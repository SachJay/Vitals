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

    [Header("Attack Variables")]
    [SerializeField] private float maxAttackCount = 1;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float maxAttackDistance = 7;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private AbilityTimer[] abilityTimers;
    [SerializeField] private CircleCollider2D attackHitbox;

    private Vector2 attackDestination = Vector2.zero;
    private float currentAttackCount = 1;
    private float elapsedTime = 0;

    private void Awake()
    {
        if (player == null)
            LogExtension.LogMissingVariable(name, nameof(player));

        if (attackHitbox == null)
            LogExtension.LogMissingVariable(name, nameof(attackHitbox));
    }

    private void Start()
    {
        player.PlayerInputHandler.OnAttackInputStarted += PlayerInput_OnAttackStarted;
        OnEnemyKilled += PlayerAttack_OnEnemyKilled;

        attackHitbox.enabled = false;
        SetTrailRenderer(false);
        InitAttack();
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
        if (!IsAttacking)
            return;

        // TODO: This needs to be fixed for multiplayer later
        //if (other.gameObject.TryGetComponent(out Enemy enemy) && enemy.transitionOnDeath)
        //    StartCoroutine(LoadNewScene());

        if (other.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(player.PlayerStats, 1);

        if (other.TryGetComponent(out Enemy _))
            OnEnemyKilled?.Invoke();
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

        AbilityTimer abilityTimer = GetFirstAvailableAbilityTimer();
        if (abilityTimer != null)
            abilityTimer.StartTimer(attackCooldown);
    }

    private void PlayerAttack_OnEnemyKilled()
    {
        EndAttack();

        for (int i = 0; i < maxAttackCount; i++)
            abilityTimers[i].OnTimerTimeout?.Invoke(attackCooldown);
    }

    private bool CanAttack()
    {
        if (player.PlayerStats.IsDead || currentAttackCount <= 0 || IsAttacking)
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

    private void InitAttack()
    {
        currentAttackCount = maxAttackCount;
        for (int index = 0; index < abilityTimers.Length; index++)
        {
            AbilityTimer abilityTimer = abilityTimers[index];
            abilityTimer.gameObject.SetActive(index < maxAttackCount);
            abilityTimer.OnTimerTimeout += AbilityTimer_OnTimerTimeout;
        }
    }

    private void AbilityTimer_OnTimerTimeout(float _)
    {
        currentAttackCount++;
    }

    private AbilityTimer GetFirstAvailableAbilityTimer()
    {
        foreach (AbilityTimer abilityTimer in abilityTimers)
        {
            if (!abilityTimer.IsStarted)
                return abilityTimer;
        }
        return null;
    }

    // TODO: Move out of dashing to its own script
    public void SetTrailRenderer(bool newState)
    {
        trailRenderer.emitting = newState;
    }
}
