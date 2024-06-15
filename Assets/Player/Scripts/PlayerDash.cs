using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public delegate void PlayerDashEvent();
    public PlayerDashEvent OnDashStarted;
    public PlayerDashEvent OnDashEnded;

    public bool IsDashing { get; private set; }
    
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private TrailRenderer trailRenderer;

    [Header("Dash Variables")]
    [SerializeField] private float maxDashCount = 2;
    [SerializeField] private float dashCooldown = 3f;
    [SerializeField] private float maxDashDistance = 15;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private DashIndicator[] dashIndicators;

    private Vector2 dashDestination = Vector2.zero;
    private float currentDashCount;
    private float elapsedDashTime = 0;

    private void Start()
    {
        player.PlayerInputHandler.OnDashInputStarted += PlayerInput_OnDashStarted;

        player.PlayerAttack.OnAttackStarted += PlayerAttack_OnAttackStarted;
        player.PlayerAttack.OnEnemyKilled += PlayerAttack_OnEnemyKilled;

        SetTrailRenderer(false);
        UpdateDashCounts();
    }

    private void FixedUpdate()
    {
        if (!IsDashing)
            return;

        Dash();
    }

    public void AddDash()
    {
        currentDashCount++;
    }

    private void PlayerInput_OnDashStarted()
    {
        if (!CanDash()) return;

        currentDashCount--;

        IsDashing = true;
        OnDashStarted?.Invoke();

        elapsedDashTime = 0;

        GetDashLocation();

        SetTrailRenderer(true);

        foreach (DashIndicator dashIndicator in dashIndicators)
        {
            if (!dashIndicator.IsStarted)
            {
                dashIndicator.StartTimer(dashCooldown);
                break;
            }
        }
    }

    private void PlayerAttack_OnAttackStarted()
    {
        if (IsDashing)
            EndDash();
    }

    private void PlayerAttack_OnEnemyKilled()
    {
        EndDash();
        for (int i = 0; i < maxDashCount; i++)
        {
            dashIndicators[i].ResetTimer();
        }
    }

    private void Dash()
    {
        elapsedDashTime += Time.fixedDeltaTime;

        float percentComplete = elapsedDashTime / dashDuration;

        transform.position = Vector2.Lerp(transform.position, dashDestination, percentComplete);

        if (Vector2.Distance(transform.position, dashDestination) < 0.2f)
        {
            transform.position = dashDestination;
            EndDash();
        }
    }

    private void GetDashLocation()
    {
        dashDestination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance((Vector2)transform.position, dashDestination) > maxDashDistance)
        {
            Vector2 maxDashDistVec = (dashDestination - (Vector2)transform.position).normalized;

            dashDestination = (Vector2)transform.position + maxDashDistVec * maxDashDistance;
        }
    }

    private bool CanDash()
    {
        if (player.PlayerStats.IsAlive || currentDashCount <= 0 || player.PlayerAttack.IsAttacking || IsDashing)
            return false;
        return true;
    }

    private void EndDash()
    {
        IsDashing = false;
        player.PlayerStats.SetInvincibleStatus(false);
        SetTrailRenderer(false);
    }

    private void UpdateDashCounts()
    {
        // Dashes
        currentDashCount = maxDashCount;

        foreach (DashIndicator ind in dashIndicators)
        {
            ind.gameObject.SetActive(false);

            // TODO: Fix this to have a general timer and not be controlled by a UI
            ind.player = player;
        }

        for (int i = 0; i < maxDashCount; i++)
        {
            dashIndicators[i].gameObject.SetActive(true);
        }
    }

    // TODO: Move out of dashing to its own script
    public void SetTrailRenderer(bool newState)
    {
        trailRenderer.emitting = newState;
    }
}
