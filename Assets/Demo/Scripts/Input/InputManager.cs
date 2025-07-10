using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static IA_Default;

public class InputManager : Singleton<InputManager>, IGeneralActions
{
    public Action<Vector2> OnMoveInput;
    public Action<bool> OnSprintInput;

    private IA_Default _inputAction;
    public InputManager()
    {
        if (_inputAction == null)
        {
            _inputAction = new IA_Default();
            _inputAction.General.SetCallbacks(this);
        }
    }

    public void SetGeneralInputEnabled(bool value)
    {
        if (value)
        {
            _inputAction.General.Enable();
        }
        else
        {
            _inputAction.General.Disable();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        OnMoveInput?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSprintInput?.Invoke(true);
        }
        if (context.canceled)
        {
            OnSprintInput?.Invoke(false);
        }
    }
}
