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
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool contactDamage = true;

    private float timer = 0.0f;
    private float duration = 0.0f;

    private void OnEnable()
    {
        timer = 0.0f;
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!contactDamage)
            return;

        if (other.transform != null && other.transform.parent != null && other.transform.parent.parent != null && other.transform.parent.parent.gameObject.TryGetComponent(out Player player))
        {
            if ((!player.PlayerStats.IsInvincible) || CompareTag(ProjectileType.Undodgeable.ToString()))
            {
                // TODO: Add IDamageable and damage
                player.PlayerStats.TakeDamage(null, 1);
                ProjectilePool.Instance.ReturnProjectile(gameObject);
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
        
        rb.AddForce(direction.normalized * projectileSO.ProjectileSpeed);

        duration = projectileSO.ProjectileDuration;
        transform.localScale = projectileSO.ProjectileScale;

        spriteRenderer.color = projectileSO.ProjectileColor;
    }
}
