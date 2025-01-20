using UnityEngine;

public class Projectile : MonoBehaviour
{
<<<<<<< Updated upstream:Assets/Projectiles/Scripts/Projectile.cs
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool contactDamage = true;
    [SerializeField] private float speed = 1;
=======
    [SerializeField]
    Rigidbody2D rb;

    public float speed = 1;

    [SerializeField]
    float duration = 5;

    [SerializeField]
    bool contactDamage = true;

    [SerializeField]
    protected bool dashable = true;

    private void Start()
    {
        StartCoroutine("Die");
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }
>>>>>>> Stashed changes:Assets/Projectiles/Projectile.cs

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!contactDamage)
            return;

        if (other.transform != null && other.transform.parent != null && other.transform.parent.parent != null && other.transform.parent.parent.gameObject.TryGetComponent(out Player player))
        {
<<<<<<< Updated upstream:Assets/Projectiles/Scripts/Projectile.cs
            if ((!player.PlayerStats.IsInvincible) || CompareTag(UtilityExtension.UNDODGEABLE))
=======
            if ((!player.PlayerStats.IsInvincible) || !dashable)
>>>>>>> Stashed changes:Assets/Projectiles/Projectile.cs
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

    public void SetDuration(float duration)
    {
        Destroy(gameObject, duration);
    }

    public void SetDirection(Vector2 direction)
    {
        rb.AddForce(direction.normalized * speed);
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
}
