using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    public delegate void PlayerThrowEvent();
    public PlayerThrowEvent OnThrowObject;

    [SerializeField] private Player player;
    

    private void Awake()
    {
        player.PlayerInputHandler.OnInteractInputStarted += Player_OnInteractInputStarted;
    }

    private void Player_OnInteractInputStarted()
    {
        if (player.PlayerPickup.HeldObject == null)
            return;

        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position).normalized;

        player.PlayerPickup.HeldObject.transform.position = transform.position;
        player.PlayerPickup.HeldObject.SetIsPickupable(false);
        player.PlayerPickup.HeldObject.Throw(direction);

        OnThrowObject?.Invoke();
    }
}
