using Cinemachine;
using UnityEngine;

public class PlayerCameraAttacher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        if (virtualCamera == null)
            LogExtension.LogMissingComponent(name, nameof(CinemachineVirtualCamera));
    }

    public void Start()
    {
        virtualCamera.Follow = Player.Instance.transform;
    }
}
