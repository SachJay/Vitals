using UnityEngine;

public class Throwable : MonoBehaviour, IInteractable
{
    public bool IsPickupable { get; private set; } = true;
    public bool IsThrown { get; private set; } = false;

    [SerializeField] private float force = 100.0f;
    [SerializeField] private float delayPickup = 1.0f;
    
    private Rigidbody2D rb;
    private float timer = -999f;

    private void Awake()
    {
        if (!TryGetComponent(out rb))
            LogExtension.LogMissingComponent(name, nameof(Rigidbody2D));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsThrown)
            Debug.Log(collision.name);
    }

    private void Update()
    {
        if (IsThrown)
        {
            timer += Time.deltaTime;

            if (timer > delayPickup)
                IsPickupable = true;

            if (rb.velocity.magnitude < 0.1f && IsPickupable)
            {
                IsThrown = false;
                rb.velocity = Vector2.zero;
            }
        }
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
