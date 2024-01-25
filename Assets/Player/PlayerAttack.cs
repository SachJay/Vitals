using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    Player player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player.IsAttacking)
        {
            if (other.gameObject.TryGetComponent(out Enemy enemy) && enemy.transitionOnDeath)
            {
                StartCoroutine(LoadNewScene());
            }

            handleDeathParticles(other.gameObject);
            Destroy(other.gameObject);

            Vector2 dashLocation = (player.rb.velocity).normalized;
            player.rb.velocity = Vector2.zero;
            player.rb.AddForce(dashLocation * player.knockbackForce, ForceMode2D.Force);
            player.ResetDashes();
        }
    }

    void handleDeathParticles(GameObject enemy)
    {
        ParticleSystem deathParticles = Instantiate(player.enemyDeathParticlesPrefab, transform.position, Quaternion.identity);

        Vector3 difference = player.transform.position - enemy.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, -difference.x) * Mathf.Rad2Deg;
        deathParticles.transform.rotation = Quaternion.Euler(rotationZ, 90.0f, 0);
        deathParticles.transform.position = enemy.transform.position;
    }

    IEnumerator LoadNewScene()
    {
        print("Load Next Scene");
        yield return new WaitForSeconds(2);
        player.LoadNextScene();
    }
}
