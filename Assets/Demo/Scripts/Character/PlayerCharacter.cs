using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : Character, IRolling, IDamagable
{
    [SerializeField]
    private PlayerWeaponEquipmentManager _weaponEquipmentManager;
    [SerializeField]
    private CharacterDefense _characterDefense;
    [SerializeField]
    private GameObject _impactPrefab;
    [SerializeField]
    private UnityEvent _onCharacterRoll;

    public UnityEvent OnDamage;
    public UnityEvent OnDeath;

    public DirectionalCharacterMovement DirectionalCharacterMovement { get => CharacterMovement as DirectionalCharacterMovement; }
    public PlayerWeaponEquipmentManager WeaponEquipmentManager { get { return _weaponEquipmentManager; } }
    public CharacterDefense CharacterDefense { get => _characterDefense; }
    public UnityEvent OnCharacterRoll => _onCharacterRoll;
    public bool IsRolling { get; private set; }

    public int MaximumHealthPoint { get; private set; } = 100;
    public int HealthPoint { get; private set; }
    public bool IsDead { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        HealthPoint = MaximumHealthPoint;
        if (!_weaponEquipmentManager)
        {
            _weaponEquipmentManager = GetComponent<PlayerWeaponEquipmentManager>();
        }
        if (!_characterDefense)
        {
            _characterDefense = GetComponent<CharacterDefense>();
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
        InputManager.Instance.OnStartBlockInput += CharacterDefense.StartBlock;
        InputManager.Instance.OnStopBlockInput += CharacterDefense.StopBlock;
    }

    private void UnBindingInput()
    {
        InputManager.Instance.OnMoveInput -= DirectionalCharacterMovement.AddMovementInput;
        InputManager.Instance.OnSprintInput -= DirectionalCharacterMovement.Sprint;
        InputManager.Instance.OnRollInput -= Roll;
        InputManager.Instance.OnLightAttackInput -= WeaponEquipmentManager.LightAttack;
        InputManager.Instance.OnHeavyAttackInput -= WeaponEquipmentManager.HeavyAttack;
        InputManager.Instance.OnStartBlockInput -= CharacterDefense.StartBlock;
        InputManager.Instance.OnStopBlockInput -= CharacterDefense.StopBlock;
    }

    public void Roll()
    {
        bool isMoving = DirectionalCharacterMovement.MoveDirection.magnitude > 0.01f;
        if (isMoving && !WeaponEquipmentManager.IsAttacking)
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

    public void Damage(DamageData damageData)
    {
        if (!IsDead)
        {
            EnemyCharacter enemyCharacter = damageData.Instigator as EnemyCharacter;
            HealthPoint -= (damageData.HitPoint + (CharacterDefense.IsBlocking ? CharacterDefense.DamageModifier : 0));
            if (!CharacterDefense.IsBlocking)
            {
                OnDamage?.Invoke();
            }
            else
            {
                enemyCharacter.BounceBack();
            }
            if (HealthPoint <= 0)
            {
                Death();
            }
            Instantiate(_impactPrefab, damageData.HitImpactPosition, Quaternion.identity);
        }
    }

    public void Death()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }

    public void DestroyCharacter()
    {
        Destroy(gameObject, 3);
    }

    public void Heal(int value)
    {

    }
}
