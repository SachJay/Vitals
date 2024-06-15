using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [HideInInspector] public PlayerInputHandler PlayerInputHandler;
    [HideInInspector] public PlayerMovement PlayerMovement;

    public PlayerStats PlayerStats;
    public PlayerAttack PlayerAttack;
    public PlayerDash PlayerDash;

    public bool IsOwned => isOwned;

    private void Awake()
    {
        InitComponentReferences();
    }

    private void InitComponentReferences()
    {
        if (TryGetComponent(out PlayerInputHandler playerInputHandler))
            PlayerInputHandler = playerInputHandler;
        else
            LogExtension.LogMissingVariable(name, nameof(PlayerInputHandler));

        if (TryGetComponent(out PlayerMovement playerMovement))
            PlayerMovement = playerMovement;
        else
            LogExtension.LogMissingVariable(name, nameof(PlayerMovement));
    }
}
