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

    private void Update()
    {
        if (!player.IsOwned || player.PlayerStats.IsDead)
            return;

        currentVelocity = player.PlayerInputHandler.Move;
        IsMoving = currentVelocity != Vector2.zero;

        rb.drag = IsMoving ? movingDrag : stopDrag;
        if (!player.PlayerDash.IsDashing && !player.PlayerAttack.IsAttacking)
        {
            HandleMovement();
            LimitSpeed();
        }
    }

    public Vector3 GetVelocity() => rb.velocity;

    private void HandleMovement()
    {
        rb.AddForce(accel * Time.deltaTime * currentVelocity, ForceMode2D.Force);
    }

    private void LimitSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
