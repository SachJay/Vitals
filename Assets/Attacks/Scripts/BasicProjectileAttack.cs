using System.Collections;
using UnityEngine;

public class ShotFire : EnemyAttack
{
    [SerializeField, Tooltip("Number Of Projectiles Per Wave")] private int projectileAmount = 3;
    [SerializeField, Tooltip("The number of degrees each projectiles are from each other")] private float projectileSpread = 0.1f;
    [SerializeField, Tooltip("Delay of entire attack. Delay after all waves")] private float endingDelay = 1f;
    [SerializeField, Tooltip("Delay between each projectile")] private float projectileDelay = 0.1f;
    [SerializeField, Tooltip("Delay between next wave")] private float waveDelay = 0f;
    [SerializeField, Tooltip("Number of waves. 1 wave shoots the number of projectiles defined")] private float numberOfWaves = 5f;
    [SerializeField, Tooltip("Upper value for the random direction a projectile will fire in degrees")] private float maxRandomDir = 5f;
    [SerializeField, Tooltip("Lower value for the random direction a projectile will fire in degrees")] private float minRandonDir = -5f;
    [SerializeField, Tooltip("Amount the projectile is fired infront of player")] private float predictiveAmount = 0f;
    [SerializeField, Tooltip("Random distance projectiles could spawn originating from enemy")] private float spawnOffset = 0f;

    public override IEnumerator ExecuteAttack(Player player)
    {
        float projectileSpreadRad = projectileSpread * Mathf.Deg2Rad;

        for (int j = 0; j < numberOfWaves; j++) 
        {
            for (int i =  -getFloorValue((float)projectileAmount / 2); i <= Mathf.Max(0, getCeilValue((float)projectileAmount / 2) - 1); i++)
            {
                Vector3 targetDir = player.transform.position - transform.position + player.PlayerMovement.GetVelocity() * predictiveAmount;
                float angle = Mathf.Atan2(targetDir.y, targetDir.x) + Random.Range(minRandonDir * Mathf.Deg2Rad, maxRandomDir * Mathf.Deg2Rad);

                float evenProjectileAdjust = projectileAmount % 2 == 0 ? projectileSpreadRad / 2 : 0;
                SpawnProjectile(angle + i * projectileSpreadRad + evenProjectileAdjust, projectilePrefab1, spawnOffset);

                if (projectileDelay != 0)
                    yield return new WaitForSeconds(projectileDelay);
            }

            if (waveDelay != 0)
                yield return new WaitForSeconds(waveDelay);
        }

        if (endingDelay != 0)
            yield return new WaitForSeconds(endingDelay);
    }

    private int getFloorValue(float input)
    {
        return (int) Mathf.Floor(input);
    }

    private int getCeilValue(float input)
    {
        return (int)Mathf.Ceil(input);
    }
}
