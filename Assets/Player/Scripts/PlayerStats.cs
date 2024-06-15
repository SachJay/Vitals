using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public bool IsAlive { get; private set; }
    public bool IsInvincible { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private float killInvincibilityDuration = 0.2f;
    [SerializeField] private ParticleSystem playerDeathParticlesPrefab;
    [SerializeField] private GameObject visualsGameObject;

    private void Start()
    {
        if (!player.IsOwned)
            return;

        player.PlayerAttack.OnEnemyKilled += PlayerAttack_OnKillEnemy;
        player.PlayerAttack.OnAttackStarted += PlayerAttack_OnAttackStarted;
        player.PlayerAttack.OnAttackEnded += PlayerAttack_OnAttackEnded;

        player.PlayerDash.OnDashStarted += PlayerDash_OnDashStarted;
        player.PlayerDash.OnDashEnded += PlayerDash_OnDashEnded;
    }

    private void PlayerDash_OnDashStarted()
    {
        IsInvincible = true;
    }

    private void PlayerDash_OnDashEnded()
    {
        IsInvincible = false;
    }

    private void PlayerAttack_OnAttackStarted()
    {
        IsInvincible = true;
    }

    private void PlayerAttack_OnAttackEnded()
    {
        IsInvincible = false;
    }

    private void PlayerAttack_OnKillEnemy()
    {
        StartCoroutine(GainTempInvincibility(killInvincibilityDuration));
    }

    private IEnumerator GainTempInvincibility(float invincibilityDuration)
    {
        IsInvincible = true;

        yield return new WaitForSeconds(invincibilityDuration);

        IsInvincible = false;
    }

    public void TakeDamage(IDamageable damager, int damage)
    {
        IsAlive = false;

        PlayDeathParticles(damager.GetTransform().position);
        visualsGameObject.SetActive(false);
    }

    public Transform GetTransform() => transform;

    // TODO: Move out of PlayerStats
    private void PlayDeathParticles(Vector3 attackPosition)
    {
        ParticleSystem deathParticles = Instantiate(playerDeathParticlesPrefab, transform.position, Quaternion.identity);

        Vector3 difference = attackPosition - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, -difference.x) * Mathf.Rad2Deg;
        deathParticles.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(rotationZ, 90.0f, 0));
    }
}
