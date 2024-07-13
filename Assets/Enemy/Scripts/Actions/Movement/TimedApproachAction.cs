using System.Collections;
using UnityEngine;

public class TimedApproachAction : MovementAction
{
    bool hasTimeLeft = true;
    [SerializeField, Tooltip("Time the action will execute for in seconds")] protected float actionTime = 1f;

    public override IEnumerator ExecuteAction(Player player)
    {
        StartCoroutine(StopTimer());
        while (hasTimeLeft)
        {
            enemy.MoveTowards(player.transform.position, speedMultiplayer);

            yield return null;
        }

        yield return null;
    }

    private IEnumerator StopTimer()
    {
        yield return new WaitForSeconds(actionTime);
        hasTimeLeft = false;
    }
}
