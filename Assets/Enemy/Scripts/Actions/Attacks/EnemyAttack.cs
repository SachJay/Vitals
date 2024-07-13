using System.Collections;
using UnityEngine;

public abstract class EnemyAttack : EnemyAction
{
    [SerializeField, Tooltip("Type of projectile fired")] protected Projectile projectilePrefab1;
    [SerializeField, Tooltip("Speed of projectile")] private float projectileSpeed = 5f;
    [SerializeField, Tooltip("Speed of projectile")] private Vector3 projectileScale = new Vector3(1, 1, 1);
    [SerializeField, Tooltip("The number of seconds projectiles last")] private float projectileDuration = 10f;

    public Projectile SpawnProjectile(Vector2 direction, Projectile projectilePrefab, float spawnOffset)
    {
        Vector3 offset = new Vector2(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset));
        Projectile projectile = Instantiate(projectilePrefab, transform.position + offset, Quaternion.identity);
        projectile.SetSpeed(projectileSpeed);
        projectile.SetDuration(projectileDuration);
        projectile.SetDirection(direction);
        projectile.SetScale(projectileScale);

        return projectile;
    }

    public Projectile SpawnProjectile(float directionAngle, Projectile projectilePrefab, float spawnOffset)
    {
        Vector2 projectileDirection = new(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));

        return SpawnProjectile(projectileDirection, projectilePrefab, spawnOffset);
    }
}
