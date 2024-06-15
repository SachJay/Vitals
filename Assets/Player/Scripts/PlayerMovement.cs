using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool IsMoving { get; private set; }

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody2D rb;

    [Header("Movement Configurations")]
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float accel = 1f;
    [SerializeField] private float stopDrag = 7f;
    [SerializeField] private float movingDrag = 1f;

    Vector2 currentVelocity = Vector2.zero;

    private void Start()
    {
        currentVelocity = player.PlayerInputHandler.Move;
        IsMoving = currentVelocity != Vector2.zero;
    }

    void Update()
    {
        if (IsMoving)
            rb.drag = movingDrag;
        else
            rb.drag = stopDrag;

        if (!player.PlayerDash.IsDashing && !player.PlayerAttack.IsAttacking)
        {
            LimitSpeed();
            HandleMovement();
        }
    }

    public Vector3 GetVelocity() => rb.velocity;

    private void LimitSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void HandleMovement()
    {
        rb.AddForce(accel * Time.deltaTime * currentVelocity, ForceMode2D.Force);
    }
}
