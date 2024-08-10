using System.Collections;
using UnityEngine;

public class SpawnAttack : EnemyAttack
{
    [Header("Spawn Attack Configurations")]
    [SerializeField] private float numberOfEnemies = 0.5f;
    [SerializeField] private float endingDelay = 0.0f;
    [SerializeField] private float spawnDelay = 0.5f;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Vector3[] transformationList;

    public override IEnumerator ExecuteAction(Player player)
    {
        for (int j = 0; j < numberOfEnemies; j++)
        {
            Enemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.transform.position = transformationList[Random.Range(0, transformationList.Length)];
            enemy.SetPlayerTarget(player);

            yield return new WaitForSeconds(spawnDelay);
        }

        yield return new WaitForSeconds(endingDelay);
    }
}
