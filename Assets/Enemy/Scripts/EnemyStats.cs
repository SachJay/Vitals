using Mirror;
using UnityEngine;

public class EnemyStats : NetworkBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem enemyDeathParticlesPrefab;

    public Transform GetTransform() => transform;

    public void TakeDamage(IDamageable damager, int damage)
    {
        if (damager == null)
            CMD_TakeDamage(transform.position + Random.insideUnitSphere);
        else
            CMD_TakeDamage(damager.GetTransform().position);
    }

    [Command]
    private void CMD_TakeDamage(Vector2 damagerPosition)
    {
        HandleDeathParticles(damagerPosition);

        Destroy(gameObject);
    }

    private void HandleDeathParticles(Vector2 damagerPosition)
    {
        if (enemyDeathParticlesPrefab == null)
            return;

        ParticleSystem deathParticles = Instantiate(enemyDeathParticlesPrefab, damagerPosition, Quaternion.identity);

        Vector3 difference = (Vector3)damagerPosition - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, -difference.x) * Mathf.Rad2Deg;
        deathParticles.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(rotationZ, 90.0f, 0));
    }
}
