using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Camera playerCamera;

    private Vector3 cameraPosition = new(0, 0, -10);

    private void Awake()
    {
        if (playerCamera == null)
            LogExtension.LogMissingVariable(name, nameof(playerCamera));
    }

    private void LateUpdate()
    {
        if (!player.IsOwned)
            return;

        if (playerCamera == null)
            return;

        cameraPosition.x = 0;
        cameraPosition.y = transform.position.y;
        cameraPosition.z = -10;
    }

}
