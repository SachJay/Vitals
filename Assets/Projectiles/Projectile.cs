using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;

    public float speed = 1;

    [SerializeField]
    float duration = 5;

    [SerializeField]
    bool contactDamage = true;

    private void Start()
    {
        StartCoroutine("Die");
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!contactDamage)
            return;

        if (other.transform != null && other.transform.parent != null && other.transform.parent.parent != null && other.transform.parent.parent.gameObject.TryGetComponent(out Player player))
        {
            if ((!player.PlayerStats.IsInvincible) || CompareTag("Undodgable"))
            {
                //Destroy(other.transform.parent.gameObject);
                // TODO: Add IDamageable and damage
                player.PlayerStats.TakeDamage(null, 1);
                Destroy(gameObject);
            }
        }
    }

    public void SetDirection(Vector2 direction)
    {
        rb.AddForce(direction.normalized * speed);
    }
}
