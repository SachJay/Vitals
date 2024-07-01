using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float duration = 5;
    [SerializeField] private bool contactDamage = true;
    [SerializeField] private float speed = 1;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!contactDamage)
            return;

        if (other.transform != null && other.transform.parent != null && other.transform.parent.parent != null && other.transform.parent.parent.gameObject.TryGetComponent(out Player player))
        {
            if ((!player.PlayerStats.IsInvincible) || CompareTag(UtilityExtension.UNDODGEABLE))
            {
                //Destroy(other.transform.parent.gameObject);
                // TODO: Add IDamageable and damage
                player.PlayerStats.TakeDamage(null, 1);
                Destroy(gameObject);
            }
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetDirection(Vector2 direction)
    {
        rb.AddForce(direction.normalized * speed);
    }
}
