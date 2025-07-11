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
            _animation.OnBeginTraceHitAnimation.AddListener(_playerWeapons[_currentWeaponIndex].StartTraceHit);
            _animation.OnEndTraceHitAnimation.AddListener(_playerWeapons[_currentWeaponIndex].StopTraceHit);
            _weaponSockets[_playerWeapons[_currentWeaponIndex].Type].gameObject.SetActive(true);
            OnSwitchWeapon?.Invoke(_playerWeapons[_currentWeaponIndex].Type);
        }
    }
}
