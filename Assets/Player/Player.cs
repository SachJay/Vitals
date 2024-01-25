using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region Variables

    public Rigidbody2D rb;

    [SerializeField]
    Camera camera;

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

    #endregion

    #region Attack Variables

    [Header("Attack Variables")]

    [SerializeField]
    float attackCount = 1;
    public float currentAttackCount = 1;

    [SerializeField]
    float attackCooldown = 3f;

    [SerializeField]
    float attackSpeed = 900;

    [SerializeField]
    float attackDuration = 0.3f;

    public bool isAttacking = false;

    [SerializeField]
    CircleCollider2D attackHitbox;

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

    float elapsedDashTime = 0;

    public bool isDashing = false;

    Vector2 dashDestination = Vector2.zero;



    #endregion

    [Header("Other Stuff")]

    public float knockbackForce = 400f;

    public string nextScene = "";

    public ParticleSystem enemyDeathParticlesPrefab; //TODO move out of player

    public ParticleSystem playerDeathParticlesPrefab;

    public GameObject visuals;

    [SerializeField]
    float frictionDeacceleration = 70;
    Vector3 cameraPosition = new Vector3(0,0,-10);

    #endregion

    #region Start and Update Functions

    private void Start()
    {
        currentDashCount = maxDashCount;
        currentAttackCount = attackCount;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
        {
            LoadNextScene();
        }
#endif 

        if (!isAlive)
            return;

        if (!isAttacking)
        {
            handleDashes();
        }

        if (!isDashing && !isAttacking)
        {
            limitSpeed();
            handleMovement();
        }

        if (isDashing)
        {
            limitDash();
        }

        handleAttack();
        handleTimers();

        cameraPosition.x = 0;
        cameraPosition.y = rb.transform.position.y;
        cameraPosition.z = -10;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            Dash();
        }
    }

    #endregion

    void handleTimers()
    {

    }

    void limitDash()
    {
        if (Vector2.Distance(rb.transform.position, dashDestination) < 1)
        {
            rb.drag = dashDrag;
        }
    }

    #region Attack Functions

    void handleAttack()
    {
        if (Input.GetMouseButtonDown(0) && currentAttackCount > 0)
        {
            currentAttackCount--;

            isAttacking = true;
            isInv = true;

            elapsedDashTime = 0;

            setDashLocation();
        }
    }

    IEnumerator Attack()
    {
        isDashing = false;
        isAttacking = true;
        isInv = true;

        //Vector2 dashLocation = getDashLocation();
        rb.AddForce(dashDestination * attackSpeed, ForceMode2D.Force);

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
        isInv = false;
    }

    #endregion

    #region Dash Functions

    void handleDashes()
    {
        if (Input.GetMouseButtonDown(1) && currentDashCount > 0)
        {
            currentDashCount--;

            isDashing = true;
            isInv = true;

            elapsedDashTime = 0;

            setDashLocation();
        }
    }


    private void setDashLocation()
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
            rb.position = dashDestination;
            isDashing = false;
            isInv = false;
        }
    }

    #endregion

    public void ResetDashes()
    {
        currentDashCount = maxDashCount;
        currentAttackCount = attackCount;
        isAttacking = false;
        isDashing = false;
    }

    void handleMovement()
    {
        Vector2 accelDirection = new Vector2(
            (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0),
            (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0));

        rb.AddForce(accelDirection * accel * Time.deltaTime, ForceMode2D.Force);
    }

    void limitSpeed ()
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

    public void KillPlayer(Vector3 attackPosition)
    {
        isAlive = false;
        PlayDeathParticles(attackPosition);
        visuals.SetActive(false);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void PlayDeathParticles(Vector3 attackPosition)
    {
        ParticleSystem deathParticles = Instantiate(playerDeathParticlesPrefab, rb.transform.position, Quaternion.identity);

        Vector3 difference =  attackPosition - rb.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, -difference.x) * Mathf.Rad2Deg;
        deathParticles.transform.rotation = Quaternion.Euler(rotationZ, 90.0f, 0);
        deathParticles.transform.position = rb.transform.position;
    }
}
