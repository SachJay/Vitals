using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //[SerializeField]
    public Player player;

    [SerializeField]
    EnemyAttack[] enemyAttack;

    [SerializeField]
    float attackDelay = 1f;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    float speed = 100;

    //[SerializeField]
    public bool transitionOnDeath = false;

    private void Start()
    {
        StartCoroutine("StartAttacks");
    }

    IEnumerator StartAttacks()
    {
        while (true)
        {
            for(int i = 0; i < enemyAttack.Length; i++)
            {
                yield return StartCoroutine(enemyAttack[i].ExecuteAttack(player));
            }

            yield return new WaitForSeconds(attackDelay);
        }
    }

    private void Update()
    {
        Vector2 targetDir = (player.transform.position - transform.position).normalized;

        rb.MovePosition(rb.position + targetDir * speed);

        Vector3 difference = player.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ + 90);

    }
}
