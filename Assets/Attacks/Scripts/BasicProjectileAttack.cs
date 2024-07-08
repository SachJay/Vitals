using System.Collections;
using UnityEngine;

public class ShotFire : EnemyAttack
{
    [SerializeField, Tooltip("Number Of Projectiles Per Wave")] private int projectileAmount = 3;
    [SerializeField, Tooltip("How far apart projectiles are from each other")] private float projectileSpread = 0.1f;
    [SerializeField, Tooltip("Delay of entire attack. Delay after all waves")] private float endingDelay = 1f;
    [SerializeField, Tooltip("Delay between each projectile")] private float projectileDelay = 0.1f;
    [SerializeField, Tooltip("Delay between next wave")] private float waveDelay = 0f;
    [SerializeField, Tooltip("Number of waves. 1 wave shoots the number of projectiles defined")] private float numberOfWaves = 5f;
    [SerializeField, Tooltip("Upper value for the random direction a projectile will fire")] private float maxRandomDir = 0.05f;
    [SerializeField, Tooltip("Lower value for the random direction a projectile will fire")] private float minRandonDir = -0.05f;
    [SerializeField, Tooltip("Amount the projectile is fired infront of player")] private float predictiveAmount = 0f;
    [SerializeField, Tooltip("Random distance projectiles could spawn originating from enemy")] private float spawnOffset = 0f;

    public override IEnumerator ExecuteAttack(Player player)
    {
        for (int j = 0; j < numberOfWaves; j++) 
        {
            for (int i = -projectileAmount; i <= projectileAmount; i++)
            {
                Vector3 targetDir = player.transform.position - transform.position + player.PlayerMovement.GetVelocity() * predictiveAmount;
                float angle = Mathf.Atan2(targetDir.y, targetDir.x) + Random.Range(minRandonDir, maxRandomDir);

                SpawnProjectile(angle + i * projectileSpread, projectilePrefab1, spawnOffset);

                if (projectileDelay != 0)
                    yield return new WaitForSeconds(projectileDelay);
            }

            if (waveDelay != 0)
                yield return new WaitForSeconds(waveDelay);
        }

        if (endingDelay != 0)
            yield return new WaitForSeconds(endingDelay);
    }
}
