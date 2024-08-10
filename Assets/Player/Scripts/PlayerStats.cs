using Mirror;
using System.Collections;
using UnityEngine;

public class PlayerStats : NetworkBehaviour, IDamageable
{
    public delegate void PlayerStatsEvent();
    public PlayerStatsEvent OnDie;

    public bool IsDead => isDead;
    public bool IsInvincible { get; private set; }

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Other Configurations")]
    [SerializeField] private ParticleSystem playerDeathParticlesPrefab;
    [SerializeField] private float killInvincibilityDuration = 0.2f;
    [SerializeField] private float reviveInvincibilityDuration = 0.5f;
    [SerializeField] private float dashEndingInvincibilityDuration = 0.1f;

    [SyncVar(hook = nameof(OnIsDeadChanged))]
    private bool isDead;

    private Coroutine invincibilityCoroutine = null;

    private void Awake()
    {
        isDead = false;
    }

    private void Start()
    {
        player.PlayerAttack.OnEnemyKilled += PlayerAttack_OnKillEnemy;
        player.PlayerAttack.OnAttackStarted += PlayerAttack_OnAttackStarted;
        player.PlayerAttack.OnAttackEnded += PlayerAttack_OnAttackEnded;

        player.PlayerDash.OnDashStarted += PlayerDash_OnDashStarted;
        player.PlayerDash.OnDashEnded += PlayerDash_OnDashEnded;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(null, 0);
        }
    }
#endif

    public void StartInvulnerability(float invincibilityDuration)
    {
        if (invincibilityCoroutine != null)
            StopCoroutine(invincibilityCoroutine);
        invincibilityCoroutine = StartCoroutine(GainTempInvincibility(invincibilityDuration));
    }

    private void PlayerDash_OnDashStarted()
    {
        IsInvincible = true;
    }

    private void PlayerDash_OnDashEnded()
    {
        StartInvulnerability(dashEndingInvincibilityDuration);
    }

    private void PlayerAttack_OnAttackStarted()
    {
        IsInvincible = true;
    }

    private void PlayerAttack_OnAttackEnded()
    {
        StartInvulnerability(dashEndingInvincibilityDuration);
    }

    private void PlayerAttack_OnKillEnemy()
    {
        StartInvulnerability(killInvincibilityDuration);
    }

    private void OnIsDeadChanged(bool _, bool newStatus)
    {
        isDead = newStatus;
        OnDie?.Invoke();
    }

    private IEnumerator GainTempInvincibility(float invincibilityDuration)
    {
        IsInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        IsInvincible = false;
        invincibilityCoroutine = null;
    }

    public void TakeDamage(IDamageable damager, int damage)
    {
        if (isOwned)
            CMD_TakeDamage();
    }

    public void Revive()
    {
        if (isOwned)
        {
            StartInvulnerability(reviveInvincibilityDuration);
            CMD_Revive();
        }
    }

    public Transform GetTransform() => transform;

    #region Mirror Functions

    [Command]
    private void CMD_TakeDamage()
    {
        isDead = true;
        PlayDeathParticles(transform.position);
        spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.0f);
    }

    [Command]
    private void CMD_Revive()
    {
        isDead = false;
    }

    #endregion

    // TODO: Move out of PlayerStats
    private void PlayDeathParticles(Vector3 attackPosition)
    {
        if (playerDeathParticlesPrefab == null)
            return;

        ParticleSystem deathParticles = Instantiate(playerDeathParticlesPrefab, transform.position, Quaternion.identity);

        Vector3 difference = attackPosition - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, -difference.x) * Mathf.Rad2Deg;
        deathParticles.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(rotationZ, 90.0f, 0));
    }

    public void TriggerStun(Vector2 impactPosition)
    {
        //FIXME IMPL STUN LOGIC FOR PLAYER
        throw new System.NotImplementedException();
    }
}
