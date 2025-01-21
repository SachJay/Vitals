using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(IDamageable damager, int damage);

    public void TriggerStun(Vector2 impactPosition);

    public Transform GetTransform();

    public bool IsAttackResetable();

    public Vector2 GetVelocity()
    {
        return Vector2.zero;
    }

}