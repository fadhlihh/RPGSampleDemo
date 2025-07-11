using UnityEngine;

public class PlayerCharacter : Character
{
    public DirectionalCharacterMovement DirectionalCharacterMovement { get => CharacterMovement as DirectionalCharacterMovement; }

    private void OnEnable()
    {
        BindingInput();
    }

    private void OnDisable()
    {
        UnBindingInput();
    }

    private void BindingInput()
    {
        InputManager.Instance.SetGeneralInputEnabled(true);
        InputManager.Instance.OnMoveInput += DirectionalCharacterMovement.AddMovementInput;
        InputManager.Instance.OnSprintInput += DirectionalCharacterMovement.Sprint;
    }

    private void UnBindingInput()
    {
        InputManager.Instance.SetGeneralInputEnabled(true);
        InputManager.Instance.OnMoveInput -= DirectionalCharacterMovement.AddMovementInput;
        InputManager.Instance.OnSprintInput -= DirectionalCharacterMovement.Sprint;
    }
}
