using Mirror;
using UnityEngine;

public enum ProjectileType
{
    Base,
    Undodgeable,
    Attackable
}

public class Projectile : NetworkBehaviour
{
    public ProjectileType ProjectileType { get; private set; }

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool contactDamage = true;
    [SerializeField] private float speed = 1;

    private float timer = 0.0f;
    private float duration = 0.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!contactDamage)
            return;

        if (other.transform != null && other.transform.parent != null && other.transform.parent.parent != null && other.transform.parent.parent.gameObject.TryGetComponent(out Player player))
        {
            if ((!player.PlayerStats.IsInvincible) || CompareTag(ProjectileType.ToString()))
            {
                // TODO: Add IDamageable and damage
                player.PlayerStats.TakeDamage(null, 1);
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= duration)
            ProjectilePool.Instance.ReturnProjectile(gameObject);
    }

    public void Init(ProjectileScriptableObject projectileSO, Vector2 direction)
    {
        ProjectileType = projectileSO.ProjectileType;
        contactDamage = projectileSO.IsContactDamage;
        speed = projectileSO.ProjectileSpeed;
        transform.localScale = projectileSO.ProjectileScale;
        rb.AddForce(direction.normalized * speed);
        duration = projectileSO.ProjectileDuration;
    }

    public void SetDirection(Vector2 direction)
    {
        rb.AddForce(direction.normalized * speed);
    }
}
