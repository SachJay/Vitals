using UnityEngine;

public class Throwable : MonoBehaviour, IInteractable
{
    public bool IsPickupable { get; private set; } = true;
    public bool IsThrown { get; private set; } = false;

    [SerializeField] private float force = 100.0f;
    [SerializeField] private int damage = 1;

    private readonly float delayCheck = 1.0f;
    private float timer = 0.0f;
    private Rigidbody2D rb;

    private void Awake()
    {
        if (!TryGetComponent(out rb))
            LogExtension.LogMissingComponent(name, nameof(Rigidbody2D));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsThrown)
            return;

        if (collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TriggerStun(transform.position);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!IsThrown)
            return;

        if (rb.velocity.magnitude < 0.25f && timer > delayCheck)
        {
            IsThrown = false;
            IsPickupable = true;
            rb.velocity = Vector2.zero;
        }

        timer += Time.deltaTime;
    }

    public void SetIsPickupable(bool isPickupable)
    {
        IsPickupable = isPickupable;
    }

    public void Throw(Vector2 direction)
    {
        timer = 0.0f;
        gameObject.SetActive(true);
        IsThrown = true;
        rb.AddForce(direction.normalized * force);
    }

    public void Interact()
    {
        if (IsPickupable)
            gameObject.SetActive(false);
    }

    public void EndInteraction() { }

    public GameObject GetGameObject() => gameObject;
}
