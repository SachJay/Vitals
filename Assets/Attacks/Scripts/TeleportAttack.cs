using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAttack : EnemyAttack
{
    [SerializeField]
    float endingDelay = 1f;

    [SerializeField]
    float waveDelay = 0f;

    [SerializeField]
    float waveSpread = 0.1f;

    [SerializeField]
    int numberOfWaves = 5;

    [SerializeField]
    int numberOfParticles = 4;

    [SerializeField]
    int maxDirectionChange = 5;

    [SerializeField]
    int minDirectionChange = 10;

    [SerializeField]
    int sprayGap = 1;

    [SerializeField]
    public Projectile projectileAttackablePrefab;

    [SerializeField]
    int maxWaveDistance = 16;

    int waveSpreadCount = 0;
    int count = 0;
    int waveMaxCount = 0;
    int direction = 1;

    [SerializeField]
    int horiOffset = -3;

    [SerializeField]
    float spawnOffset = 0f;

    public override IEnumerator ExecuteAttack(Player player)
    {
        waveSpreadCount = 0;

        for (int j = -maxWaveDistance * 2; j < maxWaveDistance * 2; j++)
        {
            if (Mathf.Abs(j) > sprayGap)
            {
                Projectile p1 = SpawnProjectile(Mathf.PI, projectilePrefab1, spawnOffset);
                p1.transform.position = p1.transform.position + new Vector3(horiOffset, j + waveSpreadCount + 1, 0);
            }
            
        }

        for (int j = 0; j < numberOfWaves; j++) {

            for(int i = -1; i <= 1; i+=2)
            {
                Projectile p1 = SpawnProjectile(Mathf.PI, projectilePrefab1, spawnOffset);
                p1.transform.position = p1.transform.position + new Vector3(horiOffset, i * sprayGap + waveSpreadCount, 0);

            }
            
            waveSpreadCount += direction;
            handleWaveDirection();

            yield return new WaitForSeconds(waveDelay);
        }

        yield return new WaitForSeconds(endingDelay);
    }

    private void handleWaveDirection()
    {
        count++;
        if (waveMaxCount < count || waveSpreadCount > maxWaveDistance || waveSpreadCount < -maxWaveDistance)
        {
            count = 0;
            direction = direction * -1;
            waveMaxCount = Random.Range(minDirectionChange, maxDirectionChange);

            if (direction == 1)
            {
                Projectile p3 = SpawnProjectile(Mathf.PI, projectileAttackablePrefab, spawnOffset);
                p3.transform.position = p3.transform.position + new Vector3(horiOffset, waveSpreadCount, 0);
            }
        }
    }
}
