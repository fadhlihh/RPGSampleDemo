using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponEquipmentManager : MonoBehaviour
{
    [SerializeField]
    protected List<Weapon> _playerWeapons = new List<Weapon>();
    [SerializeField]
    protected CombatAnimation _animation;

    protected Dictionary<EWeaponType, Transform> _weaponSockets = new Dictionary<EWeaponType, Transform>();
    protected int _currentWeaponIndex = 0;

    public List<Weapon> PlayerWeapons { get => _playerWeapons; }
    public bool IsAttacking { get; set; }
    public bool IsHeavyAttack { get; set; }

    public UnityEvent<int> OnAttack;
    public UnityEvent OnHeavyAttack;

    protected abstract void InitSocket();

    protected virtual void Start()
    {
        if (!_animation)
        {
            _animation = GetComponent<CombatAnimation>();
        }
        HUDManager.Instance.WeaponSlotUI.SetIcon(_playerWeapons[_currentWeaponIndex].Icon);
        _animation.OnBeginTraceHitAnimation.AddListener(_playerWeapons[_currentWeaponIndex].StartTraceHit);
        _animation.OnEndTraceHitAnimation.AddListener(_playerWeapons[_currentWeaponIndex].StopTraceHit);
        InitSocket();
    }

    public virtual void LightAttack()
    {
        bool isRolling = GetComponent<IRolling>() != null ? GetComponent<IRolling>().IsRolling : false;
        bool isGrounded = GetComponent<CharacterMovement>() != null ? GetComponent<CharacterMovement>().IsGrounded : true;
        if (!IsAttacking && isGrounded && !isRolling)
        {
            IsAttacking = true;
            _playerWeapons[_currentWeaponIndex].LightAttack();
            OnAttack.Invoke(_playerWeapons[_currentWeaponIndex].Combo);
        }
    }

    public virtual void HeavyAttack()
    {
        bool isRolling = GetComponent<IRolling>() != null ? GetComponent<IRolling>().IsRolling : false;
        bool isGrounded = GetComponent<CharacterMovement>() != null ? GetComponent<CharacterMovement>().IsGrounded : true;
        if (!IsAttacking && isGrounded && !isRolling)
        {
            IsAttacking = true;
            IsHeavyAttack = true;
            _playerWeapons[_currentWeaponIndex].HeavyAttack();
            OnHeavyAttack.Invoke();
        }
    }

    public void EndHeavyAttack()
    {
        _playerWeapons[_currentWeaponIndex].ResetDamageModifier();
    }
}
