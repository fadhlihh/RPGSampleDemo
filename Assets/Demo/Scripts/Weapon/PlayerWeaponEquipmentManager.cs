using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponEquipmentManager : WeaponEquipmentManager
{
    [SerializeField]
    private Transform _swordSocket;
    [SerializeField]
    private Transform _bowSocket;

    public UnityEvent<EWeaponType> OnSwitchWeapon;

    protected override void InitSocket()
    {
        if (_swordSocket != null)
        {
            _weaponSockets.Add(EWeaponType.Melee, _swordSocket);
        }

        if (_bowSocket != null)
        {
            _weaponSockets.Add(EWeaponType.Range, _bowSocket);
        }
    }

    public void NextWeapon()
    {
        if (!IsAttacking && !IsHeavyAttack)
        {
            _weaponSockets[_playerWeapons[_currentWeaponIndex].Type].gameObject.SetActive(false);
            _animation.OnBeginTraceHitAnimation.RemoveListener(_playerWeapons[_currentWeaponIndex].StartTraceHit);
            _animation.OnEndTraceHitAnimation.RemoveListener(_playerWeapons[_currentWeaponIndex].StopTraceHit);
            if (_currentWeaponIndex < _playerWeapons.Count - 1)
            {
                _currentWeaponIndex += 1;
            }
            else
            {
                _currentWeaponIndex = 0;
            }
            HUDManager.Instance.WeaponSlotUI.SetIcon(_playerWeapons[_currentWeaponIndex].Icon);
            _animation.OnBeginTraceHitAnimation.AddListener(_playerWeapons[_currentWeaponIndex].StartTraceHit);
            _animation.OnEndTraceHitAnimation.AddListener(_playerWeapons[_currentWeaponIndex].StopTraceHit);
            _weaponSockets[_playerWeapons[_currentWeaponIndex].Type].gameObject.SetActive(true);
            OnSwitchWeapon?.Invoke(_playerWeapons[_currentWeaponIndex].Type);
        }
    }

    public override void LightAttack()
    {
        bool isRolling = GetComponent<IRolling>() != null ? GetComponent<IRolling>().IsRolling : false;
        bool isGrounded = GetComponent<CharacterMovement>() != null ? GetComponent<CharacterMovement>().IsGrounded : true;
        IStamina staminaSystem = GetComponent<IStamina>();
        bool isStaminaAvailable = staminaSystem != null ? GetComponent<IStamina>().GetIsStaminaAvailable(20) : true;
        if (!IsAttacking && isGrounded && !isRolling && isStaminaAvailable)
        {
            IsAttacking = true;
            _playerWeapons[_currentWeaponIndex].LightAttack();
            OnAttack.Invoke(_playerWeapons[_currentWeaponIndex].Combo);
            staminaSystem?.DecreaseStamina(20);
        }
    }

    public override void HeavyAttack()
    {
        bool isRolling = GetComponent<IRolling>() != null ? GetComponent<IRolling>().IsRolling : false;
        bool isGrounded = GetComponent<CharacterMovement>() != null ? GetComponent<CharacterMovement>().IsGrounded : true;
        IStamina staminaSystem = GetComponent<IStamina>();
        bool isStaminaAvailable = staminaSystem != null ? GetComponent<IStamina>().GetIsStaminaAvailable(40) : true;
        if (!IsAttacking && isGrounded && !isRolling && isStaminaAvailable)
        {
            IsAttacking = true;
            IsHeavyAttack = true;
            _playerWeapons[_currentWeaponIndex].HeavyAttack();
            OnHeavyAttack.Invoke();
            staminaSystem?.DecreaseStamina(40);
        }
    }
}
