using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAction[] enemyActions;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float speed = 100;
    [SerializeField] private float agroRange = 50f;

    [SerializeField] private float stunDuration = 1f;
    [SerializeField] private float knockbackForce = 50f;

    private Player player;

    private Coroutine enemyActionCorouter;

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

        enemyActionCorouter = StartCoroutine(HandleEnemyActions());
    }

    public void StunEnemy(Vector2 impactPosition)
    {
        StartCoroutine(StunTimer(impactPosition));
    }

    private IEnumerator StunTimer(Vector2 impactPosition)
    {
        if (enemyActionCorouter != null)
            StopCoroutine(enemyActionCorouter);

        Vector2 diff = ((Vector2) gameObject.transform.position - impactPosition);
        rb.AddForce(diff * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(stunDuration);

        rb.velocity = Vector2.zero;

        enemyActionCorouter = StartCoroutine(HandleEnemyActions());
    }

    private void Update()
    {
        //Handle interrupts here
    }

    public void SetPlayerTarget(Player player)
    {
        this.player = player;
    }

    private IEnumerator HandleEnemyActions()
    {
        while (true)
        {
            if (player == null)
                yield return null;

            if (Vector2.Distance(transform.position, player.transform.position) < agroRange)
            {
                for (int i = 0; i < enemyActions.Length; i++)
                {
                    yield return StartCoroutine(enemyActions[i].ExecuteAction(player));
                }

                yield return new WaitForSeconds(attackDelay);
            } else
            {
                yield return StartCoroutine(Idle()); 
            }
        }
    }

    public void MoveTowards(Vector3 target, float speedMultiplayer)
    {
        Vector2 targetDirection = (target - transform.position).normalized;

        rb.MovePosition(rb.position + targetDirection * speed * speedMultiplayer);

        Vector3 difference = target - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ + 90);
    }

    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
