using UnityEngine;

public class PlayerRevive : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private SpriteRendererTransparencyUI spriteRendererTransparencyUI;
    [SerializeField] private TransformSizeAnimator transformSizeAnimator;

    [Header("Revive Configurations")]
    [SerializeField] private float reviveTime = 3.0f;

    private bool isReviving = false;
    private Player revivingPlayer = null;
    private float timer = 0.0f;

    private void Awake()
    {
        if (playerStats == null)
            LogExtension.LogMissingComponent(name, nameof(PlayerStats));

        if (spriteRendererTransparencyUI == null)
            LogExtension.LogMissingComponent(name, nameof(SpriteRendererTransparencyUI));

        if (transformSizeAnimator == null)
            LogExtension.LogMissingComponent(name, nameof(TransformSizeAnimator));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerStats.IsDead)
            return;

        if (collision.TryGetComponent(out Player otherPlayer) && revivingPlayer == null && !isReviving && !otherPlayer.PlayerStats.IsDead)
        {
            revivingPlayer = otherPlayer;
            StartRevive();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!playerStats.IsDead)
            return;

        if (collision.TryGetComponent(out Player otherPlayer) && revivingPlayer == otherPlayer && isReviving)
        {
            StopRevive();
        }
    }

    private void Update()
    {
        if (!isReviving)
            return;

        timer += Time.deltaTime;
        spriteRendererTransparencyUI.SetTransparency(timer / reviveTime);

        if (timer > reviveTime)
            FinishRevive();
    }

    private void StartRevive()
    {
        if (revivingPlayer != null)
            revivingPlayer.PlayerStats.OnDie += StopRevive;
        isReviving = true;
    }

    private void StopRevive()
    {
        if (revivingPlayer != null)
            revivingPlayer.PlayerStats.OnDie -= StopRevive;
        revivingPlayer = null;
        isReviving = false;
    }

    private void FinishRevive()
    {
        if (revivingPlayer != null)
            revivingPlayer.PlayerStats.OnDie -= StopRevive;
        revivingPlayer = null;

        isReviving = false;

        spriteRendererTransparencyUI.SetTransparency(1.0f);
        transformSizeAnimator.StartAnimation();
        playerStats.Revive();
    }
}
