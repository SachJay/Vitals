using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFire : EnemyAttack
{
    [SerializeField]
    int projectileNumbers = 3;

    [SerializeField]
    float projectileSpread = 0.1f;

    [SerializeField]
    float endingDelay = 1f;

    [SerializeField]
    float projectileDelay = 0.1f;

    [SerializeField]
    float waveDelay = 0f;

    [SerializeField]
    float numberOfWaves = 5f;

    [SerializeField]
    float maxOffset = 0.05f;

    [SerializeField]
    float minOffset = -0.05f;

    [SerializeField]
    float predictiveAmount = 0f;

    [SerializeField]
    float spawnOffset = 0f;

    public override IEnumerator ExecuteAttack(Player player)
    {
        for (int j = 0; j < numberOfWaves; j++) 
        {
            for (int i = -projectileNumbers; i <= projectileNumbers; i++)
            {
                Vector3 targetDir = player.transform.position - transform.position + player.GetVelocity() * predictiveAmount;
                float angle = Mathf.Atan2(targetDir.y, targetDir.x) + Random.Range(minOffset, maxOffset);

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
