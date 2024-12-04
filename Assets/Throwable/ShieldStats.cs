using UnityEngine;

public class ShieldStats : MonoBehaviour, IDamageable
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private float strength = 0.2f;
    public Transform GetTransform() => transform;

    public void TakeDamage(IDamageable damager, int damage)
    {
        Vector2 dir = damager.GetVelocity().normalized;
        float currentSpeed = rigidbody.velocity.magnitude;

        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(dir * (currentSpeed + strength), ForceMode2D.Impulse);
    }

    public void TriggerStun(Vector2 impactPosition)
    {
        Vector2 dir = (Vector2)transform.position - impactPosition;
        float currentSpeed = rigidbody.velocity.magnitude;

        rigidbody.AddForce(dir * (currentSpeed + strength), ForceMode2D.Impulse);
    }

    public bool IsAttackResetable()
    {
        return true;
    }
}
