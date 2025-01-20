using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : EnemyAction
{
    [SerializeField] GameObject enemy;
    public override IEnumerator ExecuteAction(Player player)
    {
        yield return null;

        Destroy(enemy);
    }
}
