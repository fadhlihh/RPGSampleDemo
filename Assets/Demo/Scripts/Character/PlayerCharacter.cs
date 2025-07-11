using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : Character, IRolling
{
    [SerializeField]
    private PlayerWeaponEquipmentManager _weaponEquipmentManager;
    [SerializeField]
    private UnityEvent _onCharacterRoll;

    public DirectionalCharacterMovement DirectionalCharacterMovement { get => CharacterMovement as DirectionalCharacterMovement; }
    public PlayerWeaponEquipmentManager WeaponEquipmentManager { get { return _weaponEquipmentManager; } }
    public UnityEvent OnCharacterRoll => _onCharacterRoll;
    public bool IsRolling { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (!_weaponEquipmentManager)
        {
            _weaponEquipmentManager = GetComponent<PlayerWeaponEquipmentManager>();
        }
    }

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
        InputManager.Instance.OnRollInput += Roll;
        InputManager.Instance.OnLightAttackInput += WeaponEquipmentManager.LightAttack;
        InputManager.Instance.OnHeavyAttackInput += WeaponEquipmentManager.HeavyAttack;
    }

    private void UnBindingInput()
    {
        InputManager.Instance.OnMoveInput -= DirectionalCharacterMovement.AddMovementInput;
        InputManager.Instance.OnSprintInput -= DirectionalCharacterMovement.Sprint;
        InputManager.Instance.OnRollInput -= Roll;
        InputManager.Instance.OnLightAttackInput -= WeaponEquipmentManager.LightAttack;
        InputManager.Instance.OnHeavyAttackInput -= WeaponEquipmentManager.HeavyAttack;
    }

    public void Roll()
    {
        bool isMoving = DirectionalCharacterMovement.MoveDirection.magnitude > 0.01f;
        if (isMoving)
        {
            DirectionalCharacterMovement.IsAbleToMove = false;
            IsRolling = true;
            Transform cameraTransform = Camera.main.transform;
            float rotationAngle = GameHelper.GetRotationAngleFromInput(DirectionalCharacterMovement.MoveDirection.x, DirectionalCharacterMovement.MoveDirection.z) + cameraTransform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
            OnCharacterRoll?.Invoke();
        }
    }

    public void EndRoll()
    {
        IsRolling = false;
        DirectionalCharacterMovement.IsAbleToMove = true;
    }
}
