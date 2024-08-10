using UnityEngine;

public abstract class EnemyAttack : EnemyAction
{
    [Header("Base Enemy Attack Configurations")]
    [SerializeField] protected ProjectileScriptableObject projectileSO;

    public Projectile SpawnProjectile(Vector2 direction, float spawnOffset)
    {
        Vector3 offset = new Vector2(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset));

        if (!ProjectilePool.Instance.GetProjectile(transform.position + offset, Quaternion.identity).TryGetComponent(out Projectile projectile))
        {
            LogExtension.LogMissingComponent(name, nameof(Projectile));
            return null;
        }

        projectile.Init(projectileSO, direction);

        return projectile;
    }

    public Projectile SpawnProjectile(float directionAngle, float spawnOffset)
    {
        Vector2 projectileDirection = new(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));

        return SpawnProjectile(projectileDirection, spawnOffset);
    }
}
