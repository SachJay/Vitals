using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region Variables

    #region Player State Variables

    [SerializeField]
    bool isAlive = true;

    bool isInv = false;
    public bool IsInv => isInv;

    #endregion

    #region Speed/RB Variables

    [Header("Speed/RB Variables")]

    [SerializeField]
    float maxSpeed = 1;

    [SerializeField]
    float accel = 1;

    [SerializeField]
    float drag = 7f;

    [SerializeField]
    float dashDrag = 10f;

    Vector2 currentVelocity = Vector2.zero;

    public float knockbackForce = 400f;

    [SerializeField]
    float frictionDeacceleration = 70;

    #endregion

    #region Attack Variables

    [Header("Attack Variables")]

    [SerializeField]
    float maxAttackCount = 1;
    
    float currentAttackCount = 1;

    [SerializeField]
    float attackCooldown = 3f;

    [SerializeField]
    float maxAttackDistance = 7;

    [SerializeField]
    float attackDuration = 0.5f;

    bool isAttacking = false;
    public bool IsAttacking => isAttacking;

    Vector2 attackDestination = Vector2.zero;

    [SerializeField]
    CircleCollider2D attackHitbox;

    [SerializeField]
    AttackIndicator[] attackIndicators;

    #endregion

    #region Dash Variables

    [Header("Dash Variables")]

    [SerializeField]
    float maxDashCount = 2;

    float currentDashCount;

    [SerializeField]
    float dashCooldown = 3f;

    [SerializeField]
    float maxDashDistance = 15;

    [SerializeField]
    float dashDuration = 0.5f;

    bool isDashing = false;

    Vector2 dashDestination = Vector2.zero;

    [SerializeField]
    DashIndicator[] dashIndicators;

    #endregion

    #region Attack & Dash Shared Variables

    [Header("Attack & Dash Shared Variables")]

    [SerializeField]
    TrailRenderer trailRenderer;

    float elapsedDashTime = 0;

    #endregion

    [Header("Other Stuff")]

    public Rigidbody2D rb;

    [SerializeField]
    Camera camera;

    [SerializeField]
    PlayerControls playerControls;

    public string nextScene = "";

    public ParticleSystem enemyDeathParticlesPrefab; //TODO move out of player

    public ParticleSystem playerDeathParticlesPrefab;

    public GameObject visuals;
    Vector3 cameraPosition = new Vector3(0,0,-10);

    #endregion

    #region Start and Update Functions

    private void Start()
    {
        UpdateAttackAndDashCounts();
        SetTrailRenderer(false);
    }

    private void UpdateAttackAndDashCounts()
    {
        // Attacks
        currentAttackCount = maxAttackCount;

        foreach (AttackIndicator ind in attackIndicators)
        {
            ind.gameObject.SetActive(false);
            ind.player = this;
        }

        for (int i = 0; i < maxAttackCount; i++)
        {
            attackIndicators[i].gameObject.SetActive(true);
        }

        // Dashes
        currentDashCount = maxDashCount;

        foreach (DashIndicator ind in dashIndicators)
        {
            ind.gameObject.SetActive(false);
            ind.player = this;
        }

        for (int i = 0; i < maxDashCount; i++)
        {
            dashIndicators[i].gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        #region Debug Stuff

#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.T))
        {
            LoadNextScene();
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            KnockbackPlayer();
        }

#endif

        #endregion

        if (Input.GetKeyDown(KeyCode.R))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        if (!isAlive)
            return;

        //if (!isAttacking)
        //{
        //    HandleDashes();
        //}

        if (!isDashing && !isAttacking)
        {
            LimitSpeed();
            HandleMovement();
        }

        //HandleAttacks();

        cameraPosition.x = 0;
        cameraPosition.y = rb.transform.position.y;
        cameraPosition.z = -10;
    }

    private void FixedUpdate()
    {
        if (isAttacking)
            Attack();
        else if (isDashing)
            Dash();
    }

    #endregion

    #region Attack Functions

    private void OnAttack()
    {
        if (!CanAttack()) return;

        currentAttackCount--;

        isAttacking = true;
        isInv = true;

        elapsedDashTime = 0;

        GetAttackLocation();

        attackHitbox.enabled = true;
        SetTrailRenderer(true);

        foreach (AttackIndicator ind in attackIndicators)
        {
            if (ind.IsStarted)
                continue;

            else
                ind.StartTimer(attackCooldown);

            return;
        }
    }

    private bool CanAttack()
    {
        if (!isAlive) return false;
        else if (currentAttackCount <= 0) return false;
        else if (IsAttacking || isDashing) return false;
        
        else return true;
    }

    public void AddAttack()
    {
        currentAttackCount++;
    }

    private void GetAttackLocation()
    {
        attackDestination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(rb.position, attackDestination) > maxAttackDistance)
        {
            Vector2 maxAttackDistVec = (attackDestination - rb.position).normalized;

            attackDestination = rb.position + maxAttackDistVec * maxAttackDistance;
        }
    }

    private void Attack()
    {
        elapsedDashTime += Time.fixedDeltaTime;

        float percentComplete = elapsedDashTime / attackDuration;

        rb.position = Vector2.Lerp(rb.position, attackDestination, percentComplete);

        if (Vector2.Distance(rb.position, attackDestination) < 0.2f)
        {
            attackHitbox.enabled = false;
            SetTrailRenderer(false);
            
            rb.position = attackDestination;
            
            isAttacking = false;
            isInv = false;
        }
    }

    #endregion

    #region Dash Functions

    void OnDash()
    {
        if (!CanDash()) return;

        currentDashCount--;

        isDashing = true;
        isInv = true;

        elapsedDashTime = 0;

        GetDashLocation();

        SetTrailRenderer(true);

        foreach (DashIndicator ind in dashIndicators)
        {
            if (ind.IsStarted)
                continue;
                
            else
                ind.StartTimer(dashCooldown);
                
            return;
        }
        
    }

    private bool CanDash()
    {
        if (!isAlive) return false;
        else if (currentDashCount <= 0) return false;
        else if (IsAttacking || isDashing) return false;

        else return true;
    }

    public void AddDash()
    {
        currentDashCount++;
    }

    private void GetDashLocation()
    {
        dashDestination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(rb.position, dashDestination) > maxDashDistance)
        {
            Vector2 maxDashDistVec = (dashDestination - rb.position).normalized;

            dashDestination = rb.position + maxDashDistVec * maxDashDistance;
        }
    }

    private void Dash()
    {
        elapsedDashTime += Time.fixedDeltaTime;

        float percentComplete = elapsedDashTime / dashDuration;

        rb.position = Vector2.Lerp(rb.position, dashDestination, percentComplete);

        if (Vector2.Distance(rb.position, dashDestination) < 0.2f) {
            SetTrailRenderer(false);

            rb.position = dashDestination;
            
            isDashing = false;
            isInv = false;
        }
    }

    #endregion

    #region Attack & Dash Shared Functions

    public void ResetDashes()
    {
        currentDashCount = maxDashCount;
        currentAttackCount = maxAttackCount;
        isAttacking = false;
        isDashing = false;
    }

    public void SetTrailRenderer(bool newState)
    {
        trailRenderer.emitting = newState;
    }

    #endregion

    #region Movement Functions

    private void OnMove(InputValue inputValue)
    {
        currentVelocity = inputValue.Get<Vector2>();
    }

    private void HandleMovement()
    {
        rb.AddForce(accel * Time.deltaTime * currentVelocity, ForceMode2D.Force);
    }

    void LimitSpeed()
    {
        rb.drag = drag;

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    #endregion

    public void KillPlayer(Vector3 attackPosition)
    {
        isAlive = false;
        PlayDeathParticles(attackPosition);
        visuals.SetActive(false);
    }

    #region Debug Functions

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void KnockbackPlayer()
    {
        rb.AddForce(Vector2.left * 100, ForceMode2D.Impulse);
    }

    #endregion

    public void PlayDeathParticles(Vector3 attackPosition)
    {
        ParticleSystem deathParticles = Instantiate(playerDeathParticlesPrefab, rb.transform.position, Quaternion.identity);

        Vector3 difference =  attackPosition - rb.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, -difference.x) * Mathf.Rad2Deg;
        deathParticles.transform.rotation = Quaternion.Euler(rotationZ, 90.0f, 0);
        deathParticles.transform.position = rb.transform.position;
    }
}
