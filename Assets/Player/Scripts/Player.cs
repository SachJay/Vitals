using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player Instance { get; private set; }

    [HideInInspector] public PlayerInputHandler PlayerInputHandler;
    [HideInInspector] public PlayerMovement PlayerMovement;

    public PlayerStats PlayerStats;
    public PlayerAttack PlayerAttack;
    public PlayerDash PlayerDash;
    public PlayerPickup PlayerPickup;
    public PlayerThrow PlayerThrow;

    public bool IsOwned => isOwned;

    public override void OnStartLocalPlayer()
    {
        if (isOwned)
            Instance = this;
    }

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
