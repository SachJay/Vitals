using System.Collections;
using UnityEngine;

public class TeleportAttack : EnemyAttack
{
    [SerializeField] private float endingDelay = 1f;
    [SerializeField] private float waveDelay = 0f;
    [SerializeField] private int numberOfWaves = 5;
    [SerializeField] private int maxDirectionChange = 5;
    [SerializeField] private int minDirectionChange = 10;
    [SerializeField] private int sprayGap = 1;
    [SerializeField] private Projectile projectileAttackablePrefab;
    [SerializeField] private int maxWaveDistance = 16;
    [SerializeField] private int horiOffset = -3;
    [SerializeField] private float spawnOffset = 0f;

    private int waveSpreadCount = 0;
    private int count = 0;
    private int waveMaxCount = 0;
    private int direction = 1;

    public override IEnumerator ExecuteAction(Player player)
    {
        waveSpreadCount = 0;

        for (int j = -maxWaveDistance * 2; j < maxWaveDistance * 2; j++)
        {
            if (Mathf.Abs(j) > sprayGap)
            {
                Projectile p1 = SpawnProjectile(Mathf.PI, spawnOffset);
                p1.transform.position = p1.transform.position + new Vector3(horiOffset, j + waveSpreadCount + 1, 0);
            }
        }

        for (int j = 0; j < numberOfWaves; j++) {

            for(int i = -1; i <= 1; i+=2)
            {
                Projectile p1 = SpawnProjectile(Mathf.PI, spawnOffset);
                p1.transform.position = p1.transform.position + new Vector3(horiOffset, i * sprayGap + waveSpreadCount, 0);
            }
            
            waveSpreadCount += direction;
            HandleWaveDirection();

            yield return new WaitForSeconds(waveDelay);
        }

        yield return new WaitForSeconds(endingDelay);
    }

    private void HandleWaveDirection()
    {
        count++;
        if (waveMaxCount < count || waveSpreadCount > maxWaveDistance || waveSpreadCount < -maxWaveDistance)
        {
            count = 0;
            direction = direction * -1;
            waveMaxCount = Random.Range(minDirectionChange, maxDirectionChange);

            if (direction == 1)
            {
                Projectile p3 = SpawnProjectile(Mathf.PI, spawnOffset);
                p3.transform.position = p3.transform.position + new Vector3(horiOffset, waveSpreadCount, 0);
            }
        }
    }
}
