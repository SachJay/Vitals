using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    protected bool dashable = true;

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

        if (other.transform.gameObject.TryGetComponent(out Player player))
        {
            if ((!player.PlayerStats.IsInvincible) || !dashable)
            {
                //Destroy(other.transform.parent.gameObject);
                player.PlayerStats.TakeDamage(null, 1);
                Destroy(gameObject);
            }
        }

    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetDuration(float duration)
    {
        Destroy(gameObject, duration);
    }

    public void SetDirection(Vector2 direction)
    {
        rb.AddForce(direction.normalized * speed);
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
}
