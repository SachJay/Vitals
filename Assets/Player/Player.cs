using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField]
    Camera camera;

    [SerializeField]
    float maxSpeed = 1;

    [SerializeField]
    float accel = 1;

    [SerializeField]
    float dashCount = 2;
    public float currentDashCount;

    [SerializeField]
    float dashCooldown = 3f;

    [SerializeField]
    float attackCount = 1;
    public float currentAttackCount = 1;

    [SerializeField]
    float attackCooldown = 3f;

    [SerializeField]
    float dashSpeed = 1500;

    [SerializeField]
    float dashDuration = 0.2f;

    [SerializeField]
    float attackSpeed = 900;

    [SerializeField]
    float attackDuration = 0.3f;

    [SerializeField]
    float drag = 7f;

    [SerializeField]
    float dashDrag = 10f;

    [SerializeField]
    bool isAlive = true;

    public float knockbackForce = 400f;

    public string nextScene = "";

    public ParticleSystem enemyDeathParticlesPrefab; //TODO move out of player

    public ParticleSystem playerDeathParticlesPrefab;

    public GameObject visuals;

    Vector2 currentVelocity = Vector2.zero;
    public bool isDashing = false;
    public bool isAttacking = false;
    public bool isInv = false;

    [SerializeField]
    float frictionDeacceleration = 70;

    Vector2 dashDestination = Vector2.zero;
    Vector3 cameraPosition = new Vector3(0,0,-10);

    Coroutine attackReset = null;
    Coroutine dashReset = null;

    [SerializeField]
    CircleCollider2D attackHitbox;

    private void Start()
    {
        currentDashCount = dashCount;
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

    void handleTimers()
    {
        if (attackReset == null && currentAttackCount < attackCount)
        {
            attackReset = StartCoroutine(startAttackTimer());
        }

        if (dashReset == null && currentDashCount < dashCount)
        {
            dashReset = StartCoroutine(startDashTimer());
        }
    }

    void limitDash()
    {
        if (Vector2.Distance(rb.transform.position, dashDestination) < 1)
        {
            rb.drag = dashDrag;
        }
    }

    void handleDashes()
    {
        if (Input.GetMouseButtonDown(1) && currentDashCount > 0)
        {
            currentDashCount--;
            StartCoroutine(Dash());
            
        }
    }

    void handleAttack()
    {
        if (Input.GetMouseButtonDown(0) && currentAttackCount > 0)
        {
            currentAttackCount--;
            StartCoroutine(Attack());
            
        }
    }

    IEnumerator startDashTimer()
    {
        yield return new WaitForSeconds(dashCooldown);

        currentDashCount++;
        if (currentDashCount > dashCount)
        {
            currentDashCount = dashCount;
        }
        dashReset = null;
    }

    IEnumerator startAttackTimer()
    {
        attackHitbox.enabled = true;
        yield return new WaitForSeconds(attackCooldown);

        currentAttackCount++;
        if (currentAttackCount > attackCount)
        {
            currentAttackCount = attackCount;
        }
        attackReset = null;

        attackHitbox.enabled = false;
    }

    IEnumerator startInvPeriod()
    {
        yield return new WaitForSeconds(attackCooldown);
        isInv = false;
    }

    // Satch Code
    //IEnumerator Dash()
    //{
    //    isDashing = true;
    //    isInv = true;
    //    Vector2 dashLocation = getDashLocation();
    //    rb.AddForce(dashLocation * dashSpeed, ForceMode2D.Force);

    //    yield return new WaitForSeconds(dashDuration);

    //    isDashing = false;
    //    isInv = false;
    //}

    // Habib Code
    IEnumerator Dash() // turn this into a loop so it can lerp from one location the next
    {
        isDashing = true;
        isInv = true;
        Vector2 dashLocation = getDashLocation();
        //rb.AddForce(dashLocation * dashSpeed, ForceMode2D.Force);


        // Take this out and keep this as a loop
        //yield return new WaitForSeconds(dashDuration);

        //isDashing = false;
        //isInv = false;
    }

    IEnumerator Attack()
    {
        isDashing = false;
        isAttacking = true;
        isInv = true;

        Vector2 dashLocation = getDashLocation();
        rb.AddForce(dashLocation * attackSpeed, ForceMode2D.Force);

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
        isInv = false;
    }

    private Vector2 getDashLocation()
    {
        rb.velocity = Vector2.zero;

        dashDestination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (dashDestination - rb.position).normalized;
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

    public void ResetDashes()
    {
        currentDashCount = dashCount;
        currentAttackCount = attackCount;
        isAttacking = false;
        isDashing = false;

        if (attackReset != null)
        {
            StopCoroutine(attackReset);
        }

        if (dashReset != null)
        {
            StopCoroutine(dashReset);
        }

        StartCoroutine(startInvPeriod());

        attackReset = null;
        dashReset = null;
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
