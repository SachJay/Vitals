using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    public delegate void PlayerSpellEvent();
    public PlayerSpellEvent OnSpellStarted;
    public PlayerSpellEvent OnSpellEnded;

    public bool IsCasting { get; private set; }
    
    [Header("References")]
    [SerializeField] private Player player;

    [Header("Cast Variables")]
    [SerializeField] private float maxActionCount = 2;
    [SerializeField] private float actionCooldown = 3f;
    [SerializeField] private float maxActionDistance = 1;
    [SerializeField] private float maxSpawnRange = 10;
    [SerializeField] private float actionDuration = 0.5f;
    [SerializeField] private AbilityTimer[] abilityTimers;
    [SerializeField] private GameObject spellPrefab;

    private Vector2 castDestination = Vector2.zero;
    private float currentActionCount;
    private float elapsedTime = 0;

    [SerializeField] AudioSource spellSoundEffect;
    [SerializeField] ParticleSystem spellParticleEffects;

    [SerializeField] private GameObject visuals;

    private void Start()
    {
        player.PlayerInputHandler.OnSpellInputStarted += PlayerInput_OnSpellStarted;

        player.PlayerAttack.OnEnemyKilled += PlayerAttack_OnEnemyKilled;

        InitCast();
    }

    private void FixedUpdate()
    {
        if (!IsCasting || player.PlayerAttack.IsAttacking)
            return;

        CastSpell();
    }

    public void AddCast()
    {
        currentActionCount++;
    }

    private void PlayerInput_OnSpellStarted()
    {
        if (!CanCastSpell()) 
            return;

        currentActionCount--;

        IsCasting = true;
        OnSpellStarted?.Invoke();

        elapsedTime = 0;

        castDestination = GetCastLocation(maxActionDistance);
        CreateSpell();

        visuals.transform.right = castDestination - (Vector2)transform.position;
        spellSoundEffect.Play();
        spellParticleEffects.Play();

        AbilityTimer abilityTimer = GetFirstAvailableAbilityTimer();
        if (abilityTimer != null)
            abilityTimer.StartTimer(actionCooldown);
    }

    private void PlayerAttack_OnEnemyKilled()
    {
        EndCast();

        for (int i = 0; i < maxActionCount; i++)
            abilityTimers[i].OnTimerTimeout?.Invoke(0);
    }

    private void CastSpell()
    {
        elapsedTime += Time.fixedDeltaTime;

        float percentComplete = elapsedTime / actionDuration;

        player.transform.position = Vector2.Lerp(player.transform.position, castDestination, percentComplete);
        
        if (Vector2.Distance(player.transform.position, castDestination) < 0.2f)
        {
            player.transform.position = castDestination;
            EndCast();
        }
    }

    private void CreateSpell()
    {
        GameObject spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        Throwable spellThrow = spell.GetComponent<Throwable>();
        spellThrow.Throw(castDestination - (Vector2) transform.position);
    }

    private Vector2 GetCastLocation(float maxRange)
    {
        Vector2 castLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance((Vector2)player.transform.position, castLocation) > maxRange)
        {
            Vector2 maxCastDistVec = (castLocation - (Vector2)player.transform.position).normalized;

            castLocation = (Vector2)player.transform.position + maxCastDistVec * maxRange;
        }

        return castLocation;
    }

    private bool CanCastSpell()
    {
        if (player.PlayerStats.IsDead || currentActionCount <= 0 || player.PlayerAttack.IsAttacking)
            return false;
        return true;
    }

    private void EndCast()
    {
        OnSpellEnded?.Invoke();
        IsCasting = false;
    }

    private void InitCast()
    {
        currentActionCount = maxActionCount;
        for (int index = 0; index < abilityTimers.Length; index++)
        {
            AbilityTimer abilityTimer = abilityTimers[index];
            abilityTimer.gameObject.SetActive(index < maxActionCount);
            abilityTimer.OnTimerTimeout += AbilityTimer_OnTimerTimeout;
        }
    }

    private void AbilityTimer_OnTimerTimeout(float _)
    {
        currentActionCount++;
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
}
