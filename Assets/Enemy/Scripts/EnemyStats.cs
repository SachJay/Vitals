using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem enemyDeathParticlesPrefab;
    [SerializeField] private Enemy enemy;

    public Transform GetTransform() => transform;

    public void TakeDamage(IDamageable damager, int damage)
    {
        if (damager == null)
            HandleDeathParticles(transform);
        else
            HandleDeathParticles(damager.GetTransform());

        Destroy(gameObject);
    }

    public void TriggerStun(Vector2 impactPosition)
    {
        enemy.StunEnemy(impactPosition);
    }

    private void HandleDeathParticles(Transform damager)
    {
        if (enemyDeathParticlesPrefab == null)
            return;

        ParticleSystem deathParticles = Instantiate(enemyDeathParticlesPrefab, damager.position, Quaternion.identity);

        Vector3 difference = damager.position - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, -difference.x) * Mathf.Rad2Deg;
        deathParticles.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(rotationZ, 90.0f, 0));
    }
}
