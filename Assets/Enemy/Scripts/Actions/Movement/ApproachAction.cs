using System.Collections;
using UnityEngine;

public class ApproachAction : MovementAction
{
    [SerializeField, Tooltip("Distance the enemy needs to approach to end action")] protected float endTriggerRange = 20;

    public override IEnumerator ExecuteAction(Player player)
    {

        while (Vector2.Distance(player.transform.position, enemy.transform.position) > endTriggerRange)
        {
            enemy.MoveTowards(player.transform.position, speedMultiplayer);

            yield return null;
        }

        yield return null;
    }
}
