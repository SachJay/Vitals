using System;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Variables

    #region Player State Variables

    //[SerializeField]
    bool isAlive = true;

    bool isInv = false;
    public bool IsInv => isInv;

    [SerializeField]
    float killInvDuration = 0.2f;

    #endregion

    #region Speed/RB Variables

    [Header("Speed/RB Variables")]

    [SerializeField]
    float maxSpeed = 1f;

    [SerializeField]
    float currentSpeed = 0f;

    [SerializeField]
    float accel = 1f;

    [SerializeField]
    float stopDrag = 7f;

    [SerializeField]
    float movingDrag = 1f;

    Vector2 currentVelocity = Vector2.zero;

    public float knockbackForce = 400f;

    [SerializeField]
    float frictionDeacceleration = 70;

    bool isMoving = false;

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

    //[SerializeField]
    //Scenes nextScene;

    public ParticleSystem enemyDeathParticlesPrefab; //TODO move out of player

    public ParticleSystem playerDeathParticlesPrefab;

    public GameObject visuals;
    Vector3 cameraPosition = new Vector3(0,0,-10);

    #endregion

    #region Start and Update Functions

    private void Start()
    {
        ErrorCheck();

        UpdateAttackAndDashCounts();
        SetTrailRenderer(false);
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

        if (!isAlive)
            return;

        if (isMoving)
            rb.drag = movingDrag;
        else
            rb.drag = stopDrag;
        
        if (!isDashing && !isAttacking)
        {
            LimitSpeed();
            HandleMovement();
        }
    }

    private void FixedUpdate()
    {
        if (isAttacking)
            Attack();
        else if (isDashing)
            Dash();
    }

    private void LateUpdate()
    {
        cameraPosition.x = 0;
        cameraPosition.y = rb.transform.position.y;
        cameraPosition.z = -10;
    }

    #endregion

    #region Attack Functions

    private void OnAttack()
    {
        if (!CanAttack()) return;

        if(isDashing)
            EndDash();

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
        else if (IsAttacking) return false;
        
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
            rb.position = attackDestination;

            EndAttack();
        }
    }

    private void EndAttack()
    {
        attackHitbox.enabled = false;
        SetTrailRenderer(false);

        isAttacking = false;
        isInv = false;
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

        if (Vector2.Distance(rb.position, dashDestination) < 0.2f)
        {
            rb.position = dashDestination;

            EndDash();
        }
    }

    private void EndDash()
    {
        SetTrailRenderer(false);

        isDashing = false;
        isInv = false;
    }

    #endregion

    #region Attack & Dash Shared Functions

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

    public void ResetAttack_Dash()
    {
        EndAttack();

        for (int i = 0; i < maxAttackCount; i++)
        {
            attackIndicators[i].ResetTimer();
        }

        EndDash();        
        for (int i = 0; i < maxDashCount; i++)
        {
            dashIndicators[i].ResetTimer();
        }
    }

    public void SetTrailRenderer(bool newState)
    {
        trailRenderer.emitting = newState;
    }

    #endregion

    #region Other Movement Functions

    private void OnMove(InputValue inputValue)
    {
        currentVelocity = inputValue.Get<Vector2>();

        isMoving = currentVelocity != Vector2.zero;
    }

    private void HandleMovement()
    {
        rb.AddForce(accel * Time.deltaTime * currentVelocity, ForceMode2D.Force);
    }

    void LimitSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        currentSpeed = rb.velocity.magnitude;
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    #endregion

    public void LoadNextScene()
    {
        //SceneChanger.ChangeSceneTo(nextScene);
    }

    private void OnRestart()
    {
        // TODO: Check if stage is done and stop player from restarting

        //SceneChanger.ResetCurrentScene(SceneManager.GetActiveScene().name);
    }

    public void EnemyKilled()
    {
        ResetAttack_Dash();

        StartCoroutine(TempInvincibility());
    }

    IEnumerator TempInvincibility()
    {
        isInv = true;

        yield return new WaitForSeconds(killInvDuration);

        isInv = false;
    }

    public void KillPlayer(Vector3 attackPosition)
    {
        isAlive = false;
        PlayDeathParticles(attackPosition);
        visuals.SetActive(false);
    }

    #region Debug Functions

    private void ErrorCheck()
    {
        // This function is to let us know if some variables AREN'T set when the player is added to a scene
        
        if (camera == null) 
            Debug.LogException(new Exception(name + " is missing the \"camera\" Variable, Please set it in the inspector"));

        //if (nextScene == Scenes.NONE)
            //Debug.LogException(new Exception(name + " is missing the \"nextScene\" Variable, Please set it in the inspector"));
    }

    public void KnockbackPlayer()
    {
        EndAttack();
        EndDash();
        
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
