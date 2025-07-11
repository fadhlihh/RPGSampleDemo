using UnityEngine;

public class HUDManager : SingletonBehaviour<HUDManager>
{
    [SerializeField]
    private PlayerStatusUI _playerStatusUI;
    [SerializeField]
    private WeaponSlotUI _weaponSlotUI;

    public PlayerStatusUI PlayerStatusUI { get => _playerStatusUI; }
    public WeaponSlotUI WeaponSlotUI { get => _weaponSlotUI; }

    private void Awake()
    {
        if (!_playerStatusUI)
        {
            _playerStatusUI = FindAnyObjectByType<PlayerStatusUI>();
        }
        if (!_weaponSlotUI)
        {
            _weaponSlotUI = FindAnyObjectByType<WeaponSlotUI>();
        }
    }
}

