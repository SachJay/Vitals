using Mirror;
using UnityEngine;

public class EnemyStats : NetworkBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem enemyDeathParticlesPrefab;
    [SerializeField] private Enemy enemy;

    public Transform GetTransform() => transform;

    public void TakeDamage(IDamageable damager, int damage)
    {
        if (damager == null)
            CmdTakeDamage(transform.position + Random.insideUnitSphere);
        else
            CmdTakeDamage(damager.GetTransform().position);
    }

    [Command]
    private void CmdTakeDamage(Vector2 damagerPosition)
    {
        RpcTakeDamage(damagerPosition);
    }

    [ClientRpc]
    private void RpcTakeDamage(Vector2 damagerPosition)
    {
        HandleDeathParticles(damagerPosition);
        Destroy(gameObject);
    }

    public void TriggerStun(Vector2 impactPosition)
    {
        enemy.StunEnemy(impactPosition);
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
