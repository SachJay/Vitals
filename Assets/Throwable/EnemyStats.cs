using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    [SerializeField] private Enemy enemy;

    public Transform GetTransform() => transform;

    public void TakeDamage(IDamageable damager, int damage)
    {
        enemy.Die(damager);
    }

    public void TriggerStun(Vector2 impactPosition)
    {
        enemy.StunEnemy(impactPosition);
    }

    public bool IsAttackResetable()
    {
        return true;
    }
}
