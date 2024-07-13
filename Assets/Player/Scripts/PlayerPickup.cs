using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private Transform interactionPoint;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private float interactionRadius = 1f;

    public Throwable HeldObject { get; private set; }

    private void Awake()
    {
        player.PlayerInputHandler.OnInteractInputEnded += Player_OnInteractInputEnded;
        player.PlayerThrow.OnThrowObject += Player_OnThrowObject;
    }

    private void Player_OnInteractInputEnded()
    {
        if (HeldObject != null)
            return;

        PickupObject();
    }

    private void Player_OnThrowObject()
    {
        HeldObject = null;
    }

    private void PickupObject()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            interactionPoint.position,
            interactionRadius,
            interactionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out IInteractable interactable))
            {
                if (interactable.GetGameObject().TryGetComponent(out Throwable throwable) && throwable.IsPickupable)
                {
                    interactable.Interact();
                    HeldObject = throwable;
                }
            }
        }
    }
}
