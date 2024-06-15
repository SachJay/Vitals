using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [HideInInspector] public PlayerInputHandler PlayerInputHandler;
    [HideInInspector] public PlayerStats PlayerStats;
    [HideInInspector] public PlayerMovement PlayerMovement;
    [HideInInspector] public PlayerAttack PlayerAttack;
    [HideInInspector] public PlayerDash PlayerDash;

    public bool IsOwned => isOwned;

    private void Awake()
    {
        if (!IsOwned)
            return;

        InitComponentReferences();
    }

    private void InitComponentReferences()
    {
        if (TryGetComponent(out PlayerInputHandler playerInputHandler))
            PlayerInputHandler = playerInputHandler;
        else
            LogExtension.LogMissingVariable(name, nameof(PlayerInputHandler));

        if (TryGetComponent(out PlayerStats playerStats))
            PlayerStats = playerStats;
        else
            LogExtension.LogMissingVariable(name, nameof(PlayerStats));

        if (TryGetComponent(out PlayerAttack playerAttack))
            PlayerAttack = playerAttack;
        else
            LogExtension.LogMissingVariable(name, nameof(PlayerAttack));

        if (TryGetComponent(out PlayerDash playerDash))
            PlayerDash = playerDash;
        else
            LogExtension.LogMissingVariable(name, nameof(PlayerAttack));
    }
}
