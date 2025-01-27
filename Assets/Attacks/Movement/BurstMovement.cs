using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstMovement : EnemyAction
{
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] float speed = 1;
    [SerializeField] float randomDir = 10;
    [SerializeField] AttackDirection attackDirection = AttackDirection.Towards;

    enum AttackDirection
    {
        Towards,
        Away,
        Perpendicular
    }

    public override IEnumerator ExecuteAction(Player player)
    {
        float directionIntentionMultiplyer = attackDirection == AttackDirection.Towards ? -1 : 1;
        Vector2 targetDir = (transform.position - player.transform.position) * directionIntentionMultiplyer;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) + Random.Range(randomDir * Mathf.Deg2Rad, randomDir * Mathf.Deg2Rad);
        Vector2 directionVector = new(Mathf.Cos(angle), Mathf.Sin(angle));

        rigidbody.AddForce(directionVector.normalized * speed, ForceMode2D.Impulse);

        yield return null;
    }
}
