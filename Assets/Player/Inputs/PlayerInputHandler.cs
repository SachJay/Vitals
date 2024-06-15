using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public delegate void PlayerInputEvent();
    public PlayerInputEvent OnAttackInputStarted;
    public PlayerInputEvent OnDashInputStarted;

    public Vector2 Move { get; private set; } = Vector2.zero;

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Move = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            Move = Vector2.zero;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnAttackInputStarted?.Invoke();
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnDashInputStarted?.Invoke();
        }
    }
}
