using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected string _name;
    [SerializeField]
    protected Sprite _icon;
    [SerializeField]
    protected int _damage;
    [SerializeField]
    private Transform _socket;
    [SerializeField]
    private Character _ownerCharacter;

    public virtual EWeaponType Type { get; }
    public string Name { get => _name; }
    public Sprite Icon { get => _icon; }
    public int Damage { get => _damage; }

    private void Start()
    {
        if (!_ownerCharacter)
        {
            _ownerCharacter = GetComponentInParent<Character>();
        }
    }
}
