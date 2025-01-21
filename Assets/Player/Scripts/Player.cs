using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerInputHandler PlayerInputHandler;
    [HideInInspector] public PlayerMovement PlayerMovement;

    public PlayerStats PlayerStats;
    public PlayerAttack PlayerAttack;
    public PlayerDash PlayerDash;
    public PlayerPickup PlayerPickup;
    public PlayerThrow PlayerThrow;
    public PlayerSpell PlayerSpell;

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
