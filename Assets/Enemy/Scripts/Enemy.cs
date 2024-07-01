using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAttack[] enemyAttack;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float speed = 100;

    private Player player;

    private void Start()
    {
        // TODO: Update to determine which player it should be targeting (right now it targets a random player in this list)
        if (player == null && CustomNetworkManager.Instance != null)
        {
            int randomIndex = Random.Range(0, CustomNetworkManager.Instance.GamePlayers.Count);
            PlayerObjectController playerObjectController = CustomNetworkManager.Instance.GamePlayers[randomIndex];

            if (!playerObjectController.TryGetComponent(out player))
                LogExtension.LogMissingComponent(name, nameof(Player));
        }

        StartCoroutine(StartAttacks());
    }

    public void SetPlayerTarget(Player player)
    {
        this.player = player;
    }

    private IEnumerator StartAttacks()
    {
        while (true)
        {
            for (int i = 0; i < enemyAttack.Length; i++)
            {
                if (player != null)
                    yield return StartCoroutine(enemyAttack[i].ExecuteAttack(player));
            }

            yield return new WaitForSeconds(attackDelay);
        }
    }

    private void Update()
    {
        Vector2 targetDirection = (player.transform.position - transform.position).normalized;

        rb.MovePosition(rb.position + targetDirection * speed);

        Vector3 difference = player.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ + 90);
    }
}
