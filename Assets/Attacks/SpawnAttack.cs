using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAttack : EnemyAttack
{
    [SerializeField]
    float projectileNumbers = 5;

    [SerializeField]
    float numberOfEnemies = 0.5f;

    [SerializeField]
    float endingDelay = 0.0f;

    [SerializeField]
    float spawnDelay = 0.5f;

    [SerializeField]
    Enemy enemyPrefab;

    [SerializeField]
    Vector3[] transformationList;

    [SerializeField]
    Player player;

    public override IEnumerator ExecuteAttack(Player player)
    {
        for (int j = 0; j < numberOfEnemies; j++)
        {
            Enemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.transform.position = transformationList[Random.Range(0, transformationList.Length)];
            enemy.player = player;

            yield return new WaitForSeconds(spawnDelay);
        }

        yield return new WaitForSeconds(endingDelay);
    }
    
}
