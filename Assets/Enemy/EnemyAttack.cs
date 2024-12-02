using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{

    [SerializeField]
    public Projectile projectilePrefab1;

    [SerializeField]
    float projectileSpeed = 5f;

    public abstract IEnumerator ExecuteAttack(Player player);

    public Projectile SpawnProjectile(Vector2 direction, Projectile projectilePrefab, float spawnOffset)
    {
        Vector3 offset = new Vector2(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset));
        Projectile projectile = Instantiate(projectilePrefab, transform.position + offset, Quaternion.identity);
        projectile.speed = projectileSpeed;
        projectile.SetDirection(direction);

        return projectile;
    }

    public Projectile SpawnProjectile(float directionAngle, Projectile projectilePrefab, float spawnOffset)
    {
        Vector2 projectileDirection = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));

        return SpawnProjectile(projectileDirection, projectilePrefab, spawnOffset);
    }
}
