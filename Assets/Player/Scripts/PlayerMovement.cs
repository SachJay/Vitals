using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool IsMoving { get; private set; }

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject rotatable;

    [Header("Movement Configurations")]
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float accel = 1f;
    [SerializeField] private float stopDrag = 7f;
    [SerializeField] private float movingDrag = 1f;

    Vector2 currentVelocity = Vector2.zero;

    private void Update()
    {
        if (player.PlayerStats.IsDead)
            return;

        currentVelocity = player.PlayerInputHandler.Move;
        IsMoving = currentVelocity != Vector2.zero;

        rb.drag = IsMoving ? movingDrag : stopDrag;
        if (!player.PlayerDash.IsDashing && !player.PlayerAttack.IsAttacking)
        {
            HandleMovement();
            HandleRotation();
            LimitSpeed();
        }
    }

    public Vector3 GetVelocity() => rb.velocity;

    private void HandleMovement()
    {
        rb.AddForce(accel * Time.deltaTime * currentVelocity, ForceMode2D.Force);
    }

    private void HandleRotation()
    {
        float rotationValue = Mathf.Atan2(rb.velocity.normalized.y, rb.velocity.normalized.x) * Mathf.Rad2Deg;

        rotatable.transform.rotation = Quaternion.Euler(new Vector3(rotatable.transform.rotation.x, rotatable.transform.rotation.y, rotationValue));
    }

    private void LimitSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
