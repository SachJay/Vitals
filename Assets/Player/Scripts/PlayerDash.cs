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
    [SerializeField] private AbilityTimer[] abilityTimers;

    private Vector2 dashDestination = Vector2.zero;
    private float currentDashCount;
    private float elapsedTime = 0;

    private void Start()
    {
        player.PlayerInputHandler.OnDashInputStarted += PlayerInput_OnDashStarted;

        player.PlayerAttack.OnAttackStarted += PlayerAttack_OnAttackStarted;
        player.PlayerAttack.OnEnemyKilled += PlayerAttack_OnEnemyKilled;

        SetTrailRenderer(false);
        InitDash();
    }

    private void FixedUpdate()
    {
        if (!player.IsOwned)
            return;

        if (!IsDashing || player.PlayerAttack.IsAttacking)
            return;

        Dash();
    }

    public void AddDash()
    {
        currentDashCount++;
    }

    private void PlayerInput_OnDashStarted()
    {
        if (!CanDash()) 
            return;

        currentDashCount--;

        IsDashing = true;
        OnDashStarted?.Invoke();

        elapsedTime = 0;

        GetDashLocation();
        SetTrailRenderer(true);

        AbilityTimer abilityTimer = GetFirstAvailableAbilityTimer();
        if (abilityTimer != null)
            abilityTimer.StartTimer(dashCooldown);
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
            abilityTimers[i].OnTimerTimeout?.Invoke(dashCooldown);
    }

    private void Dash()
    {
        elapsedTime += Time.fixedDeltaTime;

        float percentComplete = elapsedTime / dashDuration;

        player.transform.position = Vector2.Lerp(player.transform.position, dashDestination, percentComplete);

        if (Vector2.Distance(player.transform.position, dashDestination) < 0.2f)
        {
            player.transform.position = dashDestination;
            EndDash();
        }
    }

    private void GetDashLocation()
    {
        dashDestination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance((Vector2)player.transform.position, dashDestination) > maxDashDistance)
        {
            Vector2 maxDashDistVec = (dashDestination - (Vector2)player.transform.position).normalized;

            dashDestination = (Vector2)player.transform.position + maxDashDistVec * maxDashDistance;
        }
    }

    private bool CanDash()
    {
        if (!player.PlayerStats.IsAlive || currentDashCount <= 0 || player.PlayerAttack.IsAttacking || IsDashing)
            return false;
        return true;
    }

    private void EndDash()
    {
        OnDashEnded?.Invoke();
        IsDashing = false;
        SetTrailRenderer(false);
    }

    private void InitDash()
    {
        currentDashCount = maxDashCount;
        for (int index = 0; index < abilityTimers.Length; index++)
        {
            AbilityTimer abilityTimer = abilityTimers[index];
            abilityTimer.gameObject.SetActive(index < maxDashCount);
            abilityTimer.OnTimerTimeout += AbilityTimer_OnTimerTimeout;
        }
    }

    private void AbilityTimer_OnTimerTimeout(float _)
    {
        currentDashCount++;
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
