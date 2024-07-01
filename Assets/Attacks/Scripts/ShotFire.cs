using System.Collections;
using UnityEngine;

public class ShotFire : EnemyAttack
{
    [SerializeField] private int projectileNumbers = 3;
    [SerializeField] private float projectileSpread = 0.1f;
    [SerializeField] private float endingDelay = 1f;
    [SerializeField] private float projectileDelay = 0.1f;
    [SerializeField] private float waveDelay = 0f;
    [SerializeField] private float numberOfWaves = 5f;
    [SerializeField] private float maxOffset = 0.05f;
    [SerializeField] private float minOffset = -0.05f;
    [SerializeField] private float predictiveAmount = 0f;
    [SerializeField] private float spawnOffset = 0f;

    public override IEnumerator ExecuteAttack(Player player)
    {
        for (int j = 0; j < numberOfWaves; j++) 
        {
            for (int i = -projectileNumbers; i <= projectileNumbers; i++)
            {
                Vector3 targetDir = player.transform.position - transform.position + player.PlayerMovement.GetVelocity() * predictiveAmount;
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
