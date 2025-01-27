using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAttack : EnemyAction
{
    EnemyAttack attack;
    EnemyAction movement;

    public override IEnumerator ExecuteAction(Player player)
    {
        movement.ExecuteAction(player);
        attack.ExecuteAction(player);

        yield return null;
    }
}
