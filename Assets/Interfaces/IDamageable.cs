using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(IDamageable damager, int damage);

    public Transform GetTransform();
}
