using UnityEngine;

public class ProjectileStats : MonoBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem enemyDeathParticlesPrefab;

    public Transform GetTransform() => transform;

    public void TakeDamage(IDamageable damager, int damage)
    {
        if (damager == null)
        {
            HandleDamage(transform.position);
        }
        else
        {
            HandleDamage(damager.GetTransform().position);
        }
    }

    public void TriggerStun(Vector2 impactPosition)
    {
        HandleDamage(impactPosition);
    }

    public bool IsAttackResetable()
    {
        return true;
    }

    private void HandleDamage(Vector2 position)
    {
        HandleDeathParticles(position);

        Destroy(gameObject);
    }

    private void HandleDeathParticles(Vector2 position)
    {
        if (enemyDeathParticlesPrefab == null)
            return;

        ParticleSystem deathParticles = Instantiate(enemyDeathParticlesPrefab, position, Quaternion.identity);

        Vector3 difference = position - (Vector2) transform.position;
        float rotationZ = Mathf.Atan2(difference.y, -difference.x) * Mathf.Rad2Deg;
        deathParticles.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(rotationZ, 90.0f, 0));
    }
}
