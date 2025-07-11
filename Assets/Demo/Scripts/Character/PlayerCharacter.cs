using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : Character, IRolling, IDamagable, IStamina
{
    [SerializeField]
    private PlayerWeaponEquipmentManager _weaponEquipmentManager;
    [SerializeField]
    private CharacterDefense _characterDefense;
    [SerializeField]
    private GameObject _impactPrefab;
    [SerializeField]
    private int _staminaRegenSpeed = 1;
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

    public float MaximumStamina { get; private set; } = 100;
    public float Stamina { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        HealthPoint = MaximumHealthPoint;
        Stamina = MaximumStamina;
        if (!_weaponEquipmentManager)
        {
            _weaponEquipmentManager = GetComponent<PlayerWeaponEquipmentManager>();
        }
        if (!_characterDefense)
        {
            _characterDefense = GetComponent<CharacterDefense>();
        }
    }

    private void Update()
    {
        if (!CharacterMovement.IsSprint)
        {
            IncreaseStamina(_staminaRegenSpeed * Time.deltaTime);
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
        InputManager.Instance.OnSwitchWeaponInput += WeaponEquipmentManager.NextWeapon;
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
        InputManager.Instance.OnSwitchWeaponInput -= WeaponEquipmentManager.NextWeapon;
    }

    public void Roll()
    {
        bool isMoving = DirectionalCharacterMovement.MoveDirection.magnitude > 0.01f;
        if (isMoving && !WeaponEquipmentManager.IsAttacking && GetIsStaminaAvailable(40))
        {
            DecreaseStamina(40);
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
            HUDManager.Instance.PlayerStatusUI.SetHealthBarValue(HealthPoint, MaximumHealthPoint);
            if (!CharacterDefense.IsBlocking)
            {
                OnDamage?.Invoke();
            }
            else
            {
                enemyCharacter.BounceBack();
                DecreaseStamina(30);
                if (!GetIsStaminaAvailable(30))
                {
                    CharacterDefense.StopBlock();
                }
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

    public void DecreaseStamina(float value)
    {
        Stamina -= value;
        Stamina = Mathf.Clamp(Stamina, 0, 100);
        HUDManager.Instance.PlayerStatusUI.SetStaminaBarValue(Stamina, MaximumStamina);
    }

    public void IncreaseStamina(float value)
    {
        Stamina += value;
        Stamina = Mathf.Clamp(Stamina, 0, 100);
        HUDManager.Instance.PlayerStatusUI.SetStaminaBarValue(Stamina, MaximumStamina);
    }

    public bool GetIsStaminaAvailable(int value)
    {
        return Stamina > value;
    }
}
