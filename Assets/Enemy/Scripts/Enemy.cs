using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAction[] enemyActions;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float speed = 100;
    [SerializeField] private float agroRange = 50f;

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

        StartCoroutine(HandleEnemyActions());
    }

    private void Update()
    {
        //Handle interrupts here

        if (Input.GetKeyDown(KeyCode.L))
        {
            Knockback(player.transform.position, 120f, 2f);
        }
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

    public void Knockback(Vector3 sourcePosition, float forceMulti, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(DelayedReset(duration));

        Vector3 knockbackDir = (transform.position - sourcePosition).normalized;
        rb.AddForce(knockbackDir * forceMulti);
        print(rb.velocity);
    }

    private IEnumerator DelayedReset(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        StartCoroutine(HandleEnemyActions());
        rb.velocity = Vector3.zero; //This exists reset the velocity of the enemy
    }

        private IEnumerator Idle()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
