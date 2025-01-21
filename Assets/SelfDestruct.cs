using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : EnemyAction
{

    [SerializeField] GameObject self;

    public override IEnumerator ExecuteAction(Player player)
    {
        yield return null;

        Destroy(self);
    }
}
